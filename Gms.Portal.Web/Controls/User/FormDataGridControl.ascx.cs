using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Controls;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using Label = System.Web.UI.WebControls.Label;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormDataGridControl : BaseUserControl
    {
        public event EventHandler<GenericEventArgs<Guid>> View;
        protected virtual void OnView(GenericEventArgs<Guid> e)
        {
            if (View != null)
                View(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Edit;
        protected virtual void OnEdit(GenericEventArgs<Guid> e)
        {
            if (Edit != null)
                Edit(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Delete;
        protected virtual void OnDelete(GenericEventArgs<Guid> e)
        {
            if (Delete != null)
                Delete(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Status;
        protected virtual void OnStatus(GenericEventArgs<Guid> e)
        {
            if (Status != null)
                Status(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Review;
        protected virtual void OnReview(GenericEventArgs<Guid> e)
        {
            if (Review != null)
                Review(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Inspect;
        protected virtual void OnInspect(GenericEventArgs<Guid> e)
        {
            if (Inspect != null)
                Inspect(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Assigne;
        protected virtual void OnAssigne(GenericEventArgs<Guid> e)
        {
            if (Assigne != null)
                Assigne(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Print;
        protected virtual void OnPrint(GenericEventArgs<Guid> e)
        {
            if (Print != null)
                Print(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnView_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnView(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnEdit_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnEdit(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnDelete_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnDelete(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnStatus_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnStatus(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnReview_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnReview(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnInspect_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnInspect(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnAssigne_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnAssigne(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnPrint_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnPrint(new GenericEventArgs<Guid>(itemID.Value));

        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var descriptor = e.Row.DataItem as DictionaryItemDescriptor;

                if (descriptor != null)
                {
                    var statusID = DataConverter.ToNullableGuid(descriptor.GetValue("StatusID"));

                    if (statusID == DataStatusCache.Submit.ID)
                        e.Row.BackColor = Color.FromArgb(0, 237, 234, 230);

                    if (statusID == DataStatusCache.Rejected.ID)
                        e.Row.BackColor = Color.FromArgb(0, 255, 217, 191);

                    if (statusID == DataStatusCache.Accepted.ID)
                        e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);
                }
            }
        }

        public void InitStructure(GM_Form dbForm)
        {
            var converter = new FormEntityModelConverter(HbSession);
            var model = converter.Convert(dbForm);

            var formEntity = model.Entity;

            var controls = FormStructureUtil.InOrderFirstLevelTraversal(formEntity);

            var list = (from n in controls.OfType<FieldEntity>()
                        where n.Visible && n.DisplayOnGrid == "Always"
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

            if (!dbForm.RequiresApprove.GetValueOrDefault() || dbForm.ApprovalDeadline.GetValueOrDefault() < 1)
            {
                gvData.Columns[1].Visible = false;
                gvData.Columns[2].Visible = false;
                gvData.Columns[3].Visible = false;
            }
        }

        public void BindData(DictionaryDataView formDataView)
        {
            gvData.DataSource = formDataView;
            gvData.DataBind();
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

            label.Text = $"{ownerID:n}/{recordID:n}";
        }

        protected bool GetStatusVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            if (UserUtil.IsSuperAdmin() || UmUtil.Instance.HasAccess("Admin"))
                return true;

            if (UmUtil.Instance.HasAccess("Org"))
            {
                var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
                if (userStatuses == null)
                    return false;

                var statusLp = userStatuses.ToLookup(n => n.Step);

                var steps = (from n in statusLp
                             let e = (from m in n
                                      where m.UserID == UmUtil.Instance.CurrentUser.ID
                                      select m).Any()
                             where e
                             select n.Key).ToSortedSet();

                var min = steps.Min;

                var query = (from n in statusLp
                             where n.Key < min
                             let e = n.Any(m => m.StatusID == null || m.StatusID == DataStatusCache.None.ID)
                             where e
                             select n.Key);

                if (query.Any())
                    return false;

                return true;
            }

            return false;
        }

        protected bool GetEditVisible(object dataItem)
        {
            if (UmUtil.Instance.CurrentUser.IsSuperAdmin)
                return true;

            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.UserIDField));
            if (userID != UserUtil.GetCurrentUserID())
                return false;

            var statusID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.StatusIDField));
            if (statusID == null)
                return false;

            return (statusID == DataStatusCache.None.ID);
        }

        protected bool GetPrintVisible(object dataItem)
        {
            return true;
        }

        protected bool GetDeleteVisible(object dataItem)
        {
            if (UmUtil.Instance.CurrentUser.IsSuperAdmin)
                return true;

            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.UserIDField));
            if (userID != UserUtil.GetCurrentUserID())
                return false;

            var statusID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.StatusIDField));
            if (statusID == null)
                return false;

            return (statusID == DataStatusCache.None.ID);
        }

        protected bool GetReviewVisible(object dataItem)
        {
            if (UmUtil.Instance.CurrentUser.IsSuperAdmin)
                return false;

            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.UserIDField));
            if (userID != UserUtil.GetCurrentUserID())
                return false;

            var statusID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.StatusIDField));
            if (statusID == null)
                return false;

            return (statusID == DataStatusCache.Rejected.ID);
        }

        protected bool GetInspectVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var statusID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.StatusIDField));
            if (statusID == null)
                return false;

            if (statusID == DataStatusCache.Submit.ID)
            {
                if (UmUtil.Instance.HasAccess("Admin"))
                    return true;
            }

            return false;
        }

        protected bool GetAssigneVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var statusID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.StatusIDField));
            if (statusID == null)
                return false;

            if (statusID == DataStatusCache.None.ID)
                return false;

            return UserUtil.IsSuperAdmin();
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

            var name = $"{user.LoginName} - {user.FirstName} {user.LastName}";
            return name;
        }

        protected String GetStatusDate(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var statusDate = DataConverter.ToNullableDateTime(descriptor.GetValue(FormDataConstants.StatusChangeDateField));
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
                statusDate = DataConverter.ToNullableDateTime(descriptor.GetValue(FormDataConstants.StatusChangeDateField));
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



    }
}