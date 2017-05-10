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

        protected void formDataGridControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = Convert.ToString(FormMode.Edit),
                ["FormID"] = FormID,
                ["OwnerID"] = (OwnerID ?? FormID),
                ["RecordID"] = e.Value,
                ["ParentID"] = ParentID,
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void formDataGridControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = Convert.ToString(FormMode.View),
                ["FormID"] = FormID,
                ["OwnerID"] = (OwnerID ?? FormID),
                ["RecordID"] = e.Value,
                ["ParentID"] = ParentID,
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
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
            var model = new AssigneUsersModel
            {
                RecordID = e.Value,
            };

            var recordID = model.RecordID;
            var ownerID = (OwnerID ?? FormID);

            var document = MongoDbUtil.GetDocument(ownerID, recordID);

            BsonValue bsonValue;
            if (document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
            {
                var bsonArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
                var statusUsers = BsonDocumentConverter.ConvertToFormStatuses(bsonArray);

                var lookup = new HashLookup<int?, Guid?>();

                foreach (var item in statusUsers)
                    lookup.Add(item.Step, item.UserID);

                model.Users = lookup;
            }

            assigneUsersControl.Model = model;

            mpeAssigneUser.Show();
        }

        protected void formDataGridControl_OnPrint(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Handlers/PrintFormData.ashx")
            {
                ["FormID"] = FormID,
                ["RecordID"] = e.Value,
                ["LoginToken"] = UmUtil.Instance.CurrentToken,
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void recordStatusControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeRecordStatus.Show();
        }

        protected void btnAssigneUserOK_Click(object sender, EventArgs e)
        {
            var model = assigneUsersControl.Model;

            var users = model.Users;
            var recordID = model.RecordID;
            var ownerID = (OwnerID ?? FormID);

            var document = MongoDbUtil.GetDocument(ownerID, recordID);
            document[FormDataConstants.UserStatusesFields] = CreateNewFormStatuses(document, users);

            MongoDbUtil.UpdateDocument(ownerID, document);

            mpeAssigneUser.Hide();
        }

        protected void btnAssigneUserCancel_OnClick(object sender, EventArgs e)
        {
            mpeAssigneUser.Hide();
        }

        protected void btnRecordStatusOK_Click(object sender, EventArgs e)
        {
            var model = recordStatusControl.Model;
            if (model.StatusID == DataStatusCache.Accepted.ID)
            {
                mpeConfirmAccept.Show();
                return;
            }

            ChangeStatus();
            FillGridView();

            mpeRecordStatus.Hide();
        }

        protected void btnConfirmAcceptOK_Click(object sender, EventArgs e)
        {
            ChangeStatus();

            mpeConfirmAccept.Hide();
            mpeRecordStatus.Hide();
        }

        protected void btnRecordStatusCancel_OnClick(object sender, EventArgs e)
        {
            mpeRecordStatus.Hide();
        }

        protected void btnConfirmAcceptCancel_OnClick(object sender, EventArgs e)
        {
            mpeConfirmAccept.Hide();
        }

        protected void assigneUsersControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeAssigneUser.Show();
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

            if (FormID != UserUtil.GetMandatoryFormID())
                return;

            if (DbForm.UserMode == "SingleRecord")
            {
                var returnUrl = RequestUrl.ToEncodedUrl();

                var userID = UserUtil.GetCurrentUserID();
                var collection = MongoDbUtil.GetCollection(DbForm.ID);

                var document = (from n in collection.AsQueryable()
                                where n[FormDataConstants.UserIDField] == userID &&
                                      n[FormDataConstants.DateDeletedField] == (DateTime?)null
                                select n).FirstOrDefault();

                var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);

                var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
                {
                    ["Mode"] = Convert.ToString(FormMode.Edit),
                    ["FormID"] = FormID,
                    ["OwnerID"] = (OwnerID ?? FormID),
                    ["RecordID"] = null,
                    ["ParentID"] = ParentID,
                    ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
                };

                if (formData != null)
                {
                    if (formData.ID == null)
                        throw new Exception();

                    urlHelper["RecordID"] = formData.ID;

                    if (formData.StatusID == DataStatusCache.Rejected.ID)
                        urlHelper["Mode"] = Convert.ToString(FormMode.Review);

                    if (formData.StatusID == DataStatusCache.Submit.ID ||
                        formData.StatusID == DataStatusCache.Accepted.ID)
                        urlHelper["Mode"] = Convert.ToString(FormMode.View);
                }

                Response.Redirect(urlHelper.ToEncodedUrl());
            }
        }

        protected BsonArray CreateNewFormStatuses(BsonDocument document, ILookup<int?, Guid?> newUsersLp)
        {
            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                bsonValue = new BsonArray();

            var oldArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (oldArray == null)
                oldArray = new BsonArray();

            var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(oldArray).ToList();
            var oldUsersLp = statusUnits.ToLookup(n => n.Step);

            var list = new List<FormStatusUnit>();

            foreach (var usersGrp in newUsersLp)
            {
                var newUsers = usersGrp.ToHashSet();
                var oldUsers = oldUsersLp[usersGrp.Key];

                var lookup = oldUsers.ToLookup(n => n.UserID.GetValueOrDefault());
                var dictionary = lookup.ToDictionary(n => n.Key, n => n.First());

                var insertUsers = usersGrp.Where(n => !dictionary.ContainsKey(n.GetValueOrDefault())).ToList();
                var deleteUsers = dictionary.Keys.Where(n => !newUsers.Contains(n)).ToList();

                foreach (var userID in insertUsers)
                {
                    var unit = new FormStatusUnit
                    {
                        Step = usersGrp.Key,
                        UserID = userID,
                    };

                    dictionary.Add(userID.GetValueOrDefault(), unit);
                }

                foreach (var userID in deleteUsers)
                {
                    dictionary.Remove(userID);
                }

                list.AddRange(dictionary.Values);
            }

            var formStatusDocs = BsonDocumentConverter.ConvertToFormStatuses(list);

            var newArray = new BsonArray(formStatusDocs);
            return newArray;
        }
    }
}