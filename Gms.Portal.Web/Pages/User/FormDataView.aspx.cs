using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using DevExpress.XtraPrinting.Native;
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

            var dbForm = HbSession.Get<GM_Form>(FormID);
            if (dbForm == null)
                return;

            formDataControl.FormID = FormID;
            formDataControl.OwnerID = OwnerID;
            formDataControl.RecordID = RecordID;
            formDataControl.ParentID = ParentID;

            var model = ModelConverter.Convert(dbForm);

            var formEntity = model.Entity;
            if (formEntity == null)
                return;

            btnSave.Visible = Enabled;
            btnSave.CssClass = (Enabled ? "btn btn-success fa fa-save" : "btn btn-default fa fa-save");
            formDataControl.Enabled = Enabled;

            if (OwnerID != null && OwnerID != FormID && formEntity.Controls != null)
            {
                var allControls = FormStructureUtil.PreOrderTraversal(formEntity);
                var control = allControls.FirstOrDefault(n => n.ID == OwnerID);

                var gridEntity = control as GridEntity;
                if (gridEntity != null)
                    formDataControl.InitStructure(gridEntity);
            }
            else
            {
                formDataControl.InitStructure(formEntity);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillFormData();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var newFormDataUnit = formDataControl.GetFormData();
            newFormDataUnit.ID = RecordID;
            newFormDataUnit.FormID = FormID;
            newFormDataUnit.OwnerID = OwnerID;
            newFormDataUnit.ParentID = ParentID;
            newFormDataUnit.UserID = UserUtil.GetCurrentUserID();

            if (!ValidateFormData(newFormDataUnit))
                return;

            var oldRecordID = newFormDataUnit.ID;
            var newRecordID = Guid.NewGuid();

            var collection = MongoDbUtil.GetCollection(OwnerID);

            if (newFormDataUnit.ID != null)
            {
                var oldFormDataUnit = LoadFormDataUnit(OwnerID, oldRecordID);
                if (oldFormDataUnit != null)
                {
                    var update = Builders<BsonDocument>.Update.Set("DateDeleted", DateTime.Now);
                    var filter = Builders<BsonDocument>.Filter.Eq("ID", oldRecordID);

                    collection.UpdateMany(filter, update);

                    newFormDataUnit.PreviousID = oldRecordID;

                    if (UserUtil.IsSuperAdmin())
                        newFormDataUnit.UserID = oldFormDataUnit.UserID;

                    var listRefs = (from n in newFormDataUnit
                                    let l = n.Value as FormDataListRef
                                    where l != null
                                    select l);

                    foreach (var listRef in listRefs)
                    {
                        var subCollection = MongoDbUtil.GetCollection(listRef.OwnerID);

                        var subUpdate = Builders<BsonDocument>.Update.Set(FormDataUnit.ParentIDField, newRecordID);
                        var subFilter = Builders<BsonDocument>.Filter.Eq(FormDataUnit.ParentIDField, oldRecordID);

                        subCollection.UpdateMany(subFilter, subUpdate);
                    }
                }
            }

            newFormDataUnit.ID = newRecordID;
            newFormDataUnit.DateCreated = DateTime.Now;

            var document = BsonDocumentConverter.ConvertToBsonDocument(newFormDataUnit);

            collection.InsertOne(document);

            if (RecordID == null)
            {
                var urlHelper = new UrlHelper(RequestUrl);
                urlHelper["FormID"] = FormID;
                urlHelper["OwnerID"] = OwnerID;
                urlHelper["RecordID"] = newRecordID;
                urlHelper["ParentID"] = ParentID;

                Response.Redirect(urlHelper.ToEncodedUrl());
            }
            else
            {
                var returnUrl = Convert.ToString(RequestUrl["ReturnUrl"]);
                if (!String.IsNullOrWhiteSpace(returnUrl))
                {
                    var returnUrlHelper = new UrlHelper(returnUrl);
                    Response.Redirect(returnUrlHelper.ToEncodedUrl());
                }
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            var returnUrl = Convert.ToString(RequestUrl["ReturnUrl"]);
            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                var returnUrlHelper = new UrlHelper(returnUrl);
                Response.Redirect(returnUrlHelper.ToEncodedUrl());
            }
        }

        protected void formDataControl_OnCommand(object sender, CommandEventArgs e)
        {
            var commandArg = Convert.ToString(e.CommandArgument);

            var regex = new Regex(@"(?<ownerID>.*)/(?<recordID>.*)", RegexOptions.Compiled);
            if (!regex.IsMatch(commandArg))
                return;

            var match = regex.Match(commandArg);

            var ownerID = DataConverter.ToNullableGuid(match.Groups["ownerID"].Value);
            var recordID = DataConverter.ToNullableGuid(match.Groups["recordID"].Value);

            if (ownerID == null)
                return;

            if (e.CommandName == "Delete")
            {
                if (recordID == null)
                    return;

                var collection = MongoDbUtil.GetCollection(ownerID);

                var update = Builders<BsonDocument>.Update.Set(FormDataUnit.DateDeletedField, DateTime.Now);
                var filter = Builders<BsonDocument>.Filter.Eq(FormDataUnit.IDField, recordID);

                collection.UpdateMany(filter, update);

                FillFormData();

                return;
            }

            var urlHelper = new UrlHelper(Request.Url);
            urlHelper["Mode"] = e.CommandName;
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = ownerID;
            urlHelper["ParentID"] = RecordID;
            urlHelper["RecordID"] = recordID;
            urlHelper["ReturnUrl"] = Convert.ToString(Request.Url);

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected FormDataUnit LoadFormDataUnit(Guid? ownerID, Guid? recordID)
        {
            if (ownerID == null || recordID == null)
                return null;

            var collection = MongoDbUtil.GetCollection(ownerID);

            //var commonFilter = Builders<BsonDocument>.Filter.Eq("ID", recordID);
            //var document = documents.FirstOrDefault();

            var document = collection.AsQueryable().FirstOrDefault(n => n[FormDataUnit.IDField] == recordID);
            if (document == null)
                return null;

            return BsonDocumentConverter.ConvertToFormDataUnit(document);
        }

        protected bool ValidateFormData(FormDataUnit formDataUnit)
        {
            var dbForm = HbSession.Get<GM_Form>(FormID);
            var formModel = ModelConverter.Convert(dbForm);

            var entity = (ControlEntity)formModel.Entity;
            if (OwnerID != null && entity.ID != OwnerID)
            {
                var children = FormStructureUtil.PreOrderTraversal(entity);
                entity = children.FirstOrDefault(n => n.ID == OwnerID);
            }

            var container = (ContentEntity)entity;

            var entities = FormStructureUtil.PreOrderTraversal(container);
            var fields = entities.OfType<FieldEntity>();

            var validateFields = new List<FieldEntity>();

            var valuesDict = new Dictionary<String, Object>();
            foreach (var field in fields)
            {
                var key = Convert.ToString(field.ID);

                valuesDict[field.Name] = formDataUnit[key];
                validateFields.Add(field);
            }

            var errorMessages = new List<String>();

            foreach (var field in validateFields)
            {
                var key = Convert.ToString(field.ID);
                var expression = field.ValidationExp;

                valuesDict["@"] = formDataUnit[key];

                var result = ExpressionEvaluator.Eval(expression, valuesDict.GetValueOrDefault) as bool?;
                if (result.GetValueOrDefault())
                {
                    var message = String.Format("[{0}] - {1}", field.Name, field.ErrorMessage);
                    errorMessages.Add(message);
                }
            }

            if (errorMessages.Count == 0)
                return true;

            lblErrorMessage.Text = String.Empty;
            var text = String.Join("<br />", errorMessages);

            lblErrorMessage.Text = text;

            return false;
        }

        protected void FillFormData()
        {
            if (RecordID == null)
                return;

            var formDataUnit = LoadFormDataUnit(OwnerID, RecordID);

            formDataControl.BindFormData(formDataUnit);
            formDataControl.DataBind();
        }
    }
}