using CITI.EVO.Tools.Collections;
using CITI.EVO.Tools.EventArguments;
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
using MongoDB.Bson;
using MongoDB.Driver;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FormDataGrid : BasePage
    {
        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
        }

        public Guid? OwnerID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["OwnerID"]); }
        }

        public Guid? ParentID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["ParentID"]); }
        }

        public Guid? UserID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["UserID"]); }
        }

        private GM_Form _dbForm;
        protected GM_Form DbForm
        {
            get
            {
                if (FormID != null)
                {
                    if (_dbForm == null)
                        _dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == FormID);
                }

                return _dbForm;
            }
        }

        private FormEntity _formEntity;
        protected FormEntity FormEntity
        {
            get
            {
                if (_formEntity == null)
                {
                    if (DbForm == null)
                        return null;

                    var converter = new FormEntityModelConverter(HbSession);
                    var model = converter.Convert(DbForm);

                    _formEntity = model.Entity;
                }

                return _formEntity;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            formGridFilterControl.InitStructure(FormEntity);
            formDataGridControl.InitStructure(DbForm);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UmUtil.Instance.IsLogged)
                return;

            CheckUserMode();
            FillGridView();
            FillTemplates();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = Convert.ToString(FormMode.Edit),
                ["FormID"] = FormID,
                ["OwnerID"] = (OwnerID ?? FormID),
                ["RecordID"] = null,
                ["ParentID"] = ParentID,
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {

        }

        protected void formDataGridControl_OnPrint(object sender, GenericEventArgs<Guid> e)
        {
            var model = new ChooseTemplateModel
            {
                RecordID = e.Value,
            };

            chooseTemplateControl.Model = model;
            mpeChooseTemplate.Show();
        }

        protected void formDataGridControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var collection = MongoDbUtil.GetCollection(OwnerID);

            var filter = Builders<BsonDocument>.Filter.Eq("ID", e.Value);
            var update = Builders<BsonDocument>.Update.Set("DateDeleted", DateTime.Now);

            collection.UpdateMany(filter, update);

            FillGridView();
        }

        protected void formDataGridControl_OnStatus(object sender, GenericEventArgs<Guid> e)
        {
            var ownerID = (OwnerID ?? FormID);

            var document = MongoDbUtil.GetDocument(ownerID, e.Value);

            var formDataUnit = BsonDocumentConverter.ConvertToFormDataUnit(document);
            if (formDataUnit == null)
                return;

            var model = new RecordStatusModel
            {
                RecordID = formDataUnit.ID,
                StatusID = formDataUnit.StatusID
            };

            recordStatusControl.Model = model;
            mpeRecordStatus.Show();
        }

        protected void formDataGridControl_OnReview(object sender, GenericEventArgs<Guid> e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = Convert.ToString(FormMode.Review),
                ["FormID"] = FormID,
                ["OwnerID"] = (OwnerID ?? FormID),
                ["RecordID"] = e.Value,
                ["ParentID"] = ParentID,
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void formDataGridControl_OnInspect(object sender, GenericEventArgs<Guid> e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = "Inspect",
                ["FormID"] = FormID,
                ["OwnerID"] = (OwnerID ?? FormID),
                ["RecordID"] = e.Value,
                ["ParentID"] = ParentID,
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void formDataGridControl_OnAssigne(object sender, GenericEventArgs<Guid> e)
        {
            var recordID = e.Value;
            var ownerID = (OwnerID ?? FormID);

            var userID = UserID;
            if (UserID == null)
            {
                var document = MongoDbUtil.GetDocument(ownerID, recordID);
                if (document == null)
                    return;

                var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
                userID = formData.UserID;
            }

            var url = new UrlHelper("~/Pages/User/AssigneExperts.aspx");
            url["FormID"] = FormID;
            url["RecordID"] = recordID;
            url["UserID"] = userID;

            Response.Redirect(url.ToEncodedUrl());
        }

        protected void recordStatusControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeRecordStatus.Show();
        }

        protected void btnRecordStatusOK_OnClick(object sender, EventArgs e)
        {
            var model = recordStatusControl.Model;
            if (model.StatusID == DataStatusCache.Accepted.ID)
            {
                mpeRecordStatus.Hide();
                mpeConfirmAccept.Show();
                return;
            }

            ChangeStatus();
            FillGridView();

            mpeRecordStatus.Hide();
        }

        protected void btnConfirmAcceptOK_OnClick(object sender, EventArgs e)
        {
            ChangeStatus();

            mpeConfirmAccept.Hide();
            mpeRecordStatus.Hide();
        }

        protected void btnChooseTemplateOK_OnClick(object sender, EventArgs e)
        {
            var model = chooseTemplateControl.Model;
            if (model.TemplateID == null)
                return;

            var url = new UrlHelper("~/Handlers/PrintFormData.ashx");
            url["FormID"] = FormID;
            url["RecordID"] = model.RecordID;
            url["TemplateID"] = model.TemplateID;
            url["LoginToken"] = UmUtil.Instance.CurrentToken;

            Response.Redirect(url.ToEncodedUrl());
        }

        protected void btnChooseTemplateCancel_OnClick(object sender, EventArgs e)
        {
            mpeChooseTemplate.Hide();
        }

        protected void btnRecordStatusCancel_OnClick(object sender, EventArgs e)
        {
            mpeRecordStatus.Hide();
        }

        protected void btnConfirmAcceptCancel_OnClick(object sender, EventArgs e)
        {
            mpeConfirmAccept.Hide();
        }

        protected void FillTemplates()
        {
            chooseTemplateControl.BindTemplates(FormEntity);
        }

        protected void ChangeStatus()
        {
            var model = recordStatusControl.Model;
            var ownerID = (OwnerID ?? FormID);

            var document = MongoDbUtil.GetDocument(ownerID, model.RecordID);

            if (UserUtil.IsSuperAdmin())
            {
                var formDataUnit = BsonDocumentConverter.ConvertToFormDataUnit(document);
                if (formDataUnit == null)
                    return;

                FormDataDbUtil.ChangeStatus(ownerID.GetValueOrDefault(), model);
            }
            else
            {
                BsonValue bsonValue;
                if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                    return;

                var oldArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
                if (oldArray == null)
                    return;

                var formStatusUnits = BsonDocumentConverter.ConvertToFormStatuses(oldArray).ToList();
                var dictionary = formStatusUnits.ToDictionary(n => n.UserID);

                var userID = UserUtil.GetCurrentUserID();
                if (userID == null)
                    throw new Exception();

                var formStatus = dictionary[userID];
                formStatus.StatusID = model.StatusID;
                formStatus.DateOfStatus = DateTime.Now;

                var formStatusDocs = BsonDocumentConverter.ConvertToFormStatuses(formStatusUnits);
                var newArray = new BsonArray(formStatusDocs);

                document[FormDataConstants.UserStatusesFields] = newArray;

                MongoDbUtil.UpdateDocument(ownerID, document);
            }

            FillGridView();
        }

        protected void FillGridView()
        {
            if (FormID == null)
                return;

            if (FormEntity == null)
                return;

            var controls = FormStructureUtil.InOrderFirstLevelTraversal(FormEntity);

            var list = (from n in controls.OfType<FieldEntity>()
                        where n.Visible &&
                              n.DisplayOnGrid == "Always"
                        select n).ToList();

            var formFields = list.Select(n => Convert.ToString(n.ID)).Union(FormDataBase.DefaultFields);
            var fieldSet = formFields.ToHashSet();

            var collectionID = (OwnerID ?? FormID);

            var filters = formGridFilterControl.GetData();

            var emptys = (from n in filters
                          where String.IsNullOrWhiteSpace(Convert.ToString(n.Value))
                          select n.Key).ToHashSet();

            foreach (var key in emptys)
                filters.Remove(key);

            filters[FormDataConstants.DateDeletedField] = null;

            if (ParentID != null)
                filters[FormDataConstants.ParentIDField] = ParentID;

            if (!UserUtil.IsSuperAdmin())
            {
                var userID = UserID;
                if (!UmUtil.Instance.HasAccess("Admin") && !UmUtil.Instance.HasAccess("Org"))
                    userID = UserUtil.GetCurrentUserID();

                if (userID != null)
                    filters[FormDataConstants.UserIDField] = userID;
            }

            var sorts = new[] { $"{FormDataConstants.DateCreatedField} desc" };
            var documents = MongoDbUtil.FindDocuments(collectionID, filters, sorts, true);

            var formDataUnits = BsonDocumentConverter.ConvertToFormDataUnit(documents);

            if (!UserUtil.IsSuperAdmin() && UmUtil.Instance.HasAccess("Org"))
            {
                var orgUserID = UserUtil.GetCurrentUserID();
                formDataUnits = FormDataDbUtil.FilterByUserStatus(formDataUnits, orgUserID);
            }

            var formDataView = new DictionaryDataView(formDataUnits, fieldSet);

            formDataGridControl.BindData(formDataView);
        }

        protected void CheckUserMode()
        {
            if (OwnerID != null && FormID != OwnerID)
                return;

            if (UmUtil.Instance.HasAccess("Admin"))
                return;

            if (DbForm == null)
                return;

            if (String.IsNullOrWhiteSpace(DbForm.UserMode))
                return;

            if (DbForm.UserMode == "SingleRecord")
            {
                var returnUrl = RequestUrl.ToEncodedUrl();

                var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
                {
                    ["Mode"] = Convert.ToString(FormMode.Edit),
                    ["FormID"] = FormID,
                    ["OwnerID"] = (OwnerID ?? FormID),
                    ["RecordID"] = null,
                    ["ParentID"] = ParentID,
                    ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
                };

                var formData = GetLastFormData(DbForm.ID);
                if (formData != null)
                {
                    if (formData.ID == null)
                        throw new Exception();

                    urlHelper["RecordID"] = formData.ID;

                    if (formData.StatusID == DataStatusCache.Rejected.ID)
                        urlHelper["Mode"] = Convert.ToString(FormMode.Review);

                    if (formData.StatusID == DataStatusCache.Submit.ID ||
                        formData.StatusID == DataStatusCache.Accepted.ID)
                        urlHelper["Mode"] = Convert.ToString(FormMode.None);
                }

                Response.Redirect(urlHelper.ToEncodedUrl());
            }
        }

        protected FormDataUnit GetLastFormData(Guid formID)
        {
            var userID = UserUtil.GetCurrentUserID();

            var filter = new Dictionary<String, Object>
            {
                [FormDataConstants.UserIDField] = userID,
                [FormDataConstants.DateDeletedField] = null
            };

            var sort = new[]
            {
                $"{FormDataConstants.DateCreatedField} desc"
            };

            var documents = MongoDbUtil.FindDocuments(formID, filter, sort).ToList();

            var document = documents.FirstOrDefault();
            if (documents.Count > 1)
            {
                foreach (var bsonDoc in documents)
                {
                    if (ReferenceEquals(bsonDoc, document))
                        continue;

                    bsonDoc[FormDataConstants.DateDeletedField] = DateTime.Now;
                    MongoDbUtil.UpdateDocument(formID, bsonDoc);
                }
            }

            var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
            return formData;

        }

        protected BsonArray CreateNewFormStatuses(BsonDocument document, ISet<Guid?> newUsers)
        {
            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                bsonValue = new BsonArray();

            var oldArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (oldArray == null)
                oldArray = new BsonArray();

            var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(oldArray).ToHashSet();
            var oldUsersDict = statusUnits.ToDictionary(n => n.UserID.GetValueOrDefault());

            var insertUsers = newUsers.Where(n => !oldUsersDict.ContainsKey(n.GetValueOrDefault())).ToList();
            var deleteUsers = oldUsersDict.Keys.Where(n => !newUsers.Contains(n)).ToList();

            foreach (var userID in insertUsers)
            {
                var unit = new FormStatusUnit
                {
                    UserID = userID,
                    DateOfAssigne = DateTime.Now,
                };

                oldUsersDict.Add(userID.GetValueOrDefault(), unit);
            }

            foreach (var userID in deleteUsers)
            {
                oldUsersDict.Remove(userID);
            }

            var formStatusDocs = BsonDocumentConverter.ConvertToFormStatuses(oldUsersDict.Values);

            var newArray = new BsonArray(formStatusDocs);
            return newArray;
        }
    }
}