using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Caches;
using NHibernate.Linq;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.User
{
    public partial class UserMessagesGridControl : BaseUserControlExtend<UserMessagesModel>
    {
        public event EventHandler<GenericEventArgs<Guid>> Approve;
        protected virtual void OnApprove(GenericEventArgs<Guid> e)
        {
            if (Approve != null)
                Approve(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Reject;
        protected virtual void OnReject(GenericEventArgs<Guid> e)
        {
            if (Reject != null)
                Reject(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> MarkAsRead;
        protected virtual void OnMarkAsRead(GenericEventArgs<Guid> e)
        {
            if (MarkAsRead != null)
                MarkAsRead(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnApprove_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnApprove(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnReject_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnReject(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnMarkAsRead_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnMarkAsRead(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            var model = e.Row.DataItem as UserMessageModel;
            if (model == null)
                return;

            if (UmUtil.Instance.HasAccess("Admin"))
            {
                if (model.StatusID == DataStatusCache.Rejected.ID)
                    e.Row.BackColor = Color.FromArgb(0, 255, 217, 191);
                else if (model.StatusID == DataStatusCache.Accepted.ID)
                    e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);
            }
            else
            {
                var userID = UserUtil.GetCurrentUserID();

                if (model.ToUserID == userID && !model.Readed.GetValueOrDefault())
                    e.Row.BackColor = Color.FromArgb(0, 255, 233, 135);
                else
                    e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);
            }
        }

        protected bool GetApproveVisible()
        {
            return UmUtil.Instance.HasAccess("Admin");
        }

        protected bool GetRejectVisible()
        {
            return UmUtil.Instance.HasAccess("Admin");
        }

        protected bool GetMarkAsReadVisible()
        {
            return !UmUtil.Instance.HasAccess("Admin");
        }

        protected String GetFormName(object value)
        {
            var formID = DataConverter.ToNullableGuid(value);
            if (formID == null)
                return String.Empty;

            var formsCache = CommonObjectCache.InitObject("Forms", CommonCacheStore.Request, GetFormNames);
            return formsCache.GetValueOrDefault(formID.Value);
        }

        protected String GetUserName(object value)
        {
            var userID = DataConverter.ToNullableGuid(value);
            if (userID == null)
                return null;

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            var name = $"{user.LoginName} - {user.FirstName} {user.LastName}";
            return name;
        }

        protected IDictionary<Guid?, String> GetFormNames()
        {
            var query = (from n in HbSession.Query<GM_Form>()
                         where n.DateDeleted == null
                         select new IDNameEntity
                         {
                             ID = n.ID,
                             Name = n.Name
                         });

            var dict = new Dictionary<Guid?, String>();

            foreach (var item in query)
                dict.Add(item.ID, item.Name);

            return dict;
        }


    }
}