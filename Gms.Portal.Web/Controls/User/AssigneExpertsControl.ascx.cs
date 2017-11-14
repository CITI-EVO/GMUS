using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using Gms.Portal.Web.Enums;

namespace Gms.Portal.Web.Controls.User
{
    public partial class AssigneExpertsControl : BaseUserControlExtend<AssigneExpertsModel>
    {
        public event EventHandler<GenericEventArgs<Guid>> Attach;
        protected virtual void OnAttach(GenericEventArgs<Guid> e)
        {
            if (Attach != null)
                Attach(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Detach;
        protected virtual void OnDetach(GenericEventArgs<Guid> e)
        {
            if (Detach != null)
                Detach(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Status;
        protected virtual void OnStatus(GenericEventArgs<Guid> e)
        {
            if (Status != null)
                Status(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Email;
        protected virtual void OnEmail(GenericEventArgs<Guid> e)
        {
            if (Email != null)
                Email(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAttach_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnAttach(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnDetach_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnDetach(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnStatus_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnStatus(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnEmail_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnEmail(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            var descriptor = e.Row.DataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.UserIDField));
            if (userID == null)
                return;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return;

            var myStatuses = (from n in userStatuses
                              where n.UserID == userID &&
                                    n.Params != null &&
                                    n.Params.ContainsKey(FormDataConstants.ScoringParams)
                              select n);

            var myStatus = myStatuses.FirstOrDefault();
            if (myStatus == null)
                e.Row.BackColor = Color.FromArgb(0, 237, 234, 230);
            else
                e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);
        }

        protected bool GetViewVisible(object dataItem)
        {
            return true;
        }

        protected bool GetAttachVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue("UserID"));
            if (userID == null)
                return false;

            var userStatuses = descriptor.GetValue("UserStatusesFields") as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return true;

            var query = (from n in userStatuses
                         where n.UserID == userID
                         select n);

            return !query.Any();
        }

        protected bool GetDetachVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue("UserID"));
            if (userID == null)
                return false;

            var userStatuses = descriptor.GetValue("UserStatusesFields") as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return false;

            var query = (from n in userStatuses
                         where n.UserID == userID
                         select n);

            return query.Any();
        }

        protected bool GetStatusVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue("UserID"));
            if (userID == null)
                return false;

            var userStatuses = descriptor.GetValue("UserStatusesFields") as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return false;

            var query = (from n in userStatuses
                         where n.UserID == userID
                         select n);

            return query.Any();
        }

        protected bool GetEmailVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue("UserID"));
            if (userID == null)
                return false;

            var userStatuses = descriptor.GetValue("UserStatusesFields") as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return false;

            var query = (from n in userStatuses
                         where n.UserID == userID
                         select n);

            return query.Any();
        }

        protected Object GetCommandArg(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.UserIDField));
            return userID;
        }

        protected String GetProfileUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return "#";

            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = Convert.ToString(FormMode.View),
                ["FormID"] = descriptor.GetValue("FormID"),
                ["OwnerID"] = descriptor.GetValue("FormID"),
                ["RecordID"] = descriptor.GetValue("RecordID"),
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            return urlHelper.ToEncodedUrl();
        }
    }
}