using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Collections;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Enums;
using Gms.Portal.Web.Models;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FormDataView : BasePage
    {
        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
        }

        public Guid? UserID
        {
            get
            {
                if (FormData != null)
                    return FormData.UserID;

                return DataConverter.ToNullableGuid(RequestUrl["UserID"]);
            }
        }

        public Guid? OwnerID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["OwnerID"]); }
        }

        public Guid? RecordID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["RecordID"]); }
        }

        public Guid? ParentID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["ParentID"]); }
        }

        public Guid? ContainerID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["ContainerID"]); }
        }

        public Guid? StatusID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["StatusID"]); }
        }

        public FormMode? Mode
        {
            get { return DataConverter.ToNullableEnum<FormMode>(RequestUrl["Mode"]); }
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
                    _formData = LoadFormData(OwnerID, RecordID);

                return _formData;
            }
        }

        private FormDataUnit _parentFormData;
        protected FormDataUnit ParentFormData
        {
            get
            {
                if (ParentID == null || FormID == null)
                    return null;

                if (_parentFormData == null)
                    _parentFormData = LoadFormData(FormID, ParentID);

                return _parentFormData;
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

        protected void Page_Init(object sender, EventArgs e)
        {
            if (OwnerID == null)
                return;

            if (DbForm == null)
                return;

            if (FormEntity == null)
                return;

            var userID = (UserID ?? UserUtil.GetCurrentUserID());

            btnEdit.Visible = false;
            btnSave.Visible = false;
            btnPrint.Visible = false;
            btnSubmit.Visible = false;
            btnNewHistory.Visible = false;

            formDataControl.Enabled = false;
            formDataControl.FormID = FormID;
            formDataControl.UserID = userID;
            formDataControl.OwnerID = OwnerID;
            formDataControl.RecordID = RecordID;
            formDataControl.ParentID = ParentID;
            formDataControl.StatusID = StatusID;
            formDataControl.ContainerID = ContainerID;

            if (UmUtil.Instance.HasAccess("Admin") && RecordID != null && ParentID == null)
                btnNewHistory.Visible = true;

            if (FormData == null)
            {
                formDataControl.Enabled = true;
                btnSave.Visible = true;

                if (ParentID == null)
                    btnSubmit.Visible = true;
            }
            else
            {
                if (ParentID != null)
                {
                    if (RecordID == null || Mode == FormMode.Edit)
                    {
                        formDataControl.Enabled = true;
                        btnSave.Visible = true;
                    }
                }
                else
                {
                    btnPrint.Visible = true;

                    if (FormData.StatusID != DataStatusCache.Submit.ID &&
                        FormData.StatusID != DataStatusCache.Accepted.ID)
                    {
                        if (Mode != FormMode.View)
                        {
                            formDataControl.Enabled = true;
                            btnSubmit.Visible = true;
                            btnSave.Visible = true;
                        }
                    }

                    if (FormData.StatusID == DataStatusCache.Accepted.ID)
                    {
                        if (Mode == FormMode.Edit)
                        {
                            formDataControl.Enabled = true;
                            btnSave.Visible = true;
                        }
                        else if (Mode != FormMode.View)
                        {
                            btnEdit.Visible = true;
                        }
                    }

                    if (FormData.StatusID == DataStatusCache.Rejected.ID && !FormData.ReviewFields.IsNullOrEmpty())
                    {
                        formDataControl.Enabled = true;
                        btnSubmit.Visible = true;
                        btnSave.Visible = true;
                    }

                    if (Mode == FormMode.Inspect)
                    {
                        formDataControl.Enabled = true;
                        btnSave.Visible = true;
                    }
                }
            }

            btnSave.CssClass = (btnSave.Enabled ? "btn btn-success fa fa-save" : "btn btn-default fa fa-save");
            btnFiles.Visible = (ParentID == null);

            formDataControl.Mode = Mode;

            var formData = FormData;
            var mainEntity = (ContentEntity)FormEntity;
            var parentEntity = (ContentEntity)null;

            if (OwnerID != null && OwnerID != FormID && FormEntity.Controls != null)
            {
                var allControls = FormStructureUtil.PreOrderTraversal(FormEntity);
                var control = allControls.FirstOrDefault(n => n.ID == OwnerID);

                var contentEntity = control as ContentEntity;
                if (contentEntity != null)
                {
                    mainEntity = contentEntity;
                    parentEntity = FormEntity;
                }
            }

            formDataControl.InitStructure(mainEntity, parentEntity, formData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillFormData();
            FillTemplates();

            if (!IsPostBack)
            {
                var value = Convert.ToString(RequestUrl["Tabs"]);
                if (!String.IsNullOrWhiteSpace(value))
                {
                    var parts = value.Split(',');
                    var tabs = (from n in parts
                                let g = DataConverter.ToNullableGuid(n)
                                where g != null
                                select g.Value);

                    var @set = tabs.ToHashSet();
                    formDataControl.SetActiveTabs(@set);
                }

                CheckFormAccesibility();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //var query = (from n in DiagUtil.Current.Elapseds
            //             let m = $"[{n.Key}, {n.Value}, {DiagUtil.Current.Occurrence[n.Key]}]"
            //             select m);

            //lblTimes.Text = String.Join("; ", query);
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var canAccess = GetSaveSubmitAccesibility();
            if (!canAccess)
                return;

            var newFormData = formDataControl.GetFormData();

            var newRecordID = SaveFormData(newFormData, FormData);
            if (newRecordID == null)
                return;

            if (FormData != null)
            {
                if (newFormData.StatusID != FormData.StatusID &&
                    FormData.StatusID == DataStatusCache.Accepted.ID)
                {
                    if (Mode == FormMode.Edit)
                    {
                        lblNotifyMessage.Text = "Submit required";
                        mpeNotifyMessage.Show();
                        return;
                    }
                }
            }

            var returnUrl = Convert.ToString(RequestUrl["ReturnUrl"]);
            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                var cleanUrl = GmsCommonUtil.ConvertFromBase64(returnUrl);

                var returnUrlHelper = new UrlHelper(cleanUrl);
                Response.Redirect(returnUrlHelper.ToEncodedUrl());
            }
        }

        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            var model = new ChooseTemplateModel
            {
                RecordID = RecordID,
            };

            chooseTemplateControl.Model = model;
            mpeChooseTemplate.Show();
        }

        protected void btnEdit_OnClick(object sender, EventArgs e)
        {
            var url = new UrlHelper(RequestUrl)
            {
                ["Mode"] = "Edit"
            };

            Response.Redirect(url.ToEncodedUrl());
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            var recordID = RecordID;
            if (recordID == null)
                return;

            var canAccess = GetSaveSubmitAccesibility();
            if (!canAccess)
                return;

            var newFormData = formDataControl.GetFormData();

            var controls = FormStructureUtil.PreOrderFirstLevelTraversal(FormEntity);
            var fields = controls.Select(n => Convert.ToString(n.ID)).ToHashSet();

            if (!FormDataUtil.MergeAndEquals(newFormData, FormData, fields))
            {
                recordID = SaveFormData(newFormData, FormData);
                if (recordID == null)
                    return;
            }

            if (!ValidateFormData(newFormData))
                return;

            var ownerID = OwnerID.GetValueOrDefault();
            var submitStatus = DataStatusCache.Submit;

            FormDataDbUtil.ChangeStatus(ownerID, recordID.Value, submitStatus.ID, String.Empty);

            var redirectUrl = new UrlHelper(RequestUrl)
            {
                ["RecordID"] = recordID,
                ["Mode"] = Convert.ToString(FormMode.View)
            };

            Response.Redirect(redirectUrl.ToEncodedUrl());
        }

        protected void btnFiles_OnClick(object sender, EventArgs e)
        {
            var activeTabs = formDataControl.GetActiveTabs();

            var returnUrl = new UrlHelper(RequestUrl.ToString())
            {
                ["Tabs"] = String.Join(",", activeTabs)
            };

            var urlHelper = new UrlHelper("~/Pages/Management/FilesList.aspx")
            {
                ["ParentID"] = FormID,
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl.ToString())
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            var returnUrl = Convert.ToString(RequestUrl["ReturnUrl"]);
            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                var cleanUrl = GmsCommonUtil.ConvertFromBase64(returnUrl);

                var returnUrlHelper = new UrlHelper(cleanUrl);
                Response.Redirect(returnUrlHelper.ToEncodedUrl());
            }
        }

        protected void btnValidate_OnClick(object sender, EventArgs e)
        {
            var oldFormData = FormData;
            var newFormData = formDataControl.GetFormData();

            FormDataUtil.MergeFormDatas(newFormData, oldFormData);

            ValidateFormData(newFormData);
        }

        protected void btnNewHistory_OnClick(object sender, EventArgs e)
        {
            var url = new UrlHelper("~/Handlers/PrintFormData.ashx")
            {
                ["FormID"] = FormID,
                ["OwnerID"] = OwnerID,
                ["RecordID"] = RecordID,
                ["LoginToken"] = UmUtil.Instance.CurrentToken
            };

            using (var client = new WebClient())
            {
                var encUrl = url.ToEncodedUrl();
                var absUrl = HttpServerUtil.ResolveAbsoluteUrl(encUrl);

                var bytes = client.DownloadData(absUrl);

                var dict = new Dictionary<String, Object>
                {
                    ["ID"] = Guid.NewGuid(),
                    ["UserID"] = UserUtil.GetCurrentUserID(),
                    ["ParentID"] = RecordID,
                    ["DateCreated"] = DateTime.Now,
                    ["DateDeleted"] = null,
                    ["RawData"] = bytes
                };

                var bsonDoc = BsonDocumentConverter.ConvertToBsonDocument(dict);

                var historyCollection = MongoDbUtil.GetCollection(MongoDbUtil.HistoryCollectionName);
                historyCollection.InsertOne(bsonDoc);
            }

            lblNotifyMessage.Text = "History saved!";
            mpeNotifyMessage.Show();
        }

        protected void btnCloneDataGridOK_Click(object sender, EventArgs e)
        {
            var model = cloneFormGridControl.Model;
            if (model.SourceGridID == null || model.TargetGridID == null)
                return;

            var allControls = FormStructureUtil.PreOrderFirstLevelTraversal(FormEntity);
            var dataGrids = allControls.OfType<GridEntity>().ToDictionary(n => n.ID);

            var sourceGrid = dataGrids[model.SourceGridID.Value];
            var targetGrid = dataGrids[model.TargetGridID.Value];

            var sourceCollection = MongoDbUtil.GetCollection(sourceGrid.ID);
            var sourceQuery = (from n in sourceCollection.AsQueryable()
                               where n[FormDataConstants.DateDeletedField] == (DateTime?)null
                               select n);

            var targetCollection = MongoDbUtil.GetCollection(targetGrid.ID);
            var targetQuery = (from n in targetCollection.AsQueryable()
                               where n[FormDataConstants.DateDeletedField] == (DateTime?)null
                               select n);

            var sourceDocs = sourceQuery.ToList();

            if (model.Mode == "Add" || model.Mode == "Overwrite")
            {
                if (model.Mode == "Overwrite")
                {
                    foreach (var targetDoc in targetQuery)
                    {
                        var recordID = targetDoc[FormDataConstants.IDField].AsNullableGuid;

                        var update = Builders<BsonDocument>.Update.Set(FormDataConstants.DateDeletedField, DateTime.Now);
                        var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);

                        targetCollection.UpdateMany(filter, update);
                    }
                }

                var targetDocs = sourceDocs.Select(BsonDocumentConverter.CreateClone);
                targetCollection.InsertMany(targetDocs);
            }
            else if (model.Mode == "Merge")
            {
                var targetDocs = targetQuery.ToList();

                var sourceFields = sourceDocs.SelectMany(n => n.Names);
                var targetFields = targetDocs.SelectMany(n => n.Names);

                var allFields = new SortedSet<String>();
                allFields.UnionWith(sourceFields);
                allFields.UnionWith(targetFields);

                allFields.ExceptWith(FormDataBase.DefaultFields);

                var hashCodes = BsonDocumentConverter.ComputeHashCodes(targetDocs, allFields).Select(n => n.Key).ToHashSet();

                foreach (var sourceDoc in sourceDocs)
                {
                    var hashCode = BsonDocumentConverter.ComputeHashCode(sourceDoc, allFields);
                    if (hashCodes.Add(hashCode))
                    {
                        var targetDoc = BsonDocumentConverter.CreateClone(sourceDoc);
                        targetCollection.InsertOne(targetDoc);
                    }
                }
            }

            mpeCloneDataGrid.Hide();
        }

        protected void btnCloneDataGridCancel_OnClick(object sender, EventArgs e)
        {
            mpeCloneDataGrid.Hide();
        }

        protected void btnChooseTemplateOK_OnClick(object sender, EventArgs e)
        {
            var model = chooseTemplateControl.Model;
            if (model.TemplateID == null)
                return;

            var url = new UrlHelper("~/Handlers/PrintFormData.ashx")
            {
                ["FormID"] = FormID,
                ["RecordID"] = model.RecordID,
                ["TemplateID"] = model.TemplateID,
                ["LoginToken"] = UmUtil.Instance.CurrentToken
            };

            Response.Redirect(url.ToEncodedUrl());
        }

        protected void btnChooseTemplateCancel_OnClick(object sender, EventArgs e)
        {
            mpeChooseTemplate.Hide();
        }

        protected void btnRequiresSaveOK_Click(object sender, EventArgs e)
        {
            var newFormData = formDataControl.GetFormData();

            var newRecordID = SaveFormData(newFormData, FormData);
            if (newRecordID != null)
            {
                var returnUrl = hdReturnUrl.Value;
                var redirectUrl = hdRedirectUrl.Value;

                if (!String.IsNullOrWhiteSpace(redirectUrl))
                {
                    var returnUrlHelper = new UrlHelper(returnUrl)
                    {
                        ["RecordID"] = newRecordID,
                    };

                    var redirectUrlHelper = new UrlHelper(redirectUrl)
                    {
                        ["ParentID"] = newRecordID,
                        ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrlHelper.ToString())
                    };

                    Response.Redirect(redirectUrlHelper.ToEncodedUrl());
                }
            }

            mpeRequiresSave.Hide();
        }

        protected void btnRequiresSaveCancel_Click(object sender, EventArgs e)
        {
            mpeRequiresSave.Hide();
        }

        protected void btnNotifyMessageOK_Click(object sender, EventArgs e)
        {

        }

        protected void btnNotifyMessageCancel_OnClick(object sender, EventArgs e)
        {
            //var returnUrl = Convert.ToString(RequestUrl["ReturnUrl"]);
            //if (!String.IsNullOrWhiteSpace(returnUrl))
            //{
            //    var cleanUrl = GmsCommonUtil.ConvertFromBase64(returnUrl);

            //    var returnUrlHelper = new UrlHelper(cleanUrl);
            //    Response.Redirect(returnUrlHelper.ToEncodedUrl());
            //}

            mpeNotifyMessage.Hide();
        }

        protected void formDataControl_OnCommand(object sender, CommandEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
                return;

            var commandName = e.CommandName;
            var commandArg = Convert.ToString(e.CommandArgument);

            if (!RegexUtil.CommandRx.IsMatch(commandArg))
                return;

            var commandMatch = RegexUtil.CommandRx.Match(commandArg);

            var ownerID = DataConverter.ToNullableGuid(commandMatch.Groups["ownerID"].Value);
            var recordID = DataConverter.ToNullableGuid(commandMatch.Groups["recordID"].Value);
            var containerID = DataConverter.ToNullableGuid(commandMatch.Groups["containerID"].Value);

            if (ownerID == null)
                return;

            if (commandName == "New")
            {
                commandName = "Edit";
                containerID = recordID;
                recordID = null;
            }

            if (commandName == "Delete")
            {
                if (recordID == null)
                    return;

                var collection = MongoDbUtil.GetCollection(ownerID);

                var update = Builders<BsonDocument>.Update.Set(FormDataConstants.DateDeletedField, DateTime.Now);
                var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);

                collection.UpdateMany(filter, update);

                FillFormData();

                return;
            }

            if (commandName == "Clone")
            {
                var allControls = FormStructureUtil.PreOrderFirstLevelTraversal(FormEntity);
                var dataGrids = allControls.OfType<GridEntity>().ToDictionary(n => n.ID);

                var dataGridClones = GetGridClones(ownerID.Value, dataGrids);

                var model = new CloneFormGridModel
                {
                    SourceGridID = ownerID,
                    TargetGridID = null,
                };

                cloneFormGridControl.BindDataGrids(dataGridClones);
                cloneFormGridControl.Model = model;

                mpeCloneDataGrid.Show();

                return;
            }

            var activeTabs = formDataControl.GetActiveTabs();

            var returnUrl = new UrlHelper(RequestUrl.ToString())
            {
                ["Tabs"] = String.Join(",", activeTabs)
            };

            var urlHelper = new UrlHelper(Request.Url)
            {
                ["Mode"] = commandName,
                ["FormID"] = FormID,
                ["UserID"] = UserID,
                ["OwnerID"] = ownerID,
                ["ParentID"] = RecordID,
                ["RecordID"] = recordID,
                ["StatusID"] = (StatusID ?? formDataControl.StatusID),
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl.ToString()),
                ["ContainerID"] = containerID,
            };

            if (FormData == null)
            {
                urlHelper.Remove("ReturnUrl");

                hdReturnUrl.Value = returnUrl.ToEncodedUrl();
                hdRedirectUrl.Value = urlHelper.ToEncodedUrl();

                mpeRequiresSave.Show();

                return;
            }

            var controls = FormStructureUtil.PreOrderFirstLevelTraversal(FormEntity);
            var fields = controls.Select(n => Convert.ToString(n.ID)).ToHashSet();

            var newFormData = formDataControl.GetFormData();

            if (!FormDataUtil.MergeAndEquals(newFormData, FormData, fields) ||
                FormDataUtil.IsGridOrTreeMissing(FormData, FormEntity))
            {
                urlHelper.Remove("ReturnUrl");

                hdReturnUrl.Value = returnUrl.ToEncodedUrl();
                hdRedirectUrl.Value = urlHelper.ToEncodedUrl();

                mpeRequiresSave.Show();

                return;
            }

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void formDataControl_OnError(object sender, EventArgs e)
        {
            var genericEventArgs = e as GenericEventArgs<String>;
            if (genericEventArgs == null)
                return;

            BindErrors(genericEventArgs.Value);
        }

        protected String GenIDNumber()
        {
            if (String.IsNullOrWhiteSpace(FormModel.Abbreviation) || String.IsNullOrWhiteSpace(FormModel.Year))
                return null;

            var count = MongoDbUtil.GetCount(OwnerID);

            var number = $"{FormModel.Abbreviation}-{FormModel.Year}-{count}";
            return number;
        }

        protected void FillFormData()
        {
            if (RecordID == null)
                return;

            formDataControl.BindFormData(FormData, IsPostBack);

            DetectChanges(FormData);
            SetStatusMode(FormData);
        }

        protected void FillTemplates()
        {
            chooseTemplateControl.BindTemplates(FormEntity);
        }

        protected bool IsFillingAvailable()
        {
            if (ParentID != null)
                return true;

            var expression = FormModel.FillingValidationExpression;
            if (String.IsNullOrWhiteSpace(expression))
                return true;

            var expNode = ExpressionParser.GetOrParse(expression);
            var expGlobals = new ExpressionGlobalsUtil(UserID, FormEntity, FormData);

            Object result;
            if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out result))
            {
                BindErrors("FillingValidationExpression Error");
                return true;
            }

            return DataConverter.ToBoolean(result);
        }

        protected bool IsDublicateFormData(String hashCode)
        {
            if (!String.IsNullOrEmpty(hashCode))
            {
                var collection = MongoDbUtil.GetCollection(OwnerID);

                var dataQuery = (from n in collection.AsQueryable()
                                 where n[FormDataConstants.DateDeletedField] == (DateTime?)null &&
                                       n[FormDataConstants.HashCodeField] == hashCode
                                 select n);

                if (ParentID != null)
                {
                    dataQuery = (from n in dataQuery
                                 where n[FormDataConstants.ParentIDField] == ParentID
                                 select n);
                }

                if (RecordID != null)
                {
                    dataQuery = (from n in dataQuery
                                 where n[FormDataConstants.IDField] != RecordID
                                 select n);
                }

                var list = dataQuery.ToList();

                if (list.Any())
                    return true;
            }

            return false;
        }

        protected bool ValidateFormData(FormDataUnit formData)
        {
            var entity = (ControlEntity)FormEntity;

            if (OwnerID != null && entity.ID != OwnerID && FormModel.ID != OwnerID)
            {
                var children = FormStructureUtil.PreOrderTraversal(entity);
                entity = children.FirstOrDefault(n => n.ID == OwnerID);
            }

            var container = (ContentEntity)entity;
            var formContainer = (ContentEntity)FormEntity;

            var errorMessages = new List<String>();

            var entities = (from n in FormStructureUtil.PreOrderFirstLevelTraversal(container)
                            where n is FieldEntity || n is GridEntity || n is TreeEntity
                            select n).ToList();

            var expGlobals = new ExpressionGlobalsUtil(UserID, formContainer, formData, ParentFormData);

            foreach (var control in entities)
            {
                if (control.ID == container.ID)
                    continue;

                var fieldKey = Convert.ToString(control.ID);

                expGlobals.SetAssociation("@", fieldKey);
                expGlobals.SetAssociation("@val", fieldKey);
                expGlobals.SetAssociation("@value", fieldKey);

                var mandatory = false;
                var errorMessage = String.Empty;
                var validationExp = String.Empty;

                var formField = control as FieldEntity;
                if (formField != null)
                {
                    mandatory = formField.Mandatory.GetValueOrDefault();
                    errorMessage = formField.ErrorMessage;
                    validationExp = formField.ValidationExp;
                }

                var formGrid = control as GridEntity;
                if (formGrid != null)
                {
                    errorMessage = formGrid.ErrorMessage;
                    validationExp = formGrid.ValidationExp;
                }

                var formTree = control as TreeEntity;
                if (formTree != null)
                {
                    errorMessage = formTree.ErrorMessage;
                    validationExp = formTree.ValidationExp;
                }

                if (mandatory)
                {
                    var strVal = Convert.ToString(expGlobals.Eval("@"));
                    if (String.IsNullOrWhiteSpace(strVal) || strVal == "Select an Option")
                    {
                        var message = $"[{control.Name}] - Is Mandatory";
                        errorMessages.Add(message);
                    }
                }

                var expression = validationExp;
                if (String.IsNullOrWhiteSpace(expression))
                    continue;

                var expNode = ExpressionParser.GetOrParse(expression);

                Object result;
                if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out result))
                {
                    var message = $"[{control.Name}] - Incorrect Validation Expression";
                    errorMessages.Add(message);
                }
                else
                {
                    var @bool = DataConverter.ToNullableBool(result);
                    if (@bool.GetValueOrDefault())
                    {
                        var message = $"[{control.Name}] - {errorMessage}";
                        errorMessages.Add(message);
                    }
                }
            }

            if (errorMessages.Count == 0)
                return true;

            BindErrors(errorMessages);
            return false;
        }

        protected void BindErrors(params String[] errors)
        {
            BindErrors((IEnumerable<String>)errors);
        }
        protected void BindErrors(IEnumerable<String> errors)
        {
            var query = (from n in errors
                         where !String.IsNullOrWhiteSpace(n)
                         select new
                         {
                             Item = n
                         });

            rptErrors.DataSource = query;
            rptErrors.DataBind();
        }

        protected void SetStatusMode(FormDataUnit formDataUnit)
        {
            //btnSave.Visible = false;
            //btnSubmit.Visible = false;
            //btnEdit.Visible = false;

            //if (Mode == FormMode.Edit ||
            //    Mode == FormMode.Review ||
            //    Mode == FormMode.Inspect)
            //{
            //    btnSave.Visible = true;
            //}

            //if (formDataUnit == null)
            //    return;

            //if (ParentID == null &&
            //    formDataUnit.StatusID != DataStatusCache.Submit.ID &&
            //    formDataUnit.StatusID != DataStatusCache.Accepted.ID)
            //{
            //    btnSubmit.Visible = true;
            //}

            //if (Mode != FormMode.Edit &&
            //    formDataUnit.StatusID == DataStatusCache.Accepted.ID)
            //{
            //    btnEdit.Visible = true;
            //}

            lblStatusDesc.Text = String.Empty;

            if (formDataUnit.StatusID == DataStatusCache.Submit.ID)
            {
                lblStatusDesc.ForeColor = Color.Green;
                lblStatusDesc.Text = "Your personal data waiting for progress";
            }

            if (formDataUnit.StatusID == DataStatusCache.InProgress.ID)
            {
                lblStatusDesc.ForeColor = Color.Green;
                lblStatusDesc.Text = "Your personal data is in progress";
            }

            if (formDataUnit.StatusID == DataStatusCache.Accepted.ID)
            {
                lblStatusDesc.ForeColor = Color.Green;
                lblStatusDesc.Text = "Your personal data approved";
            }

            if (formDataUnit.StatusID == DataStatusCache.Rejected.ID)
            {
                lblStatusDesc.ForeColor = Color.Red;
                lblStatusDesc.Text = formDataUnit.Description;
            }

            lblStatusDesc.Visible = !String.IsNullOrWhiteSpace(lblStatusDesc.Text);
        }

        protected void DetectChanges(FormDataUnit formDataUnit)
        {
            if (!UmUtil.Instance.HasAccess("Admin"))
                return;

            if (formDataUnit.StatusID != DataStatusCache.Submit.ID || formDataUnit.PreviousID == null)
                return;

            var oldFormData = formDataUnit;
            while (true)
            {
                if (oldFormData.PreviousID == null)
                    return;

                var previousDoc = MongoDbUtil.GetDocument(OwnerID, oldFormData.PreviousID);
                oldFormData = BsonDocumentConverter.ConvertToFormDataUnit(previousDoc);

                if (oldFormData == null)
                    return;

                if (oldFormData.StatusID == DataStatusCache.Submit.ID ||
                    oldFormData.StatusID == DataStatusCache.Accepted.ID ||
                    oldFormData.StatusID == DataStatusCache.Rejected.ID)
                    break;
            }

            var changedFields = new Dictionary<Guid, Object>();
            var changedGrids = new HashLookup<String, Guid?>();

            var allKeys = new HashSet<String>();
            allKeys.UnionWith(formDataUnit.Keys);
            allKeys.UnionWith(oldFormData.Keys);

            foreach (var key in allKeys)
            {
                var xVal = formDataUnit[key];
                var yVal = oldFormData[key];

                if (xVal is FormDataListRef || yVal is FormDataListRef)
                {
                    //var xRef = (FormDataListRef)xVal;
                    //var yRef = (FormDataListRef)yVal;

                    //var xList = new FormDataLazyList(xRef);
                    //var xDict = xList.ToDictionary(n => n.ID);

                    //var yList = new FormDataLazyList(yRef);

                    //var xIds = xList.Select(n => n.ID);
                    //var yIds = yList.Select(n => n.ID);

                    //var xs = yIds.ToHashSet();

                    //foreach (var recID in xIds)
                    //{

                    //}
                }
                else
                {
                    if (!FormDataUtil.FormItemEquals(xVal, yVal))
                    {
                        var fieldID = DataConverter.ToNullableGuid(key);
                        if (fieldID != null)
                            changedFields.Add(fieldID.Value, yVal);
                    }
                }
            }

            formDataControl.MarkChangedFields(changedFields);
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

            var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
            return formData;
        }

        protected Guid? SaveFormData(FormDataUnit newFormData, FormDataUnit oldFormData)
        {
            var oldRecordID = RecordID;
            var newRecordID = Guid.NewGuid();
            var currentDate = DateTime.Now;

            var container = (ContentEntity)FormEntity;
            if (OwnerID != null && OwnerID != FormID)
            {
                var subControls = FormStructureUtil.PreOrderTraversal(container);
                container = subControls.OfType<ContentEntity>().First(n => n.ID == OwnerID);
            }

            var controls = FormStructureUtil.PreOrderFirstLevelTraversal(container).ToList();

            newFormData.ID = newRecordID;
            newFormData.FormID = FormID;
            newFormData.OwnerID = OwnerID;
            newFormData.ParentID = ParentID;
            newFormData.IDNumber = GenIDNumber();
            newFormData.ContainerID = ContainerID;
            newFormData.DateCreated = currentDate;
            newFormData.UserID = UserUtil.GetCurrentUserID();
            newFormData.StatusID = DataStatusCache.None.ID;
            newFormData.HashCode = FormStructureUtil.ComputeHashCode(controls, newFormData);

            if (ParentID != null)
            {
                if (!ValidateFormData(newFormData))
                    return null;
            }

            FormDataUtil.MergeFormDatas(newFormData, oldFormData);
            newFormData.DateDeleted = null;

            var treeAndGrids = (from n in controls
                                where n is GridEntity || n is TreeEntity
                                select n);

            foreach (var entity in treeAndGrids)
            {
                var fieldKey = Convert.ToString(entity.ID);

                var listRef = newFormData[fieldKey] as FormDataListRef;
                if (listRef == null)
                {
                    listRef = new FormDataListRef(FormID, entity.ID, ParentID);
                    newFormData[fieldKey] = listRef;
                }
            }

            if (IsDublicateFormData(newFormData.HashCode))
            {
                BindErrors("Dublicate record");
                return null;
            }

            var mainCollection = MongoDbUtil.GetCollection(OwnerID);
            var historyCollection = MongoDbUtil.GetCollection(OwnerID);

            if (oldFormData != null)
            {
                newFormData.PreviousID = oldRecordID;
                newFormData.StatusID = oldFormData.StatusID.GetValueOrDefault(DataStatusCache.None.ID);
                newFormData.HashCode = FormStructureUtil.ComputeHashCode(controls, newFormData);

                if (!String.IsNullOrWhiteSpace(oldFormData.IDNumber))
                    newFormData.IDNumber = oldFormData.IDNumber;

                if (UserUtil.IsSuperAdmin())
                    newFormData.UserID = oldFormData.UserID;
                else if (oldFormData.StatusID == DataStatusCache.Accepted.ID)
                    newFormData.StatusID = DataStatusCache.None.ID;

                var mainUpdate = Builders<BsonDocument>.Update.Set(FormDataConstants.DateDeletedField, currentDate);
                var mainFilter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, oldRecordID);

                mainCollection.UpdateMany(mainFilter, mainUpdate);

                var historyUpdate = Builders<BsonDocument>.Update.Set(FormDataConstants.ParentIDField, newRecordID);
                var historyFilter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.ParentIDField, oldRecordID);

                historyCollection.UpdateMany(historyFilter, historyUpdate);

                var listRefs = newFormData.Values.OfType<FormDataListRef>();
                foreach (var listRef in listRefs)
                {
                    var subCollection = MongoDbUtil.GetCollection(listRef.OwnerID);

                    var subUpdate = Builders<BsonDocument>.Update.Set(FormDataConstants.ParentIDField, newRecordID);
                    var subFilter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.ParentIDField, oldRecordID);

                    subCollection.UpdateMany(subFilter, subUpdate);
                }
            }

            var document = BsonDocumentConverter.ConvertToBsonDocument(newFormData);

            mainCollection.InsertOne(document);

            return newRecordID;
        }

        protected IEnumerable<GridEntity> GetGridClones(Guid dataGridID, IDictionary<Guid, GridEntity> dataGrids)
        {
            var primaryGrid = dataGrids[dataGridID];
            var primaryGridFields = primaryGrid.Controls.Select(n => n.Name).ToHashSet();

            foreach (var otherGrid in dataGrids.Values)
            {
                if (otherGrid.ID == primaryGrid.ID)
                    continue;

                var otherGridFields = otherGrid.Controls.Select(n => n.Name).ToHashSet();

                if (primaryGridFields.SetEquals(otherGridFields))
                    yield return otherGrid;
            }
        }

        private void CheckFormAccesibility()
        {
            //var canAccessForm = IsFillingAvailable();
            //if (!canAccessForm)
            //{
            //    lblStatusDesc.Text = FormModel.FillingValidationMessage;
            //    lblStatusDesc.Visible = !String.IsNullOrWhiteSpace(lblStatusDesc.Text);

            //    btnSave.Visible = false;
            //    btnSubmit.Visible = false;
            //    btnEdit.Visible = false;
            //    btnFiles.Visible = false;

            //    formDataControl.Visible = false;
            //}
        }

        private bool GetSaveSubmitAccesibility()
        {
            if (!UmUtil.Instance.CurrentUser.IsSuperAdmin && !string.IsNullOrWhiteSpace(FormEntity.VisibleExpression))
            {
                var expNode = ExpressionParser.GetOrParse(FormEntity.VisibleExpression);
                var expGlobals = new ExpressionGlobalsUtil(UserID, FormEntity, FormData);

                Object eval;
                if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out eval))
                    return false;

                var result = DataConverter.ToNullableBoolean(eval).GetValueOrDefault();
                if (result)
                    return true;

                if (RecordID == null)
                    return false;

                if (FormData.StatusID != DataStatusCache.None.ID)
                    return true;

                if (FormData.PreviousID == null)
                    return false;

                foreach (var formData in FormDataDbUtil.GetPreviousFormDatas(FormData))
                {
                    if (formData.StatusID != DataStatusCache.None.ID)
                        return true;
                }
            }
            return true;
        }

    }
}