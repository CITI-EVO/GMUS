using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Enums;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using Label = System.Web.UI.WebControls.Label;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormApproveDataGridControl : BaseUserControlExtend<FormApproveDataGridModel>
    {
        public event EventHandler<GenericEventArgs<Guid?>> Status;
        protected virtual void OnStatus(GenericEventArgs<Guid?> e)
        {
            if (Status != null)
                Status(this, e);
        }

        protected bool? AllowStatusChange
        {
            get { return DataConverter.ToNullableBool(ViewState["AllowStatusChange"]); }
            set { ViewState["AllowStatusChange"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnStatus_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnStatus(new GenericEventArgs<Guid?>(itemID));
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
                              let f = DataConverter.ToNullableGuid(n.Params.GetValueOrDefault(FormDataConstants.FieldIDField))
                              where f == fieldID && n.UserID == UserUtil.GetCurrentUserID()
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

            label.Text = $"{ownerID:n}/{recordID:n}";
        }

        public void InitStructure(GM_Form dbForm)
        {
            var converter = new FormEntityModelConverter(HbSession);
            var model = converter.Convert(dbForm);

            var formEntity = model.Entity;

            var controls = FormStructureUtil.InOrderFirstLevelTraversal(formEntity);

            var list = (from n in controls.OfType<FieldEntity>()
                        where n.Visible &&
                              n.DisplayOnGrid == "Always"
                        select n).ToList();

            var columns = gvData.Columns;

            var existFields = columns.OfType<BoundField>().Select(n => n.DataField).ToHashSet();

            foreach (var field in list)
            {
                var dataField = Convert.ToString(field.ID);
                if (existFields.Contains(dataField))
                    continue;

                var column = new GridViewMetaBoundField(null, dbForm.ID, field, formEntity);
                gvData.Columns.Add(column);
            }

            AllowStatusChange = true;

            var expGlobals = new ExpressionGlobalsUtil();
            if (!String.IsNullOrWhiteSpace(model.VisibleExpression))
            {
                var expNode = ExpressionParser.GetOrParse(model.VisibleExpression);

                var result = ExpressionEvaluator.TryEval(expNode, expGlobals.Eval);
                if (result.Error == null)
                    AllowStatusChange = DataConverter.ToNullableBoolean(result.Value);
            }

            var columnsLp = gvData.Columns.OfType<DataControlField>().ToLookup(n => n.HeaderText);

            if (!dbForm.RequiresApprove.GetValueOrDefault())
            {
                var statusNameColumn = columnsLp["Status Name"].First();
                statusNameColumn.Visible = false;

                var submitDateColumn = columnsLp["Submit Date"].First();
                submitDateColumn.Visible = false;

                var statusChangeDateColumn = columnsLp["Status Change Date"].First();
                statusChangeDateColumn.Visible = false;
            }

            if (dbForm.ApprovalDeadline.GetValueOrDefault() < 1)
            {
                var daysLeftColumn = columnsLp["Days Left"].First();
                daysLeftColumn.Visible = false;
            }
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

        protected bool GetStatusVisible(object dataItem)
        {
            if (!AllowStatusChange.GetValueOrDefault())
                return false;

            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            if (UserUtil.IsSuperAdmin() || UmUtil.Instance.HasAccess("Admin") || UmUtil.Instance.HasAccess("Org"))
                return true;

            return false;
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

        protected String GetUserStatus(object eval)
        {
            var userID = DataConverter.ToNullableGuid(eval);

            var userStatus = UserUtil.GetUserStatus(userID);
            return userStatus;
        }

        protected String GetSubmitDate(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var statusDate = DataConverter.ToNullableDateTime(descriptor.GetValue(FormDataConstants.DateOfSubmitField));
            if (statusDate == null)
                return null;

            return $"{statusDate:dd.MM.yyyy hh:mm}";
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

        protected Guid? GetCommandArg(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var recordID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.IDField));
            return recordID;
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