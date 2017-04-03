using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Controls;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using Label = System.Web.UI.WebControls.Label;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormDataGridControl : BaseUserControlExtend<FormDataGridModel>
    {
        public event EventHandler<GenericEventArgs<Guid>> Status;
        protected virtual void OnStatus(GenericEventArgs<Guid> e)
        {
            if (Status != null)
                Status(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnStatus_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnStatus(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var descriptor = e.Row.DataItem as DictionaryItemDescriptor;

                if (descriptor != null)
                {
                    var statusId = DataConverter.ToNullableGuid(descriptor.GetValue("StatusID"));
                    if (statusId == DataStatusCache.Submit.ID)
                    {
                        e.Row.BackColor = Color.LightPink;
                    }
                }
            }
        }

        public override void SetModel(FormDataGridModel model)
        {
            var columns = gvData.Columns;

            var existFields = columns.OfType<BoundField>().Select(n => n.DataField).ToHashSet();

            foreach (var field in model.Fields)
            {
                var dataField = Convert.ToString(field.ID);
                if (existFields.Contains(dataField))
                    continue;

                var column = new GridViewBoundField
                {
                    HeaderText = field.Name.TrimLen(25),
                    DataField = Convert.ToString(field.ID)
                };

                gvData.Columns.Add(column);
            }

            base.SetModel(model);
        }

        protected Control CreateHeaderControl(FieldEntity field)
        {
            var label = new CITI.EVO.Tools.Web.UI.Controls.Label
            {
                Text = field.Name.TrimLen(25)
            };

            return label;
        }

        protected Control CreateItemControl(FieldEntity field)
        {
            var label = new Label();
            label.DataBinding += label_DataBinding;

            return label;
        }

        protected void label_DataBinding(object sender, EventArgs e)
        {
            var label = sender as Label;
            if (label == null)
                return;

            var container = label.NamingContainer as GridViewRow;
            if (container == null)
                return;

            var descriptor = container.DataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return;

            var ownerID = descriptor.GetValue("OwnerID");
            var recordID = descriptor.GetValue("ID");

            label.Text = String.Format("{0:n}/{1:n}", ownerID, recordID);
        }

        protected bool GetStatusVisible(object eval)
        {
            var statusID = DataConverter.ToNullableGuid(eval);
            if (statusID == null)
                return false;

            var dbStatus = DataStatusCache.GetStatus(statusID);
            if (dbStatus == null)
                return false;

            return UmUtil.Instance.HasAccess("Admin") && statusID != DataStatusCache.None.ID;
        }

        protected bool GetEditVisible(object eval)
        {
            if (UmUtil.Instance.CurrentUser.IsSuperAdmin)
                return true;

            var userId = DataConverter.ToNullableGuid(eval);
            return userId == UserUtil.GetCurrentUserID();
        }

        protected bool GetDeleteVisible(object eval)
        {
            if (UmUtil.Instance.CurrentUser.IsSuperAdmin)
                return true;

            var userId = DataConverter.ToNullableGuid(eval);
            return userId == UserUtil.GetCurrentUserID();
        }

        protected String GetStatusName(object eval)
        {
            var statusID = DataConverter.ToNullableGuid(eval);
            if (statusID == null)
                return null;

            var dbStatus = DataStatusCache.GetStatus(statusID);
            if (dbStatus == null)
                return null;

            return dbStatus.Name;
        }

        protected String GetUserName(object eval)
        {
            var userID = DataConverter.ToNullableGuid(eval);

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            var name = String.Format("{0} - {1} {2}", user.LoginName, user.FirstName, user.LastName);
            return name;
        }
    }
}