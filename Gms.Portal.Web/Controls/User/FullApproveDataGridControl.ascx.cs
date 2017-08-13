using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Enums;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FullApproveDataGridControl : BaseUserControlExtend<FullApproveDataGridModel>
    {
        public event EventHandler<GenericEventArgs<String>> Accept;
        protected virtual void OnAccept(GenericEventArgs<String> e)
        {
            if (Accept != null)
                Accept(this, e);
        }

        public event EventHandler<GenericEventArgs<String>> Reject;
        protected virtual void OnReject(GenericEventArgs<String> e)
        {
            if (Reject != null)
                Reject(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnAccept_OnCommand(object sender, CommandEventArgs e)
        {
            var commandArg = Convert.ToString(e.CommandArgument);
            OnAccept(new GenericEventArgs<String>(commandArg));
        }

        protected void btnReject_OnCommand(object sender, CommandEventArgs e)
        {
            var commandArg = Convert.ToString(e.CommandArgument);
            OnReject(new GenericEventArgs<String>(commandArg));
        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            var descriptor = e.Row.DataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return;

            var fieldID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FieldIDField));
            if (fieldID == null)
                return;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return;

            var myStatuses = (from n in userStatuses
                              where n.Params != null
                              let p = n.Params.GetValueOrDefault(FormDataConstants.FieldIDField)
                              let f = DataConverter.ToNullableGuid(p)
                              where f == fieldID &&
                                    n.UserID == UserUtil.GetCurrentUserID()
                              select n);

            var myStatus = myStatuses.FirstOrDefault();
            if (myStatus == null)
                return;

            if (myStatus.StatusID == DataStatusCache.Submit.ID)
                e.Row.BackColor = Color.FromArgb(0, 237, 234, 230);

            if (myStatus.StatusID == DataStatusCache.Rejected.ID)
                e.Row.BackColor = Color.FromArgb(0, 255, 217, 191);

            if (myStatus.StatusID == DataStatusCache.Accepted.ID)
                e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);
        }

        protected bool GetAcceptVisible(object dataItem)
        {
            if (!UmUtil.Instance.HasAccess("Admin") && !UmUtil.Instance.HasAccess("Org"))
                return false;

            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var fieldID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FieldIDField));
            if (fieldID == null)
                return false;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return true;

            var myStatuses = (from n in userStatuses
                              where n.Params != null
                              let p = n.Params.GetValueOrDefault(FormDataConstants.FieldIDField)
                              let f = DataConverter.ToNullableGuid(p)
                              where f == fieldID &&
                                    n.UserID == UserUtil.GetCurrentUserID()
                              select n);

            var myStatus = myStatuses.FirstOrDefault();
            if (myStatus == null)
                return true;

            if (myStatus.StatusID == DataStatusCache.Accepted.ID)
                return false;

            return true;
        }

        protected bool GetRejectVisible(object dataItem)
        {
            if (!UmUtil.Instance.HasAccess("Admin") && !UmUtil.Instance.HasAccess("Org"))
                return false;

            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var fieldID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FieldIDField));
            if (fieldID == null)
                return false;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return true;

            var myStatuses = (from n in userStatuses
                              where n.Params != null
                              let p = n.Params.GetValueOrDefault(FormDataConstants.FieldIDField)
                              let f = DataConverter.ToNullableGuid(p)
                              where f == fieldID &&
                                    n.UserID == UserUtil.GetCurrentUserID()
                              select n);

            var myStatus = myStatuses.FirstOrDefault();
            if (myStatus == null)
                return true;

            if (myStatus.StatusID == DataStatusCache.Rejected.ID)
                return false;

            return true;
        }

        protected bool GetPrintVisible(object dataItem)
        {
            return true;
        }

        protected String GetStatusName(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var fieldID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FieldIDField));
            if (fieldID == null)
                return null;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return null;

            var myStatuses = (from n in userStatuses
                              where n.Params != null
                              let f = DataConverter.ToNullableGuid(n.Params.GetValueOrDefault(FormDataConstants.FieldIDField))
                              where f == fieldID && n.UserID == UserUtil.GetCurrentUserID()
                              select n);

            var myStatus = myStatuses.FirstOrDefault();
            if (myStatus == null)
                return null;

            var dbStatus = DataStatusCache.GetStatus(myStatus.StatusID);
            if (dbStatus == null)
                return null;

            return dbStatus.Name;
        }

        protected String GetUserName(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.UserIDField));
            if (userID == null)
                return null;

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            var name = $"{user.LoginName} - {user.FirstName} {user.LastName}";
            return name;
        }

        protected String GetStatusDate(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var fieldID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FieldIDField));
            if (fieldID == null)
                return null;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return null;

            var myStatuses = (from n in userStatuses
                              where n.Params != null
                              let f = DataConverter.ToNullableGuid(n.Params.GetValueOrDefault(FormDataConstants.FieldIDField))
                              where f == fieldID && n.UserID == UserUtil.GetCurrentUserID()
                              select n);

            var myStatus = myStatuses.FirstOrDefault();
            if (myStatus == null)
                return null;

            var dbStatus = DataStatusCache.GetStatus(myStatus.StatusID);
            if (dbStatus == null)
                return null;

            return $"{myStatus.DateOfStatus:dd.MM.yyyy hh:mm}";
        }

        protected int? GetDaysLeft(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var formID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FormIDField));
            if (formID == null)
                return null;

            var statusDate = DataConverter.ToNullableDateTime(descriptor.GetValue(FormDataConstants.DateOfSubmitField));
            if (statusDate == null)
            {
                statusDate = DataConverter.ToNullableDateTime(descriptor.GetValue(FormDataConstants.DateOfStatusField));
                if (statusDate == null)
                    return null;
            }

            var currentStatusID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.StatusIDField));
            if (currentStatusID != DataStatusCache.Submit.ID)
                return null;

            var formDeadlines = GmsCommonUtil.GetFormDeadlines();
            if (formDeadlines == null)
                return null;

            var deadline = formDeadlines.GetValueOrDefault(formID.Value);
            if (deadline == null)
                return null;

            var diff = DateTime.Now - statusDate.Value;
            return (int)(deadline - diff.TotalDays);
        }

        protected String GetCommandArg(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var recordID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.IDField));
            if (recordID == null)
                return null;

            var formID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FormIDField));
            if (formID == null)
                return null;

            var fieldID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FieldIDField));
            if (fieldID == null)
                return null;

            return $"{recordID}/{formID}/{fieldID}";
        }

        protected String GetViewUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return "#";

            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = Convert.ToString(FormMode.View),
                ["FormID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["OwnerID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["RecordID"] = descriptor.GetValue(FormDataConstants.IDField),
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            return urlHelper.ToEncodedUrl();
        }

        protected String GetPrintUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return "#";

            var urlHelper = new UrlHelper("~/Handlers/PrintFormData.ashx")
            {
                ["FormID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["RecordID"] = descriptor.GetValue(FormDataConstants.IDField),
                ["LoginToken"] = UmUtil.Instance.CurrentToken,
            };

            return urlHelper.ToEncodedUrl();
        }
    }
}