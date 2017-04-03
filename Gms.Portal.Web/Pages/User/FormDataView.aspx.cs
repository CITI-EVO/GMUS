using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Collections;
using CITI.EVO.Tools.Comparers;
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
using Gms.Portal.Web.Models;
using NHibernate.Linq;
using CommonUtil = Gms.Portal.Web.Utils.CommonUtil;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FormDataView : BasePage
    {
        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
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

        public bool Enabled
        {
            get { return (Convert.ToString(RequestUrl["Mode"]) != "View"); }
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

        private FormDataUnit _formDataUnit;
        protected FormDataUnit FormDataUnit
        {
            get
            {
                if (RecordID == null)
                    return null;

                if (_formDataUnit == null)
                    _formDataUnit = LoadFormDataUnit(OwnerID, RecordID);

                return _formDataUnit;
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

            formDataControl.FormID = FormID;
            formDataControl.OwnerID = OwnerID;
            formDataControl.RecordID = RecordID;
            formDataControl.ParentID = ParentID;

            btnSave.Visible = Enabled;
            btnSave.CssClass = (Enabled ? "btn btn-success fa fa-save" : "btn btn-default fa fa-save");
            formDataControl.Enabled = Enabled;

            if (OwnerID != null && OwnerID != FormID && FormEntity.Controls != null)
            {
                var allControls = FormStructureUtil.PreOrderTraversal(FormEntity);
                var control = allControls.FirstOrDefault(n => n.ID == OwnerID);

                var gridEntity = control as GridEntity;
                if (gridEntity != null)
                    formDataControl.InitStructure(gridEntity);
            }
            else
            {
                formDataControl.InitStructure(FormEntity);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyViewMode();
            FillFormData();

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
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var oldRecordID = RecordID;
            var newRecordID = Guid.NewGuid();
            var currentDate = DateTime.Now;
            var newFormDataUnit = formDataControl.GetFormData();

            if (ParentID != null)
            {
                if (!ValidateFormData(newFormDataUnit))
                    return;
            }

            newFormDataUnit.ID = newRecordID;
            newFormDataUnit.FormID = FormID;
            newFormDataUnit.OwnerID = OwnerID;
            newFormDataUnit.ParentID = ParentID;
            newFormDataUnit.DateCreated = currentDate;
            newFormDataUnit.UserID = UserUtil.GetCurrentUserID();
            newFormDataUnit.StatusID = DataStatusCache.None.ID;

            var collection = MongoDbUtil.GetCollection(OwnerID);

            if (FormDataUnit != null)
            {
                newFormDataUnit.PreviousID = oldRecordID;

                if (UserUtil.IsSuperAdmin())
                    newFormDataUnit.UserID = FormDataUnit.UserID;

                var update = Builders<BsonDocument>.Update.Set(FormDataConstants.DateDeletedField, currentDate);
                var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, oldRecordID);

                collection.UpdateMany(filter, update);

                var listRefs = (from n in newFormDataUnit
                                let m = n.Value as FormDataListRef
                                where m != null
                                select m);

                foreach (var listRef in listRefs)
                {
                    var subCollection = MongoDbUtil.GetCollection(listRef.OwnerID);

                    var subUpdate = Builders<BsonDocument>.Update.Set(FormDataConstants.ParentIDField, newRecordID);
                    var subFilter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.ParentIDField, oldRecordID);

                    subCollection.UpdateMany(subFilter, subUpdate);
                }
            }

            var controls = FormStructureUtil.PreOrderFirstLevelTraversal(FormEntity);
            var formGrids = controls.OfType<GridEntity>();

            foreach (var entity in formGrids)
            {
                var key = Convert.ToString(entity.ID);

                if ((newFormDataUnit[key] as FormDataListRef) == null)
                    newFormDataUnit[key] = new FormDataListRef(FormID, entity.ID, ParentID);
            }

            var document = BsonDocumentConverter.ConvertToBsonDocument(newFormDataUnit);

            collection.InsertOne(document);

            //if (RecordID == null && ParentID == null)
            //{
            //    var urlHelper = new UrlHelper(RequestUrl);
            //    urlHelper["FormID"] = FormID;
            //    urlHelper["OwnerID"] = OwnerID;
            //    urlHelper["RecordID"] = newRecordID;
            //    urlHelper["ParentID"] = ParentID;

            //    Response.Redirect(urlHelper.ToEncodedUrl());
            //}

            var returnUrl = Convert.ToString(RequestUrl["ReturnUrl"]);
            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                var cleanUrl = CommonUtil.ConvertFromBase64(returnUrl);

                var returnUrlHelper = new UrlHelper(cleanUrl);
                Response.Redirect(returnUrlHelper.ToEncodedUrl());
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            var returnUrl = Convert.ToString(RequestUrl["ReturnUrl"]);
            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                var cleanUrl = CommonUtil.ConvertFromBase64(returnUrl);

                var returnUrlHelper = new UrlHelper(cleanUrl);
                Response.Redirect(returnUrlHelper.ToEncodedUrl());
            }
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (RecordID == null)
                return;

            var formDataUnit = LoadFormDataUnit(OwnerID, RecordID);
            if (!ValidateFormData(formDataUnit))
                return;

            var ownerID = OwnerID.GetValueOrDefault();
            var recordID = RecordID.GetValueOrDefault();
            var submitStatus = DataStatusCache.Submit;

            FormDataDbUtil.ChangeStatus(ownerID, recordID, submitStatus.ID, String.Empty);

            Response.Redirect(RequestUrl.ToEncodedUrl());
        }

        protected void formDataControl_OnCommand(object sender, CommandEventArgs e)
        {
            var control = sender as Control;
            if (control == null)
                return;

            var commandName = e.CommandName;
            var commandArg = Convert.ToString(e.CommandArgument);

            var commandRx = new Regex(@"(?<ownerID>.*)/(?<recordID>.*)", RegexOptions.Compiled);

            if (!commandRx.IsMatch(commandArg))
                return;

            var commandMatch = commandRx.Match(commandArg);

            var ownerID = DataConverter.ToNullableGuid(commandMatch.Groups["ownerID"].Value);
            var recordID = DataConverter.ToNullableGuid(commandMatch.Groups["recordID"].Value);

            if (ownerID == null)
                return;

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

            var activeTabs = formDataControl.GetActiveTabs();

            var returnUrl = new UrlHelper(RequestUrl.ToString());
            returnUrl["Tabs"] = String.Join(",", activeTabs);

            var urlHelper = new UrlHelper(Request.Url);
            urlHelper["Mode"] = commandName;
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = ownerID;
            urlHelper["ParentID"] = RecordID;
            urlHelper["RecordID"] = recordID;
            urlHelper["ReturnUrl"] = CommonUtil.ConvertToBase64(returnUrl.ToString());

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void ApplyViewMode()
        {
            btnSubmit.Visible = (UmUtil.Instance.HasAccess("Submit") && RecordID != null && ParentID == null);
        }

        protected void FillFormData()
        {
            if (RecordID == null)
                return;

            var formDataUnit = LoadFormDataUnit(OwnerID, RecordID);
            formDataControl.BindFormData(formDataUnit, IsPostBack);

            DetectChanges(formDataUnit);
            SetStatusMode(formDataUnit);
        }

        protected FormDataUnit LoadFormDataUnit(Guid? ownerID, Guid? recordID)
        {
            if (ownerID == null || recordID == null)
                return null;

            var collection = MongoDbUtil.GetCollection(ownerID);

            //var commonFilter = Builders<BsonDocument>.Filter.Eq("ID", recordID);
            //var document = documents.FirstOrDefault();

            var document = collection.AsQueryable().FirstOrDefault(n => n[FormDataConstants.IDField] == recordID);
            if (document == null)
                return null;

            return BsonDocumentConverter.ConvertToFormDataUnit(document);
        }

        protected bool ValidateFormData(FormDataUnit formDataUnit)
        {
            var entity = (ControlEntity)FormEntity;

            if (OwnerID != null && entity.ID != OwnerID && FormModel.ID != OwnerID)
            {
                var children = FormStructureUtil.PreOrderTraversal(entity);
                entity = children.FirstOrDefault(n => n.ID == OwnerID);
            }

            var container = (ContentEntity)entity;

            var entities = (from n in FormStructureUtil.PreOrderFirstLevelTraversal(container)
                            where n is FieldEntity || n is GridEntity
                            select n);

            var validateFields = new List<ControlEntity>();

            var valuesDict = new Dictionary<String, Object>();
            foreach (var field in entities)
            {
                var key = Convert.ToString(field.ID);

                var val = formDataUnit[key];
                if (val is FormDataBinary)
                {
                    var binary = (FormDataBinary)val;
                    val = binary.FileName;
                }
                else if (val is FormDataListRef)
                {
                    var formListRef = (FormDataListRef)formDataUnit[key];
                    var formDataList = new FormDataLazyList(formListRef);

                    val = formDataList.Count;
                }

                valuesDict[field.Name] = val;

                validateFields.Add(field);
            }

            var errorMessages = new List<String>();

            foreach (var control in validateFields)
            {
                var key = Convert.ToString(control.ID);
                var val = formDataUnit[key];

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

                if (mandatory)
                {
                    var strVal = Convert.ToString(val);
                    if (String.IsNullOrWhiteSpace(strVal) || strVal == "Select an Option")
                    {
                        var message = String.Format("[{0}] - Is Mandatory", control.Name);
                        errorMessages.Add(message);
                    }
                }

                var expression = validationExp;
                if (String.IsNullOrWhiteSpace(expression))
                    continue;

                valuesDict["@"] = val;
                valuesDict["@val"] = val;
                valuesDict["@value"] = val;

                var result = ExpressionEvaluator.Eval(expression, valuesDict.GetValueOrDefault);

                var @bool = DataConverter.ToNullableBool(result);
                if (@bool.GetValueOrDefault())
                {
                    var message = String.Format("[{0}] - {1}", control.Name, errorMessage);
                    errorMessages.Add(message);
                }
            }

            if (errorMessages.Count == 0)
                return true;

            var query = (from n in errorMessages
                         where !String.IsNullOrWhiteSpace(n)
                         select new
                         {
                             Item = n
                         });

            rptErrors.DataSource = query;
            rptErrors.DataBind();

            return false;
        }

        protected void SetStatusMode(FormDataUnit formDataUnit)
        {
            if (formDataUnit == null)
                return;

            if (formDataUnit.StatusID == DataStatusCache.Submit.ID ||
                formDataUnit.StatusID == DataStatusCache.Accepted.ID)
            {
                btnSubmit.Enabled = false;
                btnSubmit.CssClass = "btn btn-default fa fa-share-square-o";
            }
            else
            {
                btnSubmit.CssClass = "btn btn-primary fa fa-share-square-o";
            }

            lblStatusDesc.Text = String.Empty;

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
            if (formDataUnit.StatusID == DataStatusCache.Submit.ID || formDataUnit.PreviousID == null)
                return;

            var currentFormData = formDataUnit;
            while (true)
            {
                var previousDoc = MongoDbUtil.GetDocument(OwnerID, currentFormData.PreviousID);
                currentFormData = BsonDocumentConverter.ConvertToFormDataUnit(previousDoc);

                if (currentFormData == null)
                    return;

                if (currentFormData.StatusID == DataStatusCache.Submit.ID)
                    break;
            }

            var changedFields = new HashSet<String>();
            var changedGrids = new HashLookup<String, Guid?>();

            var allKeys = new HashSet<String>();
            allKeys.UnionWith(formDataUnit.Keys);
            allKeys.UnionWith(currentFormData.Keys);

            foreach (var key in allKeys)
            {
                var xVal = formDataUnit[key];
                var yVal = currentFormData[key];

                if (xVal is FormDataListRef || yVal is FormDataListRef)
                {
                    var xRef = (FormDataListRef)xVal;
                    var yRef = (FormDataListRef)yVal;

                    var xList = new FormDataLazyList(xRef);
                    var xDict = xList.ToDictionary(n => n.ID);

                    var yList = new FormDataLazyList(yRef);

                    var xIds = xList.Select(n => n.ID);
                    var yIds = yList.Select(n => n.ID);

                    var xs = yIds.ToHashSet();

                    foreach (var recID in xIds)
                    {

                    }
                }
                else
                {
                    if (!FormItemEquals(xVal, yVal))
                        changedFields.Add(key);
                }
            }
        }

        protected bool FormItemEquals(Object xVal, Object yVal)
        {
            return FormItemCompare(xVal, yVal) == 0;
        }
        protected int FormItemCompare(Object xVal, Object yVal)
        {
            if (ReferenceEquals(xVal, yVal))
                return 0;

            if (Equals(xVal, yVal))
                return 0;

            if (xVal == null && yVal == null)
                return 0;

            if (xVal != null && yVal == null)
                return 1;

            if (xVal == null && yVal != null)
                return -1;

            var ordinalComparer = StringComparer.OrdinalIgnoreCase;
            var bytesComparer = new ByteArrayComparer();

            if (xVal is FormDataListRef)
                return 1;

            if (yVal is FormDataListRef)
                return -1;

            if (xVal is FormDataBinary && yVal is FormDataBinary)
            {
                var xBin = (FormDataBinary)xVal;
                var yBin = (FormDataBinary)yVal;

                var order = ordinalComparer.Compare(xBin.FileName, yBin.FileName);
                if (order == 0)
                    order = bytesComparer.Compare(xBin.FileBytes, yBin.FileBytes);

                return order;
            }

            if (xVal is FormDataBinary)
                return 1;
            if ((yVal is FormDataBinary))
                return -1;

            var xs = Convert.ToString(xVal);
            var ys = Convert.ToString(yVal);

            var result = ordinalComparer.Compare(xs, ys);
            return result;
        }
    }
}