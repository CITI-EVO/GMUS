using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Monitoring;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Util;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class MonitoringBudgetDataControl : BaseUserControl
    {
        private IDictionary<Guid?, BudgetParagraphEntity> _paragraphs;
        private IDictionary<Guid?, MonitoringFlewEntity> _flews;

        private bool _controlsPrepared;

        public event EventHandler Save;
        protected virtual void OnSave(EventArgs e)
        {
            if (Save != null)
                Save(this, e);
        }

        public event EventHandler Cancel;
        protected virtual void OnCancel(EventArgs e)
        {
            if (Cancel != null)
                Cancel(this, e);
        }

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

        public event EventHandler<GenericEventArgs<Guid>> History;
        protected virtual void OnHistory(GenericEventArgs<Guid> e)
        {
            if (History != null)
                History(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Files;
        protected virtual void OnFiles(GenericEventArgs<Guid> e)
        {
            if (Files != null)
                Files(this, e);
        }

        public bool ShowFooter
        {
            get
            {
                return DataConverter.ToNullableBool(ViewState["ShowFooter"]).GetValueOrDefault();
            }
            set
            {
                ViewState["ShowFooter"] = value;
                ShowHideFooter();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowHideFooter();
            PrepareControls();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            OnSave(e);
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            OnCancel(e);
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

        protected void btnFiles_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnFiles(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnStatus_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnStatus(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnHistory_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnHistory(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void gvData_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var entity = e.Row.DataItem as MoneyTransferEntity;
                if (entity != null)
                {
                    if (entity.Status == MonitoringItemStatuses.Accepted)
                        e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);

                    if (entity.Status == MonitoringItemStatuses.Rejected)
                        e.Row.BackColor = Color.FromArgb(0, 255, 217, 191);
                }
            }
        }

        protected void gvData_HideEmptyRows(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Visible = false;
        }

        public void BindData(IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<MonitoringFlewEntity> flews, IEnumerable<MoneyTransferEntity> transfers)
        {
            _paragraphs = paragraphs.ToDictionary(n => n.ID);
            _flews = flews.ToDictionary(n => n.ID);

            gvData.DataSource = transfers;
            gvData.DataBind();

            ShowHideFooter();
        }

        public IEnumerable<MonitoringBudgetDataEntity> GetData(IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<MoneyTransferEntity> transfers)
        {
            var result = (from n in transfers
                          select new MonitoringBudgetDataEntity
                          {
                              DateOfTransfer = n.DateOfTransfer,
                              Incoming = n.Incoming,
                              Outgoing = n.Outgoing,
                              Remain = n.Remain,
                              Paragraph = GetParagraphName(n.ParagraphID),
                              Goal = n.Goal ?? String.Empty,
                              Comment = n.Comment ?? String.Empty,
                              Status = n.Status ?? String.Empty,
                              StatusUser = GetUserName(n.StatusUserID) ?? String.Empty,
                              StatusDate = n.StatusDate,
                              Flaws = GetFlewsNames(n.FlawsID),
                              FlawsScore = GetFlewsScores(n.FlawsID),
                              DateCreated = n.DateCreated.GetValueOrDefault()
                          });

            return result;
        }

        public void ShowCommands()
        {
            if (gvData.Columns.Count > 0)
                gvData.Columns[0].Visible = true;
        }
        public void HideCommands()
        {
            if (gvData.Columns.Count > 0)
                gvData.Columns[0].Visible = false;
        }

        public MonitoringBudgetItemModel GetFooter()
        {
            if (gvData.FooterRow == null)
                return null;

            PrepareControls();

            var model = new MonitoringBudgetItemModel();

            var controls = UserInterfaceUtil.TraverseControls(gvData.FooterRow);
            var controlsLp = controls.ToLookup(n => n.ID);

            var hdID = controlsLp["hdID"].SingleOrDefault() as HiddenField;
            if (hdID != null)
                model.ID = DataConverter.ToNullableGuid(hdID.Value);

            var tbxGoal = controlsLp["tbxGoal"].SingleOrDefault() as TextBox;
            if (hdID != null)
                model.Goal = DataConverter.ToString(GetControlValue(tbxGoal));

            var hdRecordID = controlsLp["hdRecordID"].SingleOrDefault() as HiddenField;
            if (hdRecordID != null)
                model.RecordID = DataConverter.ToNullableGuid(hdRecordID.Value);

            var cbxParagraphs = controlsLp["cbxParagraphs"].SingleOrDefault() as DropDownList;
            if (cbxParagraphs != null)
                model.ParagraphID = cbxParagraphs.TryGetGuidValue();

            var tbxDateOfTransfer = controlsLp["tbxDateOfTransfer"].SingleOrDefault() as TextBox;
            if (tbxDateOfTransfer != null)
                model.DateOfTransfer = DataConverter.ToNullableDateTime(tbxDateOfTransfer.Text);

            var tbxIncoming = controlsLp["tbxIncoming"].SingleOrDefault() as TextBox;
            var tbxOutgoing = controlsLp["tbxOutgoing"].SingleOrDefault() as TextBox;

            if (tbxIncoming != null && tbxOutgoing != null)
            {
                var incoming = DataConverter.ToNullableDouble(tbxIncoming.Text);
                var outgoing = DataConverter.ToNullableDouble(tbxOutgoing.Text);

                if (Math.Abs(incoming.GetValueOrDefault()) > double.Epsilon)
                {
                    model.Type = "Incoming";
                    model.Amount = incoming;
                }
                else if (Math.Abs(outgoing.GetValueOrDefault()) > double.Epsilon)
                {
                    model.Type = "Outgoing";
                    model.Amount = outgoing;
                }
            }

            return model;
        }

        public void FillFooter(MonitoringBudgetItemModel model)
        {
            PrepareFooter();

            if (gvData.FooterRow == null)
                return;

            var controls = UserInterfaceUtil.TraverseControls(gvData.FooterRow);
            var controlsLp = controls.ToLookup(n => n.ID);

            var hdID = controlsLp["hdID"].SingleOrDefault() as HiddenField;
            if (hdID != null)
                hdID.Value = Convert.ToString(model.ID);

            var hdRecordID = controlsLp["hdRecordID"].SingleOrDefault() as HiddenField;
            if (hdRecordID != null)
                hdRecordID.Value = Convert.ToString(model.RecordID);

            var tbxDateOfTransfer = controlsLp["tbxDateOfTransfer"].SingleOrDefault() as TextBox;
            if (tbxDateOfTransfer != null)
                tbxDateOfTransfer.Text = $"{model.DateOfTransfer:dd.MM.yyyy}";

            var tbxIncoming = controlsLp["tbxIncoming"].SingleOrDefault() as TextBox;
            var tbxOutgoing = controlsLp["tbxOutgoing"].SingleOrDefault() as TextBox;

            if (tbxIncoming != null && tbxOutgoing != null)
            {
                if (model.Type == "Incoming")
                    tbxIncoming.Text = $"{model.Amount}";
                else if (model.Type == "Outgoing")
                    tbxOutgoing.Text = $"{model.Amount}";
                else if (model.Type == "Clear")
                {
                    tbxIncoming.Text = String.Empty;
                    tbxOutgoing.Text = String.Empty;
                }
            }

            var tbxGoal = controlsLp["tbxGoal"].SingleOrDefault() as TextBox;
            if (tbxGoal != null)
                tbxGoal.Text = model.Goal;

            var cbxParagraphs = controlsLp["cbxParagraphs"].SingleOrDefault() as DropDownList;
            if (cbxParagraphs != null)
                cbxParagraphs.TrySetSelectedValue(model.ParagraphID);
        }

        protected void ShowHideFooter()
        {
            PrepareFooter();
            BindFooter();

            gvData.ShowFooter = ShowFooter;

            if (gvData.FooterRow != null)
                gvData.FooterRow.Visible = ShowFooter;
        }

        protected void PrepareFooter()
        {
            var collection = gvData.DataSource as IEnumerable;
            if (gvData.FooterRow == null || collection == null || !collection.Cast<Object>().Any())
            {
                var fields = new HashSet<String>
                {
                    "ID",
                    "Goal",
                    "FlawsID",
                    "Remain",
                    "Status",
                    "Comment",
                    "Incoming",
                    "Outgoing",
                    "StatusDate",
                    "ExpireDate",
                    "ParagraphID",
                    "DateCreated",
                    "StatusUserID",
                    "DateOfTransfer",
                };

                gvData.RowDataBound += gvData_HideEmptyRows;

                gvData.DataSource = GenEmptyData(fields);
                gvData.DataBind();

                gvData.RowDataBound -= gvData_HideEmptyRows;
            }
        }

        protected void PrepareControls()
        {
            if (_controlsPrepared)
                return;

            if (gvData.FooterRow == null)
                return;

            var controls = UserInterfaceUtil.TraverseControls(gvData.FooterRow);
            foreach (var control in controls)
            {
                var hidden = control as HiddenField;
                if (hidden != null)
                    hidden.Value = Convert.ToString(GetControlValue(hidden));

                var textBox = control as TextBox;
                if (textBox != null)
                    textBox.Text = Convert.ToString(GetControlValue(textBox));

                var dropDown = control as DropDownList;
                if (dropDown != null)
                    dropDown.TrySetSelectedValue(GetControlValue(dropDown));
            }

            _controlsPrepared = true;
        }

        protected void BindFooter()
        {
            if (gvData.FooterRow != null && _paragraphs != null)
            {
                var control = UserInterfaceUtil.TraverseControls(gvData.FooterRow).FirstOrDefault(n => n.ID == "cbxParagraphs");

                var cbxParagraphs = control as DropDownList;
                if (cbxParagraphs != null)
                {
                    var leafParagraphs = (from n in _paragraphs.Values
                                          where n.Container
                                          select n);

                    cbxParagraphs.BindData(leafParagraphs);
                }
            }
        }

        protected String GetFlewsNames(object eval)
        {
            var flewsID = eval as IEnumerable<Guid?>;
            if (flewsID == null)
                return null;

            var query = (from n in flewsID
                         where n != null
                         let f = _flews.GetValueOrDefault(n)
                         where f != null
                         select f.Name);

            var flews = String.Join("; ", query);
            return flews;
        }

        protected double? GetFlewsScores(object eval)
        {
            var flewsID = eval as IEnumerable<Guid?>;
            if (flewsID == null)
                return null;

            var query = (from n in flewsID
                         where n != null
                         let f = _flews.GetValueOrDefault(n)
                         where f != null
                         select f.Score);

            var sum = query.Sum();
            return sum;
        }

        protected String GetDatePart(object eval)
        {
            return $"{eval:dd.MM.yyyy}";
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

        protected String GetParagraphName(object eval)
        {
            var itemID = DataConverter.ToNullableGuid(eval);
            if (itemID == null)
                return null;

            if (_paragraphs == null)
                return null;

            var entity = _paragraphs.GetValueOrDefault(itemID.Value);
            if (entity == null)
                return null;

            return entity.Name;
        }

        protected bool GetViewVisible(object dataItem)
        {
            return false;
        }

        protected bool GetEditVisible(object dataItem)
        {
            if (UmUtil.Instance.HasAccess("Admin"))
                return true;

            var entity = dataItem as MoneyTransferEntity;
            if (entity == null)
                return false;

            if (entity.Status == MonitoringItemStatuses.Accepted)
                return false;

            if (entity.Status == MonitoringItemStatuses.Rejected)
            {
                if (entity.ExpireDate != null && entity.ExpireDate < DateTime.Now)
                    return false;
            }
            else if (entity.StatusDate != null)
            {
                return false;
            }

            return true;
        }

        protected bool GetDeleteVisible(object dataItem)
        {
            if (UmUtil.Instance.HasAccess("Admin"))
                return true;

            var entity = dataItem as MoneyTransferEntity;
            if (entity == null)
                return false;

            if (entity.Status == MonitoringItemStatuses.Accepted)
                return false;

            if (entity.Status == MonitoringItemStatuses.Rejected)
            {
                if (entity.ExpireDate != null && entity.ExpireDate < DateTime.Now)
                    return false;
            }
            else if (entity.StatusDate != null)
            {
                return false;
            }

            return true;
        }

        protected bool GetStatusVisible(object dataItem)
        {
            return UserUtil.IsSuperAdmin() || UmUtil.Instance.HasAccess("Admin") || UmUtil.Instance.HasAccess("MonitoringStatus");
        }

        protected bool GetHistoryVisible(object dataItem)
        {
            return UserUtil.IsSuperAdmin() || UmUtil.Instance.HasAccess("Admin") || UmUtil.Instance.HasAccess("MonitoringStatus");
        }

        protected bool GetFilesVisible(object dataItem)
        {
            return true;
        }

        protected String GetCorrectName(Object val)
        {
            var name = Convert.ToString(val);

            if (String.IsNullOrWhiteSpace(name))
                return "[Name Is Empty]";

            return name;
        }

        protected Object GetControlValue(Control control)
        {
            if (control == null)
                return null;

            return Request.Form[control.UniqueID];
        }

        protected DictionaryDataView GenEmptyData(ISet<String> fields)
        {
            var dict = new Dictionary<String, Object>();
            foreach (var field in fields)
                dict[field] = String.Empty;

            var dataView = new DictionaryDataView(new[] { dict }, fields);
            return dataView;
        }
    }
}