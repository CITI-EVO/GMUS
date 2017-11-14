using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Collections;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Controls.User.Monitoring;
using Gms.Portal.Web.Entities.Monitoring;
using Gms.Portal.Web.Entities.Others;
using MongoDB.Bson;
using MongoDB.Driver;
using NVelocityTemplateEngine;

namespace Gms.Portal.Web.Pages.User
{
    public partial class RecordMonitoring : BasePage
    {
        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
        }

        public Guid? RecordID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["RecordID"]); }
        }

        private GM_Form _dbForm;
        protected GM_Form DbForm
        {
            get
            {
                if (_dbForm == null)
                    _dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == FormID);

                return _dbForm;
            }
        }

        private FormModel _formModel;
        protected FormModel FormModel
        {
            get
            {
                if (_formModel == null)
                    _formModel = ModelConverter.Convert(DbForm);

                return _formModel;
            }
        }

        protected FormEntity FormEntity
        {
            get { return FormModel.Entity; }
        }

        private FormDataUnit _formData;
        protected FormDataUnit FormData
        {
            get
            {
                if (RecordID == null)
                    return null;

                if (_formData == null)
                    _formData = LoadFormData(FormID, RecordID);

                return _formData;
            }
        }

        private ISet<MonitoringFlewEntity> _flews;
        public ISet<MonitoringFlewEntity> Flews
        {
            get
            {
                _flews = (_flews ?? GetFlews().ToHashSet());
                return _flews;
            }
        }

        private ISet<BudgetParagraphEntity> _paragraphs;
        public ISet<BudgetParagraphEntity> Paragraphs
        {
            get
            {
                _paragraphs = (_paragraphs ?? GenParagraphsTree().ToHashSet());
                return _paragraphs;
            }
        }

        private FormEntityModelConverter _modelConverter;
        protected FormEntityModelConverter ModelConverter
        {
            get
            {
                _modelConverter = (_modelConverter ?? new FormEntityModelConverter(HbSession));
                return _modelConverter;
            }
        }

        private ILookup<String, ControlEntity> _controlsLp;
        protected ILookup<String, ControlEntity> ControlsLp
        {
            get
            {
                _controlsLp = (_controlsLp ?? FormStructureUtil.PreOrderTraversal(FormEntity).ToLookup(n => n.Alias, StringComparer.OrdinalIgnoreCase));
                return _controlsLp;
            }
        }

        private IEnumerable<MoneyTransferEntity> _moneyTransfers;
        protected IEnumerable<MoneyTransferEntity> MoneyTransfers
        {
            get
            {
                _moneyTransfers = (_moneyTransfers ?? LoadMoneyTransfers());
                return _moneyTransfers;
            }
        }

        private ILookup<String, FormStatusUnit> _formStatusUnits;
        protected ILookup<String, FormStatusUnit> FormStatusUnits
        {
            get
            {
                if (_formStatusUnits == null && FormData.UserStatuses != null)
                {
                    var statusUnits = FormData.UserStatuses;
                    _formStatusUnits = statusUnits.ToLookup(n => $"{n.UserID}_{n.StatusID}");
                }

                return _formStatusUnits;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //monitoringDataGridsControl.InitStructure(FormEntity);
            monitoringProjectsGridsControl.InitStructure(FormEntity);

            FillMonitoringData();
            FillProjectsData();

            ApplyViewMode();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            var monitoring = FormEntity.Monitoring;
            if (monitoring == null)
                return;

            cbxPrintTemplates.DataSource = monitoring.PrintFields;
            cbxPrintTemplates.DataBind();

            mpePrint.Show();
        }
        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
        }

        protected void btnBudgetSubmit_OnClick(object sender, EventArgs e)
        {
            var paymentType = cbxFinancingType.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(paymentType))
            {
                lblGlobalError.Text = "Please select Financing Type";
                return;
            }

            var organizationType = cbxOrganizationType.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(organizationType))
            {
                lblGlobalError.Text = "Please select Organization Type";
                return;
            }

            var organizationID = cbxOrganizations.TryGetGuidValue();
            if (organizationID == null)
            {
                lblGlobalError.Text = "Please select Organization";
                return;
            }

            var period = cbxPeriods.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(period))
            {
                lblGlobalError.Text = "Please select Period";
                return;
            }

            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            if (startDate == null)
            {
                lblGlobalError.Text = "Please select Start Date";
                return;
            }

            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);
            if (endDate == null)
            {
                lblGlobalError.Text = "Please select End Date";
                return;
            }

            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null,
                ["PaymentType"] = paymentType,
                ["OrganizationType"] = organizationType,
                ["OrganizationID"] = organizationID
            };


            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter);

            var docsToSubmit = (from n in documents
                                let val = BsonTypeMapper.MapToDotNetValue(n["DateOfTransfer"])
                                let dt = DataConverter.ToNullableDateTime(val)
                                where dt >= startDate && dt <= endDate
                                select n);

            var submitDate = DateTime.Now;

            foreach (var document in docsToSubmit)
            {
                var oldValue = BsonTypeMapper.MapToDotNetValue(document["SubmitDate"]);

                var oldDate = DataConverter.ToNullableDateTime(oldValue);
                if (oldDate != null)
                    continue;

                document["SubmitDate"] = submitDate;
                document["SubmitUserID"] = UserUtil.GetCurrentUserID();

                MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringBudgetCollectionName, document);
            }

            Response.Redirect(RequestUrl.ToEncodedUrl());
        }
        protected void btnBudgetApprove_OnClick(object sender, EventArgs e)
        {
            var period = cbxPeriods.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(period))
            {
                lblGlobalError.Text = "Please select Period";
                return;
            }

            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            if (startDate == null)
            {
                lblGlobalError.Text = "Please select Start Date";
                return;
            }

            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);
            if (endDate == null)
            {
                lblGlobalError.Text = "Please select End Date";
                return;
            }

            var paymentType = cbxFinancingType.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(paymentType))
            {
                lblGlobalError.Text = "Please select Financing Type";
                return;
            }

            var organizationType = cbxOrganizationType.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(organizationType))
            {
                lblGlobalError.Text = "Please select Organization Type";
                return;
            }

            var organizationID = cbxOrganizations.TryGetGuidValue();
            if (organizationID == null)
            {
                lblGlobalError.Text = "Please select Organization";
                return;
            }

            mpeBudgetApprove.Show();
        }

        protected void btnProjectsSubmit_OnClick(object sender, EventArgs e)
        {
            var period = cbxPeriods.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(period))
            {
                lblGlobalError.Text = "Please select Period";
                return;
            }

            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            if (startDate == null)
            {
                lblGlobalError.Text = "Please select Start Date";
                return;
            }

            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);
            if (endDate == null)
            {
                lblGlobalError.Text = "Please select End Date";
                return;
            }

            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringProjectCollectionName, filter);

            var docsToSubmit = (from n in documents
                                let sdVal = BsonTypeMapper.MapToDotNetValue(n["StartDate"])
                                let edVal = BsonTypeMapper.MapToDotNetValue(n["EndDate"])
                                let startDt = DataConverter.ToNullableDateTime(sdVal)
                                let endDt = DataConverter.ToNullableDateTime(edVal)
                                where startDt >= startDate && startDt <= endDate &&
                                      endDt >= startDate && endDt <= endDate
                                select n);

            var submitDate = DateTime.Now;

            foreach (var document in docsToSubmit)
            {
                var oldValue = BsonTypeMapper.MapToDotNetValue(document["SubmitDate"]);

                var oldDate = DataConverter.ToNullableDateTime(oldValue);
                if (oldDate != null)
                    continue;

                document["SubmitDate"] = submitDate;
                document["SubmitUserID"] = UserUtil.GetCurrentUserID();

                MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringBudgetCollectionName, document);
            }

            Response.Redirect(RequestUrl.ToEncodedUrl());
        }
        protected void btnProjectsApprove_OnClick(object sender, EventArgs e)
        {
            var period = cbxPeriods.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(period))
            {
                lblGlobalError.Text = "Please select Period";
                return;
            }

            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            if (startDate == null)
            {
                lblGlobalError.Text = "Please select Start Date";
                return;
            }

            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);
            if (endDate == null)
            {
                lblGlobalError.Text = "Please select End Date";
                return;
            }

            mpeProjectApprove.Show();
        }

        protected void btnSubmitPrint_OnClick(object sender, EventArgs e)
        {
            var id = DataConverter.ToNullableGuid(cbxPrintTemplates.SelectedValue);
            if (id == null)
                return;

            var monitoring = FormEntity.Monitoring;
            if (monitoring == null || monitoring.Flews == null)
                return;

            var template = monitoring.PrintFields.FirstOrDefault(n => n.ID == id);
            if (template == null)
                return;

            if (template.PrintType == "Excel")
            {
                var dataSet = new DataSet(template.Name);

                if (template.BudgetForm.GetValueOrDefault())
                {
                    var dataTable = GetBudgetData().ToDataTable();
                    dataTable.TableName = "Budget";
                    dataSet.Tables.Add(dataTable);
                }

                if (template.SummaryBudgetForm.GetValueOrDefault())
                {
                    var dataTable = GetSummaryBudgetData().ToDataTable();
                    dataTable.TableName = "SummaryBudget";
                    dataSet.Tables.Add(dataTable);
                }

                if (template.ProjectsForm.GetValueOrDefault())
                {
                    var dataTable = GetMonitoringProjectsData().ToDataTable();
                    dataTable.TableName = "ProjectsForm";
                    dataSet.Tables.Add(dataTable);
                }

                ReportUnitHelper.Export(Response, template.PrintType, dataSet);
            }
            else
            {
                ExportToPdf(template);
            }

            mpePrint.Hide();
        }
        protected void btnPrintCancel_OnClick(object sender, EventArgs e)
        {
            mpePrint.Hide();
        }

        protected void monitoringFilesControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {

        }

        protected void btnMonitoringBudgetDataExport_OnClick(object sender, EventArgs e)
        {
            var dataTable = GetBudgetData().ToDataTable();
            var target = sender == btnMonitoringBudgetDataExport ? "Excel" : "PDF";
            ReportUnitHelper.Export(Response, target, dataTable);
        }

        protected void monitoringBudgetDataControl_OnSave(object sender, EventArgs e)
        {
            lblMonitoringBudgetDataError.Text = String.Empty;

            var model = monitoringBudgetDataControl.GetFooter();

            if (model.Amount == null)
            {
                lblMonitoringBudgetDataError.Text = "Please enter amount (Incoming or Outgoing)";
                return;
            }

            if (model.DateOfTransfer == null)
            {
                lblMonitoringBudgetDataError.Text = "Please enter Date Of Transfer";
                return;
            }

            if (model.ParagraphID == null)
            {
                lblMonitoringBudgetDataError.Text = "Please select Paragraph";
                return;
            }

            if (model.Type == null)
            {
                lblMonitoringBudgetDataError.Text = "Please select Type";
                return;
            }

            var paymentType = cbxFinancingType.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(paymentType))
            {
                lblMonitoringBudgetDataError.Text = "Please select Financing Type";
                return;
            }

            var organizationType = cbxOrganizationType.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(organizationType))
            {
                lblMonitoringBudgetDataError.Text = "Please select Organization Type";
                return;
            }

            var organizationID = cbxOrganizations.TryGetGuidValue();
            if (organizationID == null)
            {
                lblMonitoringBudgetDataError.Text = "Please select Organization";
                return;
            }

            var oldDict = (IDictionary<String, Object>)null;
            if (model.ID != null)
            {
                var oldDocument = MongoDbUtil.GetDocument(MongoDbUtil.MonitoringBudgetCollectionName, model.ID);
                if (oldDocument != null)
                {
                    oldDocument["DateDeleted"] = DateTime.Now;
                    MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringBudgetCollectionName, oldDocument);

                    oldDict = BsonDocumentConverter.ConvertToDictionary(oldDocument);
                }
            }

            var incoming = (double?)null;
            var outgoing = (double?)null;

            if (model.Type == "Incoming")
                incoming = model.Amount;
            else if (model.Type == "Outgoing")
                outgoing = model.Amount;

            var newDict = new Dictionary<String, Object>
            {
                ["ID"] = Guid.NewGuid(),
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["PreviousID"] = model.ID,

                ["Goal"] = model.Goal,
                ["ParagraphID"] = model.ParagraphID,
                ["DateOfTransfer"] = model.DateOfTransfer,

                ["Remain"] = null,
                ["Incoming"] = incoming,
                ["Outgoing"] = outgoing,

                ["CreateUserID"] = UserUtil.GetCurrentUserID(),

                ["StatusID"] = null,
                ["StatusDate"] = null,
                ["StatusUserID"] = null,

                ["SubmitDate"] = null,
                ["SubmitUserID"] = null,

                ["Comment"] = null,
                ["PaymentType"] = paymentType,
                ["OrganizationID"] = organizationID,
                ["OrganizationType"] = organizationType,

                ["DateCreated"] = DateTime.Now,
                ["DateChanged"] = null,
                ["DateDeleted"] = null,
            };

            if (oldDict != null)
            {
                newDict["StatusID"] = oldDict.GetValueOrDefault("StatusID");

                newDict["SubmitDate"] = oldDict.GetValueOrDefault("SubmitDate");
                newDict["SubmitUserID"] = oldDict.GetValueOrDefault("SubmitUserID");

                newDict["PreviousID"] = oldDict.GetValueOrDefault("ID");
                newDict["StatusDate"] = oldDict.GetValueOrDefault("StatusDate");
                newDict["StatusUserID"] = oldDict.GetValueOrDefault("StatusUserID");
            }

            var newDocument = BsonDocumentConverter.ConvertToBsonDocument(newDict);
            MongoDbUtil.InsertDocument(MongoDbUtil.MonitoringBudgetCollectionName, newDocument);

            NormilizeRemains();

            var clearModel = new MonitoringBudgetItemModel
            {
                DateOfTransfer = model.DateOfTransfer
            };

            monitoringBudgetDataControl.FillFooter(clearModel);

            FillMonitoringData();
        }
        protected void monitoringBudgetDataControl_OnCancel(object sender, EventArgs e)
        {
            var model = new MonitoringBudgetItemModel { Type = "Clear" };
            monitoringBudgetDataControl.FillFooter(model);

            ApplyViewMode();
        }
        protected void monitoringBudgetDataControl_OnStatus(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter);

            var document = documents.SingleOrDefault();
            if (document == null)
                return;

            var dictionary = BsonDocumentConverter.ConvertToDictionary(document);

            var flawsID = dictionary.GetValueOrDefault("FlawsID") as IEnumerable<Guid?>;
            flawsID = (flawsID ?? Enumerable.Empty<Guid?>());

            var model = new MonitoringStatusModel
            {
                RecordID = e.Value,

                FlawsID = flawsID.ToHashSet(),
                Status = Convert.ToString(dictionary.GetValueOrDefault("Status")),
                Comment = Convert.ToString(dictionary.GetValueOrDefault("Comment")),
                ExpireDate = DataConverter.ToNullableDateTime(dictionary.GetValueOrDefault("ExpireDate")),
            };

            monitoringStatusControl.Model = model;

            mpeMonitoringStatus.Show();
        }
        protected void monitoringBudgetDataControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter);
            var document = documents.SingleOrDefault();

            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            var incoming = DataConverter.ToNullableDouble(dict.GetValueOrDefault("Incoming"));
            var outgoing = DataConverter.ToNullableDouble(dict.GetValueOrDefault("Outgoing"));

            var type = String.Empty;
            var amount = (double?)null;

            if (incoming == null && outgoing != null)
            {
                type = "Outgoing";
                amount = outgoing;
            }
            else if (incoming != null && outgoing == null)
            {
                type = "Incoming";
                amount = incoming;
            }

            var model = new MonitoringBudgetItemModel
            {
                ID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("ID")),
                Type = type,
                Amount = amount,
                Goal = DataConverter.ToString(dict.GetValueOrDefault("Goal")),
                ParagraphID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("ParagraphID")),
                DateOfTransfer = DataConverter.ToNullableDateTime(dict.GetValueOrDefault("DateOfTransfer")),
            };

            monitoringBudgetDataControl.FillFooter(model);
        }
        protected void monitoringBudgetDataControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter);

            var document = documents.SingleOrDefault();
            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            var incoming = DataConverter.ToNullableDouble(dict.GetValueOrDefault("Incoming"));
            var outgoing = DataConverter.ToNullableDouble(dict.GetValueOrDefault("Outgoing"));

            var type = String.Empty;
            var amount = (double?)null;

            if (incoming == null && outgoing != null)
            {
                type = "Outgoing";
                amount = outgoing;
            }
            else if (incoming != null && outgoing == null)
            {
                type = "Incoming";
                amount = incoming;
            }

            var model = new MonitoringBudgetItemModel
            {
                ID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("ID")),
                Type = type,
                Amount = amount,
                Goal = DataConverter.ToString(dict.GetValueOrDefault("Goal")),
                ParagraphID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("ParagraphID")),
                DateOfTransfer = DataConverter.ToNullableDateTime(dict.GetValueOrDefault("DateOfTransfer")),
            };

            monitoringBudgetDataControl.FillFooter(model);

            ApplyViewMode();
        }
        protected void monitoringBudgetDataControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter);
            var document = documents.SingleOrDefault();

            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            dict["DateDeleted"] = DateTime.Now;

            document = BsonDocumentConverter.ConvertToBsonDocument(dict);
            MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringBudgetCollectionName, document);

            NormilizeRemains();

            FillMonitoringData();
        }
        protected void monitoringBudgetDataControl_OnFiles(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter);

            var document = documents.SingleOrDefault();
            if (document == null)
            {
                documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringProjectCollectionName, filter);
                document = documents.SingleOrDefault();
            }

            var dict = BsonDocumentConverter.ConvertToDictionary(document);
            var status = Convert.ToString(dict.GetValueOrDefault("Status"));
            var flawID = dict.GetValueOrDefault("FlawsID") as Object[];

            var flaw = (status != MonitoringItemStatuses.Accepted && !flawID.IsNullOrEmpty());

            //var dateOfSubmit = DataConverter.ToNullableDateTime(FormData[FormDataConstants.DateOfMonitoringSubmitField]);

            var model = new MonitoringProjectFilesModel
            {
                ParentID = e.Value,
                //Submited = (dateOfSubmit != null),
                Flaw = flaw,
            };

            model.Files = GetFiles(e.Value);

            monitoringProjectFilesControl.Model = model;
            monitoringProjectFilesControl.DataBind();

            mpeMonitoringProjectFiles.Show();

            ApplyViewMode();
        }
        protected void monitoringBudgetDataControl_OnHistory(object sender, GenericEventArgs<Guid> e)
        {
            var dictionaries = LoadBudgetHitory(e.Value);

            var transfers = (from n in dictionaries
                             let t = new MoneyTransferEntity(n)
                             orderby t.DateOfTransfer, t.DateCreated
                             select t).ToList();

            monitoringBudgetHistoryControl.BindData(Paragraphs, Flews, transfers);

            mpeBudgetHistory.Show();
        }

        protected void btnSummaryBudgetExport_OnClick(object sender, EventArgs e)
        {
            var dataTable = GetSummaryBudgetData().ToDataTable();

            var target = sender == btnSummaryBudgetExport ? "Excel" : "PDF";
            ReportUnitHelper.Export(Response, target, dataTable);
        }

        protected void btnMonitoringStatusOK_OnClick(object sender, EventArgs e)
        {
            var model = monitoringStatusControl.Model;

            var filter = new Dictionary<String, Object>
            {
                ["ID"] = model.RecordID,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter);
            var document = documents.SingleOrDefault();

            if (document == null)
                return;

            var dictionary = BsonDocumentConverter.ConvertToDictionary(document);

            dictionary["FlawsID"] = model.FlawsID;
            dictionary["Comment"] = model.Comment;
            dictionary["Status"] = model.Status;
            dictionary["ExpireDate"] = model.ExpireDate;
            dictionary["StatusDate"] = DateTime.Now;
            dictionary["StatusUserID"] = UserUtil.GetCurrentUserID();

            document = BsonDocumentConverter.ConvertToBsonDocument(dictionary);
            MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringBudgetCollectionName, document);

            FillMonitoringData();

            mpeMonitoringStatus.Hide();
        }
        protected void btnMonitoringStatusCancel_OnClick(object sender, EventArgs e)
        {
            mpeMonitoringStatus.Hide();
        }
        protected void monitoringStatusControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeMonitoringStatus.Show();
        }

        protected void btnMonitoringProjectItemNew_OnClick(object sender, EventArgs e)
        {
            var projectTasks = GetProjectTasks(FormEntity, FormData);

            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringProjectCollectionName, filter);

            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary);
            var dictionariesLp = dictionaries.ToLookup(n => n["TaskID"]);

            foreach (var projectTask in projectTasks)
            {
                var @new = false;

                var dict = dictionariesLp[projectTask.ID].FirstOrDefault();
                if (dict == null)
                {
                    @new = true;

                    dict = new Dictionary<String, Object>
                    {
                        ["ID"] = Guid.NewGuid(),
                        ["OwnerID"] = FormID,
                        ["RecordID"] = RecordID,
                        ["TaskID"] = projectTask.ID,

                        ["Name"] = projectTask.Name,
                        ["StartDate"] = projectTask.StartDate,
                        ["EndDate"] = projectTask.EndDate,

                        ["IsDone"] = false,
                        ["DoneDate"] = null,
                        ["DoneUserID"] = null,

                        ["SubmitDate"] = null,
                        ["SubmitUserID"] = null,

                        ["DoneUserID"] = null,
                        ["DoneUserID"] = null,

                        ["CreateUserID"] = UserUtil.GetCurrentUserID(),

                        ["Warning"] = null,
                        ["WarningDescription"] = null,

                        ["DateCreated"] = DateTime.Now,
                        ["DateChanged"] = null,
                        ["DateDeleted"] = null,
                    };
                }

                dict["OwnerID"] = FormID;
                dict["RecordID"] = RecordID;
                dict["TaskID"] = projectTask.ID;
                dict["Name"] = projectTask.Name;
                dict["StartDate"] = projectTask.StartDate;
                dict["EndDate"] = projectTask.EndDate;

                var bsonDoc = BsonDocumentConverter.ConvertToBsonDocument(dict);

                if (@new)
                    MongoDbUtil.InsertDocument(MongoDbUtil.MonitoringProjectCollectionName, bsonDoc);
                else
                    MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringProjectCollectionName, bsonDoc);
            }

            FillProjectsData();
        }
        protected void btnMonitoringProjectExcel_OnClick(object sender, EventArgs e)
        {
            var dataTable = GetMonitoringProjectsData().ToDataTable();

            var target = sender == btnMonitoringProjectExcel ? "Excel" : "PDF";
            ReportUnitHelper.Export(Response, target, dataTable);
        }

        protected void btnMonitoringProjectStatusOk_OnClick(object sender, EventArgs e)
        {
            var model = monitoringProjectStatusControl.Model;

            var document = MongoDbUtil.GetDocument(MongoDbUtil.MonitoringProjectCollectionName, model.ID);
            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            dict["DoneDescription"] = model.Description;
            dict["DoneStatus"] = model.DoneStatus;

            var bsonDoc = BsonDocumentConverter.ConvertToBsonDocument(dict);

            MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringProjectCollectionName, bsonDoc);
            FillProjectsData();

            mpeMonitoringProjectItemStatus.Hide();
        }
        protected void btnMonitoringProjectStatusCancel_OnClick(object sender, EventArgs e)
        {
            mpeMonitoringProjectItemStatus.Hide();
        }

        protected void monitoringProjectFlawControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeMonitoringProjectFlawStatus.Show();
        }

        protected void monitoringProjectsDataControl_OnFlaw(object sender, GenericEventArgs<Guid> e)
        {
            var document = MongoDbUtil.GetDocument(MongoDbUtil.MonitoringProjectCollectionName, e.Value);
            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);
            var entity = new ProjectTaskEntity(dict);

            var model = new MonitoringProjectFlawItemModel
            {
                ID = e.Value,
                Status = entity.Status,
                FlawsID = entity.FlawsID,
                ExpireDate = entity.ExpireDate,
                Comment = entity.Comment
            };

            monitoringProjectFlawControl.Model = model;
            mpeMonitoringProjectFlawStatus.Show();
        }
        protected void monitoringProjectsDataControl_OnStatus(object sender, GenericEventArgs<Guid> e)
        {
            var document = MongoDbUtil.GetDocument(MongoDbUtil.MonitoringProjectCollectionName, e.Value);
            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);
            var entity = new ProjectTaskEntity(dict);

            var model = new MonitoringProjectStatusItemModel
            {
                ID = e.Value,
                DoneStatus = entity.DoneStatus,
                Description = entity.DoneDescription,
            };

            monitoringProjectStatusControl.Model = model;
            mpeMonitoringProjectItemStatus.Show();
        }
        protected void monitoringProjectsDataControl_OnUpload(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter);

            var document = documents.SingleOrDefault();
            if (document == null)
            {
                documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringProjectCollectionName, filter);
                document = documents.SingleOrDefault();
            }

            var dict = BsonDocumentConverter.ConvertToDictionary(document);
            var status = Convert.ToString(dict.GetValueOrDefault("Status"));
            var flawID = dict.GetValueOrDefault("FlawsID") as Object[];
            //var dateOfSubmit = DataConverter.ToNullableDateTime(FormData[FormDataConstants.DateOfMonitoringSubmitField]);

            var flaw = (status != MonitoringItemStatuses.Accepted && !flawID.IsNullOrEmpty());

            var model = new MonitoringProjectFilesModel
            {
                ParentID = e.Value,
                //Submited = (dateOfSubmit != null),
                Flaw = flaw,
            };

            model.Files = GetFiles(e.Value);

            monitoringProjectFilesControl.Model = model;
            monitoringProjectFilesControl.DataBind();

            mpeMonitoringProjectFiles.Show();

            ApplyViewMode();
        }
        protected void monitoringProjectsDataControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var document = MongoDbUtil.GetDocument(MongoDbUtil.MonitoringProjectCollectionName, e.Value);
            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);
            dict["DateDeleted"] = DateTime.Now;


            var bsonDoc = BsonDocumentConverter.ConvertToBsonDocument(dict);
            MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringProjectCollectionName, bsonDoc);
            FillProjectsData();
        }

        protected void btnBudgetHistoryCancel_OnClick(object sender, EventArgs e)
        {
            mpeBudgetHistory.Hide();
        }

        protected void btnMonitoringProjectFlawOK_OnClick(object sender, EventArgs e)
        {
            var model = monitoringProjectFlawControl.Model;

            var document = MongoDbUtil.GetDocument(MongoDbUtil.MonitoringProjectCollectionName, model.ID);
            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            dict["Status"] = model.Status;
            dict["FlawsID"] = model.FlawsID;
            dict["ExpireDate"] = model.ExpireDate;
            dict["Comment"] = model.Comment;

            var bsonDoc = BsonDocumentConverter.ConvertToBsonDocument(dict);

            MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringProjectCollectionName, bsonDoc);

            mpeMonitoringProjectFlawStatus.Hide();

            FillProjectsData();
        }
        protected void btnMonitoringProjectFlawCancel_OnClick(object sender, EventArgs e)
        {
            mpeMonitoringProjectFlawStatus.Hide();
        }

        protected void btnMonitoringProjectFilesOK_OnClick(object sender, EventArgs e)
        {
            var model = monitoringProjectFilesControl.Model;

            if (model.ParentID == null)
                return;

            if (!String.IsNullOrWhiteSpace(model.FileUrl))
            {
                var fileDict = new Dictionary<String, Object>
                {
                    ["ID"] = Guid.NewGuid(),
                    ["ParentID"] = model.ParentID,
                    ["FileName"] = null,
                    ["FileData"] = null,
                    ["FileUrl"] = model.FileUrl,
                    ["Description"] = model.Description,

                    ["DateCreated"] = DateTime.Now,
                    ["DateChanged"] = null,
                    ["DateDeleted"] = null,
                };

                var bsonFileDoc = BsonDocumentConverter.ConvertToBsonDocument(fileDict);
                MongoDbUtil.InsertDocument(MongoDbUtil.FilesCollectionName, bsonFileDoc);
            }

            if (!model.PostedFiles.IsNullOrEmpty())
            {
                var allowedFiles = new[] { ".doc", ".docx", ".xls", ".xlsx", ".pdf" };
                var files = model.PostedFiles;

                var isNotValid = (from n in files
                                  let f = Path.GetExtension(n.FileName)
                                  where !allowedFiles.Contains(f)
                                  select n).Any();

                if (isNotValid)
                {
                    lblFileResult.Text = "არასწორი ფაილის ფორმატი";
                    mpeMonitoringProjectFiles.Show();
                    return;
                }

                foreach (var postedFile in files)
                {
                    byte[] fileData;
                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(postedFile.ContentLength);
                    }

                    if (fileData.IsNullOrEmpty())
                        continue;

                    var fileDict = new Dictionary<String, Object>
                    {
                        ["ID"] = Guid.NewGuid(),
                        ["ParentID"] = model.ParentID,
                        ["FileName"] = postedFile.FileName,
                        ["FileData"] = fileData,
                        ["Description"] = model.Description,
                        ["FileUrl"] = null,

                        ["DateCreated"] = DateTime.Now,
                        ["DateChanged"] = null,
                        ["DateDeleted"] = null,
                    };

                    var bsonFileDoc = BsonDocumentConverter.ConvertToBsonDocument(fileDict);
                    MongoDbUtil.InsertDocument(MongoDbUtil.FilesCollectionName, bsonFileDoc);
                }
            }

            model.Files = GetFiles(model.ParentID);

            monitoringProjectFilesControl.Model = model;
            monitoringProjectFilesControl.DataBind();

            mpeMonitoringProjectFiles.Show();
        }
        protected void btnMonitoringProjectFilesCancel_OnClick(object sender, EventArgs e)
        {
            mpeMonitoringProjectFiles.Hide();
        }
        protected void monitoringProjectFilesControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var document = MongoDbUtil.GetDocument(MongoDbUtil.FilesCollectionName, e.Value);
            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);
            dict["DateDeleted"] = DateTime.Now;

            var bsonDoc = BsonDocumentConverter.ConvertToBsonDocument(dict);
            MongoDbUtil.UpdateDocument(MongoDbUtil.FilesCollectionName, bsonDoc);

            var model = monitoringProjectFilesControl.Model;

            model.Files = GetFiles(model.ParentID);
            monitoringProjectFilesControl.Model = model;
            monitoringProjectFilesControl.DataBind();

            mpeMonitoringProjectFiles.Show();
        }

        protected void btnBudgetApproveOK_OnClick(object sender, EventArgs e)
        {
            var period = cbxPeriods.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(period))
            {
                lblGlobalError.Text = "Please select Period";
                return;
            }

            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            if (startDate == null)
            {
                lblGlobalError.Text = "Please select Start Date";
                return;
            }

            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);
            if (endDate == null)
            {
                lblGlobalError.Text = "Please select End Date";
                return;
            }

            ChangeBudgetStatus(DataStatusCache.Accepted.ID, period, startDate, endDate);

            Response.Redirect(RequestUrl.ToEncodedUrl());
        }

        protected void btnProjectApproveOK_OnClick(object sender, EventArgs e)
        {
            var period = cbxPeriods.TryGetStringValue();
            if (String.IsNullOrWhiteSpace(period))
            {
                lblGlobalError.Text = "Please select Period";
                return;
            }

            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            if (startDate == null)
            {
                lblGlobalError.Text = "Please select Start Date";
                return;
            }

            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);
            if (endDate == null)
            {
                lblGlobalError.Text = "Please select End Date";
                return;
            }

            ChangeProjectStatus(DataStatusCache.Accepted.ID, period, startDate, endDate);

            Response.Redirect(RequestUrl.ToEncodedUrl());
        }

        protected void btnBudgetApproveCancel_OnClick(object sender, EventArgs e)
        {
            mpeBudgetApprove.Hide();
        }

        protected void btnProjectApproveCancel_OnClick(object sender, EventArgs e)
        {
            mpeProjectApprove.Hide();
        }

        protected void cbxPeriods_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            tbxStartDate.Text = tbxEndDate.Text = null;

            var currentPeriod = cbxPeriods.TryGetIntValue();
            if (currentPeriod == null)
                return;

            var periodLength = GetPeriodLength();

            var startDate = (DateTime?)null;
            var endDate = (DateTime?)null;

            var periodStartDateAlias = $"ProjectStartDate_{currentPeriod}";

            var periodStartDateField = ControlsLp[periodStartDateAlias].FirstOrDefault();
            if (periodStartDateField != null)
            {
                var periodStartDateKey = Convert.ToString(periodStartDateField.ID);

                var periodStartDate = DataConverter.ToNullableDateTime(FormData[periodStartDateKey]);
                if (periodStartDate == null)
                    return;

                startDate = periodStartDate;

                var periodEndDateAlias = $"ProjectEndDate_{currentPeriod}";

                var periodEndDateField = ControlsLp[periodEndDateAlias].FirstOrDefault();
                if (periodEndDateField != null)
                {
                    var periodEndDateKey = Convert.ToString(periodEndDateField.ID);

                    var periodEndDate = DataConverter.ToNullableDateTime(FormData[periodEndDateKey]);
                    endDate = periodEndDate;
                }
                else if (periodLength != null)
                {
                    endDate = startDate.Value.AddMonths(periodLength.Value);
                }
            }
            else
            {
                var projectStartDateField = ControlsLp["ProjectStartDate"].FirstOrDefault();
                if (projectStartDateField == null)
                    return;

                var projectStartDateKey = Convert.ToString(projectStartDateField.ID);

                var projectStartDate = DataConverter.ToNullableDateTime(FormData[projectStartDateKey]);
                if (projectStartDate == null)
                    return;

                if (currentPeriod < 1)
                    return;

                var start = periodLength.Value * (currentPeriod.Value - 1);

                startDate = projectStartDate.Value.AddMonths(start);
                endDate = startDate.Value.AddMonths(periodLength.Value).AddDays(-1);
            }

            tbxStartDate.Text = $"{startDate:dd.MM.yyyy}";
            tbxEndDate.Text = $"{endDate:dd.MM.yyyy}";
        }

        protected int? GetPeriodLength()
        {
            var periodLengthField = _controlsLp["PeriodLength"].FirstOrDefault();
            if (periodLengthField == null)
                return null;

            var periodLengthKey = Convert.ToString(periodLengthField.ID);

            var periodLength = DataConverter.ToNullableInt(FormData[periodLengthKey]);
            return periodLength;
        }

        protected void ApplyViewMode()
        {
            if (FormData == null)
                return;

            var orgs = GetOrgs();

            var organizationType = cbxOrganizationType.TryGetStringValue();
            if (!String.IsNullOrWhiteSpace(organizationType))
                orgs = orgs.Where(n => n.Group == organizationType);

            cbxOrganizations.BindData(orgs);

            if (UserUtil.IsSuperAdmin())
                return;

            //var paymentType = cbxReqPayment.TryGetStringValue();
            //if (!String.IsNullOrWhiteSpace(paymentType))
            //{
            //    orgs = orgs.Where(n => n.Group == paymentType);
            //}
            var period = cbxPeriods.TryGetStringValue();

            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);

            btnBudgetSubmit.Visible = UmUtil.Instance.HasAccess("Submit");
            btnBudgetApprove.Visible = (!IsBudgetPeriodApproved(period, startDate, endDate) && UmUtil.Instance.HasAccess("Org"));

            btnProjectsSubmit.Visible = UmUtil.Instance.HasAccess("Submit");
            btnProjectsApprove.Visible = (!IsProjectPeriodApproved(period, startDate, endDate) && UmUtil.Instance.HasAccess("Org"));

            if (!UmUtil.Instance.HasAccess("Monitoring") && !UmUtil.Instance.HasAccess("MonitoringStatus"))
            {
                var filesModel = monitoringProjectFilesControl.Model;
                var footerModel = monitoringBudgetDataControl.GetFooter();

                btnMonitoringProjectItemNew.Visible = false;

                btnMonitoringProjectFilesOK.Visible = filesModel.Flaw.GetValueOrDefault();
                monitoringBudgetDataControl.ShowFooter = (footerModel.ID != null);
            }
        }

        protected void NormilizeRemains()
        {
            var filter = new Dictionary<String, Object>
            {
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter);

            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary);

            var query = (from n in dictionaries
                         let dateCreated = DataConverter.ToNullableDateTime(n["DateCreated"])
                         let dateOfTransfer = DataConverter.ToNullableDateTime(n["DateOfTransfer"])
                         orderby dateOfTransfer, dateCreated
                         select n);

            var remain = (double?)0D;

            foreach (var item in query)
            {
                var incoming = DataConverter.ToNullableDouble(item["Incoming"]);
                var outgoing = DataConverter.ToNullableDouble(item["Outgoing"]);

                if (incoming != null && outgoing == null)
                    remain += incoming;
                else if (incoming == null && outgoing != null)
                    remain -= outgoing;

                item["Remain"] = remain;

                var document = BsonDocumentConverter.ConvertToBsonDocument(item);
                MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringBudgetCollectionName, document);
            }
        }

        protected void FillProjectsData()
        {
            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var sorts = new[] { "DateCreated" };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringProjectCollectionName, filter, sorts);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary);
            var projectTasks = dictionaries.Select(n => new ProjectTaskEntity(n)).ToList();

            monitoringProjectFlawControl.BindData(Flews);

            monitoringProjectsGridsControl.BindData(FormData, projectTasks);
            monitoringProjectsDataControl.BindData(Flews, projectTasks);
        }

        protected void FillMonitoringData()
        {
            var projectPeriodsField = ControlsLp["PeriodAmount"].FirstOrDefault();
            if (projectPeriodsField != null)
            {
                var projectPeriodsKey = Convert.ToString(projectPeriodsField.ID);

                var projectPeriods = DataConverter.ToNullableInt(FormData[projectPeriodsKey]);
                if (projectPeriods != null)
                {
                    var periodsQuery = (from n in Enumerable.Range(1, projectPeriods.Value)
                                        select new
                                        {
                                            Name = Convert.ToString(n),
                                            Value = Convert.ToString(n)
                                        });

                    cbxPeriods.BindData(periodsQuery);
                }
            }

            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);

            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var paymentType = cbxFinancingType.TryGetStringValue();
            if (!String.IsNullOrWhiteSpace(paymentType))
                filter["PaymentType"] = paymentType;

            var organizationType = cbxOrganizationType.TryGetStringValue();
            if (!String.IsNullOrWhiteSpace(organizationType))
                filter["OrganizationType"] = organizationType;

            var organizationID = cbxOrganizations.TryGetGuidValue();
            if (!String.IsNullOrWhiteSpace(organizationType))
                filter["OrganizationID"] = organizationID;


            var sorts = new[] { "DateCreated" };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter, sorts);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary);

            var transfersQuery = dictionaries.Select(n => new MoneyTransferEntity(n));

            if (startDate != null)
            {
                transfersQuery = (from n in transfersQuery
                                  where n.DateOfTransfer >= startDate
                                  select n);
            }

            if (endDate != null)
            {
                transfersQuery = (from n in transfersQuery
                                  where n.DateOfTransfer <= endDate
                                  select n);
            }

            transfersQuery = (from n in transfersQuery
                              orderby n.DateOfTransfer, n.DateCreated
                              select n);

            var transfersList = transfersQuery.ToList();

            var comparer = StringComparer.OrdinalIgnoreCase;

            var paragraphs = Paragraphs.AsEnumerable();

            var reqPayment = cbxFinancingType.TryGetStringValue();
            var paragraphOrg = cbxOrganizationType.TryGetStringValue();

            if (!String.IsNullOrWhiteSpace(paragraphOrg))
            {
                paragraphs = (from n in paragraphs
                              where comparer.Equals(n.Category, paragraphOrg)
                              select n);
            }

            monitoringStatusControl.BindData(Flews);

            monitoringBudgetDataControl.BindData(paragraphs, Flews, transfersList);

            summaryBudgetDataControl.BindData(startDate, endDate, reqPayment, FormEntity, FormData, paragraphs, transfersList);
        }

        protected IEnumerable<MonitoringFlewEntity> GetFlews()
        {
            if (FormEntity == null)
                yield break;

            var monitoring = FormEntity.Monitoring;
            if (monitoring == null)
                yield break;

            var flews = monitoring.Flews;
            if (flews == null)
                yield break;

            var list = flews.OrderBy(n => n.Name);
            foreach (var entity in list)
                yield return entity;
        }

        protected IEnumerable<FileEntity> GetFiles(Guid? parentID)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ParentID"] = parentID,
                ["DateDeleted"] = null,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.FilesCollectionName, filter);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary).ToList();

            var entities = dictionaries.Select(n => new FileEntity(n));
            return entities;
        }

        protected IEnumerable<MoneyTransferEntity> LoadMoneyTransfers()
        {
            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var sorts = new[] { "DateCreated" };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter, sorts);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary);

            var entities = dictionaries.Select(n => new MoneyTransferEntity(n));
            return entities;
        }

        protected FormDataUnit LoadFormData(Guid? ownerID, Guid? recordID)
        {
            if (ownerID == null || recordID == null)
                return null;

            var filter = new Dictionary<String, Object>
            {
                {FormDataConstants.IDField, recordID }
            };

            var documents = MongoDbUtil.FindDocuments(ownerID, filter).ToList();

            var document = documents.SingleOrDefault();
            if (document == null)
                return null;

            var formData = BsonDocumentConverter.ConvertToFormDataUnit(document, true);
            return formData;
        }

        protected int CompareMoneyTransfer(MoneyTransferEntity x, MoneyTransferEntity y)
        {
            if (x == null && x == null)
                return 0;

            if (x == null && y != null)
                return -1;

            if (x != null && y == null)
                return 1;

            var order = x.DateOfTransfer.GetValueOrDefault().CompareTo(y.DateOfTransfer.GetValueOrDefault());
            if (order == 0)
                order = x.DateCreated.GetValueOrDefault().CompareTo(y.DateCreated.GetValueOrDefault());

            return order;
        }
        protected bool FindMoneyTransferPredicate(MoneyTransferEntity x, MoneyTransferEntity y)
        {
            var order = CompareMoneyTransfer(x, y);
            if (order > -1)
                return true;

            return false;
        }

        protected bool IsBudgetPeriodApproved(String period, DateTime? startDate, DateTime? endDate)
        {
            var formStatusUnits = FormStatusUnits;
            if (formStatusUnits == null)
                return false;

            var key = $"{UserUtil.GetCurrentUserID()}_{DataStatusCache.Accepted.ID}";

            var userStatuses = formStatusUnits[key];
            foreach (var statusUnit in userStatuses)
            {
                var @params = statusUnit.Params;
                if (@params != null && @params.ContainsKey(FormDataConstants.MonitoringBudget))
                {
                    var statusPeriod = DataConverter.ToString(@params.GetValueOrDefault(FormDataConstants.MonitoringPeriod));
                    if (statusPeriod == period)
                        return true;

                    //var statusEndDate = DataConverter.ToNullableDateTime(@params.GetValueOrDefault(FormDataConstants.MonitoringStartDate));
                    //var statusStartDate = DataConverter.ToNullableDateTime(@params.GetValueOrDefault(FormDataConstants.MonitoringEndDate));

                    //if (GmsCommonUtil.DateBetween(statusStartDate, startDate, endDate) &&
                    //    GmsCommonUtil.DateBetween(statusEndDate, startDate, endDate))
                    //    return true;
                }
            }

            return false;
        }
        protected bool IsProjectPeriodApproved(String period, DateTime? startDate, DateTime? endDate)
        {
            var formStatusUnits = FormStatusUnits;
            if (formStatusUnits == null)
                return false;

            var key = $"{UserUtil.GetCurrentUserID()}_{DataStatusCache.Accepted.ID}";

            var userStatuses = formStatusUnits[key];
            foreach (var statusUnit in userStatuses)
            {
                var @params = statusUnit.Params;
                if (@params != null && @params.ContainsKey(FormDataConstants.MonitoringProject))
                {
                    var statusPeriod = DataConverter.ToString(@params.GetValueOrDefault(FormDataConstants.MonitoringPeriod));
                    if (statusPeriod == period)
                        return true;

                    //var statusEndDate = DataConverter.ToNullableDateTime(@params.GetValueOrDefault(FormDataConstants.MonitoringStartDate));
                    //var statusStartDate = DataConverter.ToNullableDateTime(@params.GetValueOrDefault(FormDataConstants.MonitoringEndDate));

                    //if (GmsCommonUtil.DateBetween(statusStartDate, startDate, endDate) &&
                    //    GmsCommonUtil.DateBetween(statusEndDate, startDate, endDate))
                    //    return true;
                }
            }

            return false;
        }

        protected bool ChangeBudgetStatus(Guid? statusID, String period, DateTime? startDate, DateTime? endDate)
        {
            var document = MongoDbUtil.GetDocument(FormID, RecordID);

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
            {
                bsonValue = new BsonArray();
                document[FormDataConstants.UserStatusesFields] = bsonValue;
            }

            var oldArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (oldArray == null)
                oldArray = new BsonArray();

            var formStatuses = BsonDocumentConverter.ConvertToFormStatuses(oldArray).ToList();

            var query = (from n in formStatuses
                         where n.UserID == UserUtil.GetCurrentUserID() &&
                               n.Params != null &&
                               n.Params.ContainsKey(FormDataConstants.MonitoringBudget)
                         select n);

            var formStatus = query.FirstOrDefault();
            if (formStatus == null)
            {
                formStatus = new FormStatusUnit
                {
                    Params = new Dictionary<String, Object>
                    {
                        [FormDataConstants.MonitoringBudget] = "@",
                    }
                };
            }

            formStatus.UserID = UserUtil.GetCurrentUserID();
            formStatus.StatusID = (statusID ?? formStatus.StatusID);
            formStatus.DateOfStatus = DateTime.Now;

            formStatus.Params[FormDataConstants.MonitoringPeriod] = period;
            formStatus.Params[FormDataConstants.MonitoringStartDate] = startDate;
            formStatus.Params[FormDataConstants.MonitoringEndDate] = endDate;

            var newArray = BsonDocumentConverter.ConvertToFormStatusesArray(formStatuses);
            document[FormDataConstants.UserStatusesFields] = newArray;

            MongoDbUtil.UpdateDocument(FormID, document);
            return true;
        }
        protected bool ChangeProjectStatus(Guid? statusID, String period, DateTime? startDate, DateTime? endDate)
        {
            var document = MongoDbUtil.GetDocument(FormID, RecordID);

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
            {
                bsonValue = new BsonArray();
                document[FormDataConstants.UserStatusesFields] = bsonValue;
            }

            var oldArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (oldArray == null)
                oldArray = new BsonArray();

            var formStatuses = BsonDocumentConverter.ConvertToFormStatuses(oldArray).ToList();

            var query = (from n in formStatuses
                         where n.UserID == UserUtil.GetCurrentUserID() &&
                               n.Params != null &&
                               n.Params.ContainsKey(FormDataConstants.MonitoringProject)
                         select n);

            var formStatus = query.FirstOrDefault();
            if (formStatus == null)
            {
                formStatus = new FormStatusUnit
                {
                    Params = new Dictionary<String, Object>
                    {
                        [FormDataConstants.MonitoringProject] = "@",
                    }
                };
            }

            formStatus.UserID = UserUtil.GetCurrentUserID();
            formStatus.StatusID = (statusID ?? formStatus.StatusID);
            formStatus.DateOfStatus = DateTime.Now;

            formStatus.Params[FormDataConstants.MonitoringPeriod] = period;
            formStatus.Params[FormDataConstants.MonitoringStartDate] = startDate;
            formStatus.Params[FormDataConstants.MonitoringEndDate] = endDate;

            var newArray = BsonDocumentConverter.ConvertToFormStatusesArray(formStatuses);
            document[FormDataConstants.UserStatusesFields] = newArray;

            MongoDbUtil.UpdateDocument(FormID, document);
            return true;
        }

        protected IEnumerable<ProjectItemInfoEntity> GetProjectTasks(FormEntity formEntity, FormDataUnit formData)
        {
            if (formEntity == null)
                yield break;

            var comparer = StringLogicalComparer.OrdinalIgnoreCase;
            var controls = FormStructureUtil.PreOrderIndexedTraversal(formEntity);

            var dataGrids = (from n in controls
                             where (n is GridEntity || n is TreeEntity)
                             select n);

            var controlLp = dataGrids.ToLookup(n => n.Alias, comparer);

            var projectTasks = controlLp["ProjectTasks"].SingleOrDefault() as ContentEntity;
            if (projectTasks == null)
                yield break;

            var projectTaskFields = FormStructureUtil.PreOrderIndexedTraversal(projectTasks);
            var projectTaskFieldsLp = projectTaskFields.ToLookup(n => n.Alias, comparer);

            var taskNameField = projectTaskFieldsLp["ProjectTasks_Name"].SingleOrDefault();
            if (taskNameField == null)
                throw new Exception(@"Unable to find 'ProjectTasks\TaskName'");

            var taskStartDateField = projectTaskFieldsLp["ProjectTasks_StartDate"].SingleOrDefault();
            if (taskStartDateField == null)
                throw new Exception(@"Unable to find 'ProjectTasks\TaskStartDate'");

            var taskEndDateField = projectTaskFieldsLp["ProjectTasks_EndDate"].SingleOrDefault();
            if (taskEndDateField == null)
                throw new Exception(@"Unable to find 'ProjectTasks\TaskEndDate'");

            var projectTasksKey = Convert.ToString(projectTasks.ID);
            var projectTaskNameKey = Convert.ToString(taskNameField.ID);
            var projectTaskStartDateKey = Convert.ToString(taskStartDateField.ID);
            var projectTaskEndDateKey = Convert.ToString(taskEndDateField.ID);

            var projectTasksData = GetFormDataList(formData[projectTasksKey]);
            if (projectTasksData == null)
                yield break;

            foreach (var projectTaskData in projectTasksData)
            {
                var itemID = DataConverter.ToNullableGuid(projectTaskData[FormDataConstants.IDField]);
                var taskName = DataConverter.ToString(projectTaskData[projectTaskNameKey]);
                var taskStartDate = DataConverter.ToNullableDateTime(projectTaskData[projectTaskStartDateKey]);
                var taskEndDate = DataConverter.ToNullableDateTime(projectTaskData[projectTaskEndDateKey]);

                var entity = new ProjectItemInfoEntity
                {
                    ID = itemID,
                    Name = taskName,
                    EndDate = taskEndDate,
                    StartDate = taskStartDate
                };

                yield return entity;
            }
        }

        protected IEnumerable<BudgetParagraphEntity> GenParagraphsTree()
        {
            return GenParagraphsTree(null, FormEntity);
        }
        protected IEnumerable<BudgetParagraphEntity> GenParagraphsTree(Guid? parentID, ControlEntity control)
        {
            //TODO: dasaoptimizirebelia, gadasayvania RWayTrie, TrenaryTrie-ze

            var content = control as ContentEntity;
            if (content == null || content.Controls == null)
                yield break;

            foreach (var item in content.Controls)
            {
                var actualParent = parentID;
                var controlAlias = (item.Alias ?? String.Empty);

                if (RegexUtil.BudgetElementParserRx.IsMatch(controlAlias))
                {
                    var match = RegexUtil.BudgetElementParserRx.Match(controlAlias);

                    actualParent = item.ID;

                    var entity = new BudgetParagraphEntity
                    {
                        ID = actualParent,
                        Name = item.Name,
                        ParentID = parentID,
                        Category = match.Groups["type"].Value,
                    };

                    var subContent = item as ContentEntity;
                    if (subContent != null && subContent.Controls != null)
                    {
                        var query = (from n in subContent.Controls
                                     where IsParagraphValieControl(n)
                                     select n).OfType<ContentEntity>();

                        var container = (from n in query
                                         where IsContainerParagraph(n)
                                         select n).Any();

                        entity.Container = container;

                        foreach (var child in query)
                        {
                            if (child is TreeEntity || child is GridEntity)
                            {
                                var contentParagraphs = GetPharagraphsFromData(actualParent, child);
                                foreach (var paragraph in contentParagraphs)
                                    yield return paragraph;
                            }
                        }
                    }

                    yield return entity;
                }

                foreach (var subChild in GenParagraphsTree(actualParent, item))
                    yield return subChild;
            }
        }
        protected IEnumerable<BudgetParagraphEntity> GetPharagraphsFromData(Guid? parentID, ContentEntity content)
        {
            var fields = (from m in FormStructureUtil.PreOrderTraversal(content)
                          let a = (m.Alias ?? String.Empty)
                          where a.StartsWith("ProjectBudget_Name", StringComparison.OrdinalIgnoreCase)
                          select m).OfType<FieldEntity>();

            var contentKey = Convert.ToString(content.ID);

            var dataGrid = FormData[contentKey] as FormDataListBase;
            if (dataGrid == null)
                yield break;

            foreach (var field in fields)
            {
                var fieldKey = Convert.ToString(field.ID);

                var fieldDataSourceUtil = new FieldDataSourceUtil(FormData.UserID, FormEntity, content, field);

                foreach (var formData in dataGrid)
                {
                    var value = formData[fieldKey];
                    var text = Convert.ToString(value);

                    if (value != null)
                        text = fieldDataSourceUtil.GetFieldText(value);

                    var itemKey = $"{formData.ID}_{field.ID}";
                    var itemID = itemKey.ComputeMd5Guid();

                    var entity = new BudgetParagraphEntity
                    {
                        ID = itemID,
                        Name = text,
                        FieldID = field.ID,
                        ParentID = parentID,
                        ContentID = content.ID,
                        Container = true,
                        DependOnValue = value,
                    };

                    yield return entity;
                }
            }
        }

        protected IEnumerable<IDictionary<String, Object>> LoadBudgetHitory(Guid? itemID)
        {
            var document = MongoDbUtil.GetDocument(MongoDbUtil.MonitoringBudgetCollectionName, itemID);
            while (document != null)
            {
                var dictionary = BsonDocumentConverter.ConvertToDictionary(document);
                yield return dictionary;

                var previousID = DataConverter.ToNullableGuid(dictionary.GetValueOrDefault("PreviousID"));
                if (previousID == null)
                    yield break;

                document = MongoDbUtil.GetDocument(MongoDbUtil.MonitoringBudgetCollectionName, previousID);
            }
        }

        protected IEnumerable<MonitoringBudgetDataEntity> GetBudgetData()
        {
            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);

            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var sorts = new[] { "DateCreated" };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter, sorts);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary);

            var transfersQuery = dictionaries.Select(n => new MoneyTransferEntity(n));

            if (startDate != null)
            {
                transfersQuery = (from n in transfersQuery
                                  where n.DateOfTransfer >= startDate
                                  select n);
            }

            if (endDate != null)
            {
                transfersQuery = (from n in transfersQuery
                                  where n.DateOfTransfer <= endDate
                                  select n);
            }

            transfersQuery = (from n in transfersQuery
                              orderby n.DateOfTransfer, n.DateCreated
                              select n);

            var result = monitoringBudgetDataControl.GetData(Paragraphs, transfersQuery);
            return result;
        }

        protected IEnumerable<SummaryBudgetEntity> GetSummaryBudgetData()
        {
            var startDate = DataConverter.ToNullableDateTime(tbxStartDate.Text);
            var endDate = DataConverter.ToNullableDateTime(tbxEndDate.Text);

            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var sorts = new[] { "DateCreated" };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringBudgetCollectionName, filter, sorts);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary).ToList();

            var comparer = StringComparer.OrdinalIgnoreCase;

            var paragraphs = Paragraphs.AsEnumerable();

            var reqPayment = cbxFinancingType.TryGetStringValue();
            var paragraphOrg = cbxOrganizationType.TryGetStringValue();

            if (!String.IsNullOrWhiteSpace(paragraphOrg))
            {
                paragraphs = (from n in paragraphs
                              where comparer.Equals(n.Category, paragraphOrg)
                              select n);
            }

            var result = summaryBudgetDataControl.GetSummaryGridData(startDate, endDate, reqPayment, FormEntity, FormData, paragraphs, dictionaries);
            return result;
        }

        protected IEnumerable<ProjectsDataEntity> GetMonitoringProjectsData()
        {
            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var sorts = new[] { "DateCreated" };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringProjectCollectionName, filter, sorts);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary).ToList();
            var entities = dictionaries.Select(n => new ProjectTaskEntity(n)).ToList();

            var result = monitoringProjectsDataControl.GetData(Flews, entities);
            return result;
        }

        protected void ExportToPdf(MonitoringPrintFieldEntity template)
        {
            var budgetData = GetBudgetData();
            var summaryData = GetSummaryBudgetData();
            var projectsData = GetMonitoringProjectsData();

            var velocityContext = new Dictionary<String, Object>
            {
                ["nvUtil"] = new NVelocityUtil(FormData, FormEntity),
                ["FormEntity"] = FormEntity,
                ["BudgetData"] = budgetData,
                ["SummaryData"] = summaryData,
                ["ProjectsData"] = projectsData,
                ["langPair"] = LanguageUtil.GetLanguage(),
            };

            var velocityEngine = NVelocityEngineFactory.CreateNVelocityMemoryEngine();
            var processedText = velocityEngine.Process(velocityContext, template.Template);

            var name = $"{template.Name} - {DateTime.Now:dd.MM.yyyy HH.mm}";

            var layaut = template.IsLendscape.GetValueOrDefault() ? "Landscape" : "Portrait";
            PdfUtil.HtmlToPdf(Response, processedText, name, layaut);
        }

        protected bool IsParagraphValieControl(ControlEntity control)
        {
            if (control == null)
                return false;

            if (!(control is GridEntity || control is TreeEntity))
                return false;

            if (String.IsNullOrWhiteSpace(control.Alias))
                return false;

            if (!control.Alias.StartsWith("ProjectBudget", StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        protected bool IsContainerParagraph(ContentEntity control)
        {
            var query = from m in FormStructureUtil.PreOrderTraversal(control)
                        let a = (m.Alias ?? String.Empty)
                        where a.StartsWith("ProjectBudget_Name", StringComparison.OrdinalIgnoreCase)
                        select m;

            if (!query.Any())
            {
                var key = Convert.ToString(control.ID);

                var list = GetFormDataList(FormData[key]);
                if (list == null)
                    return false;

                var count = list.Count;
                return count > 0;
            }

            return false;
        }

        protected FormDataListBase GetFormDataList(Object value)
        {
            if (value == null)
                return null;

            var dataList = value as FormDataListBase;
            if (dataList != null)
                return (FormDataListBase)value;

            var listRef = value as FormDataListRef;
            if (listRef != null)
                return new FormDataLazyList(listRef);

            return null;
        }

        protected IEnumerable<KeyNameGroupEntity> GetOrgs()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

            var controls = FormStructureUtil.PreOrderTraversal(FormEntity);
            var controlsLp = controls.ToLookup(n => n.Alias, comparer);

            var primaryOrgField = controlsLp["LeadOrgName"].OfType<FieldEntity>().FirstOrDefault();
            if (primaryOrgField != null)
            {
                var primaryOrgKey = Convert.ToString(primaryOrgField.ID);
                var primaryOrgValue = FormData[primaryOrgKey];

                var primaryOrgDsUtil = new FieldDataSourceUtil(null, FormEntity, primaryOrgField);
                var primaryOrgText = primaryOrgDsUtil.GetFieldText(primaryOrgValue);

                var entity = new KeyNameGroupEntity
                {
                    Key = primaryOrgValue,
                    Name = primaryOrgText,
                    Group = "Leading"
                };

                yield return entity;
            }

            var custodianOrgsGrid = controlsLp["CustodianOrgs"].OfType<ContentEntity>().FirstOrDefault();
            if (custodianOrgsGrid == null)
                yield break;

            var custodianOrgsKey = Convert.ToString(custodianOrgsGrid.ID);
            var custodianOrgsValue = FormData[custodianOrgsKey];

            var custodianOrgsList = GetFormDataList(custodianOrgsValue);
            if (custodianOrgsList == null)
                yield break;

            var fields = FormStructureUtil.PreOrderTraversal(custodianOrgsGrid);
            var fieldsLp = fields.ToLookup(n => n.Alias, comparer);

            var custodianOrgField = fieldsLp["CustodianOrgName"].OfType<FieldEntity>().FirstOrDefault();
            if (custodianOrgField == null)
                yield break;

            var custodianOrgKey = Convert.ToString(custodianOrgField.ID);
            var custodianOrgDsUtil = new FieldDataSourceUtil(null, custodianOrgsGrid, custodianOrgField);

            foreach (var formData in custodianOrgsList)
            {
                var custodianOrgValue = formData[custodianOrgKey];
                var custodianOrgText = custodianOrgDsUtil.GetFieldText(custodianOrgValue);

                var entity = new KeyNameGroupEntity
                {
                    Key = custodianOrgValue,
                    Name = custodianOrgText,
                    Group = "Custodian"
                };

                yield return entity;
            }
        }
    }
}