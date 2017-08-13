using System;
using System.Collections.Generic;
using System.Configuration;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using NHibernate.Linq;
using System.Linq;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using CITI.EVO.Tools.Web.UI.Helpers;
using MongoDB.Driver;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FormApproveDataGrid : BasePage
    {
        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
        }

        public String SourceField
        {
            get { return DataConverter.ToString(RequestUrl["SourceField"]); }
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
            InitDataGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UmUtil.Instance.IsLogged)
                return;

            FillDataGrid();
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            var model = formApproveDataGridFilterControl.Model;

            var url = new UrlHelper(RequestUrl)
            {
                ["FormID"] = model.FormID,
                ["SourceField"] = model.SourceField
            };

            Response.Redirect(url.ToEncodedUrl());
        }

        protected void formApproveDataGridControl_OnStatus(object sender, GenericEventArgs<Guid?> e)
        {
            var dataApproveModel = formApproveDataGridControl.Model;

            var document = MongoDbUtil.GetDocument(dataApproveModel.FormID, e.Value);

            var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
            if (formData == null)
                return;

            var model = new RecordStatusModel
            {
                FormID = dataApproveModel.FormID,
                SourceField = dataApproveModel.SourceField,
                RecordID = formData.ID,
                StatusID = formData.StatusID,
            };

            recordStatusControl.Model = model;
            mpeRecordStatus.Show();
        }

        protected void recordStatusControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeRecordStatus.Show();
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
            FillDataGrid();

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

        protected void ChangeStatus()
        {
            var dataApproveModel = formApproveDataGridControl.Model;

            var formID = dataApproveModel.FormID;
            var ownerID = dataApproveModel.FormID;
            var sourceField = dataApproveModel.SourceField;

            var statusModel = recordStatusControl.Model;

            var document = MongoDbUtil.GetDocument(ownerID, statusModel.RecordID);

            if (UserUtil.IsSuperAdmin())
            {
                var formDataUnit = BsonDocumentConverter.ConvertToFormDataUnit(document);
                if (formDataUnit == null)
                    return;

                FormDataDbUtil.ChangeStatus(ownerID.GetValueOrDefault(), statusModel);
            }
            else
            {
                BsonValue bsonValue;
                if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                    return;

                var oldArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
                if (oldArray == null)
                    oldArray = new BsonArray();

                var formStatuses = BsonDocumentConverter.ConvertToFormStatuses(oldArray);
                var dictionary = formStatuses.ToDictionary(n => n.UserID);

                var userID = UserUtil.GetCurrentUserID();
                if (userID == null)
                    throw new Exception();

                var formStatus = dictionary.GetValueOrDefault(userID);
                if (formStatus == null)
                {
                    formStatus = new FormStatusUnit
                    {
                        UserID = userID
                    };

                    dictionary.Add(userID, formStatus);
                }

                var @params = new Dictionary<String, Object>
                {
                    {FormDataConstants.FieldParams, "@"},
                    {FormDataConstants.FieldIDField, sourceField}
                };

                formStatus.Params = @params;
                formStatus.UserID = userID;
                formStatus.StatusID = statusModel.StatusID;
                formStatus.DateOfStatus = DateTime.Now;

                var statusDocs = BsonDocumentConverter.ConvertToFormStatuses(dictionary.Values);
                var newArray = new BsonArray(statusDocs);

                document[FormDataConstants.UserStatusesFields] = newArray;

                MongoDbUtil.UpdateDocument(ownerID, document);
            }

            FillDataGrid();
        }

        protected void InitDataGrid()
        {
            if (DbForm == null)
                return;

            formApproveDataGridControl.InitStructure(DbForm);
        }

        protected void FillDataGrid()
        {
            if (DbForm == null || FormEntity == null)
                return;

            var controls = FormStructureUtil.InOrderFirstLevelTraversal(FormEntity);

            var formFields = (from n in controls.OfType<FieldEntity>()
                              where n.Visible &&
                                    n.DisplayOnGrid == "Always"
                              orderby n.OrderIndex, n.Name
                              select n.Name).ToHashSet();

            var defaultFields = FormDataBase.DefaultFields;

            var fields = defaultFields.Union(formFields).ToHashSet();

            var formDatas = LoadUserData();
            var dataView = new DictionaryDataView(formDatas, fields);

            var filterModel = formApproveDataGridFilterControl.Model;

            var dataAproveModel = new FormApproveDataGridModel
            {
                FormID = filterModel.FormID,
                SourceField = filterModel.SourceField,
                DataView = dataView
            };

            formApproveDataGridControl.Model = dataAproveModel;
            formApproveDataGridControl.DataBind();
        }

        protected IEnumerable<Guid> GetLegalFormsID()
        {
            var profileForms = ConfigurationManager.AppSettings["LegalForms"];
            var parts = profileForms.Split(',');

            var query = (from n in parts
                         let g = DataConverter.ToNullableGuid(n.Trim())
                         where g != null
                         select g.Value);

            return query.ToList();
        }

        protected IEnumerable<FormDataBase> LoadUserData()
        {
            if (!RegexUtil.DataSourceParserRx.IsMatch(SourceField))
                yield break;

            var match = RegexUtil.DataSourceParserRx.Match(SourceField);

            var ownerID = DataConverter.ToNullableGuid(match.Groups["parentID"].Value);
            var fieldID = DataConverter.ToNullableGuid(match.Groups["childID"].Value);

            if (FormID == null || fieldID == null || FormEntity == null)
                yield break;

            var fields = FormStructureUtil.PreOrderTraversal(FormEntity).OfType<FieldEntity>();

            var field = fields.FirstOrDefault(n => n.ID == fieldID);
            if (field == null || String.IsNullOrWhiteSpace(field.DataSourceID))
                yield break;

            if (field.ValueExpression != FormDataConstants.IDField)
                yield break;

            var myRecordsID = GetMyRecordID(field.DataSourceID);

            var formRecords = GetFormRecords(myRecordsID, FormID, ownerID, fieldID);

            var recordsSet = new HashSet<Guid?>();

            foreach (var formRecord in formRecords)
            {
                if (recordsSet.Add(formRecord.ID))
                    yield return formRecord;
            }
        }

        protected String[] GetMyRecordID(String dataSource)
        {
            var cache = CommonObjectCache.InitObject("@{MyRecordsCache}", CommonCacheStore.Request, () => new Dictionary<String, String[]>());

            var array = cache.GetValueOrDefault(dataSource);
            if (array != null)
                return array;

            var childID = (Guid?)null;
            var parentID = DataConverter.ToNullableGuid(dataSource);

            if (RegexUtil.DataSourceParserRx.IsMatch(dataSource))
            {
                var match = RegexUtil.DataSourceParserRx.Match(dataSource);

                parentID = DataConverter.ToNullableGuid(match.Groups["parentID"].Value);
                childID = DataConverter.ToNullableGuid(match.Groups["childID"].Value);
            }

            var myDataFilter = new Dictionary<String, Object>
            {
                [FormDataConstants.DateDeletedField] = null,
            };

            if (!UmUtil.Instance.HasAccess("Admin"))
                myDataFilter[FormDataConstants.UserIDField] = UserUtil.GetCurrentUserID();

            var collectionID = (childID ?? parentID);

            var documents = MongoDbUtil.FindDocuments(collectionID, myDataFilter);

            var formDatas = BsonDocumentConverter.ConvertToFormDataUnit(documents).ToList();
            if (formDatas.Count == 0)
                return null;

            array = formDatas.Select(n => Convert.ToString(n.ID)).ToArray();
            cache[dataSource] = array;

            return array;
        }

        protected IEnumerable<IDictionary<String, Object>> GetLegalRecords()
        {
            var forms = GetLegalFormsID();
            var converter = new FormEntityModelConverter(HbSession);

            foreach (var formID in forms)
            {
                var dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == formID);
                var model = converter.Convert(dbForm);

                var controls = FormStructureUtil.PreOrderTraversal(model.Entity);
                var controlsDict = controls.ToDictionary(n => Convert.ToString(n.ID), n => (String.IsNullOrWhiteSpace(n.Alias) ? n.Alias : n.Name));

                var collection = MongoDbUtil.GetCollection(formID);

                var query = (from n in collection.AsQueryable()
                    where n[FormDataConstants.DateDeletedField] == (DateTime?)null
                    select n);

                foreach (var bsonDoc in query)
                {
                    var formData = BsonDocumentConverter.ConvertToFormDataUnit(bsonDoc);

                    var resultDict = FormDataUtil.Transform(formData, controlsDict);
                    yield return resultDict;
                }
            }
        }

        protected IEnumerable<FormDataUnit> GetFormRecords(String[] array, Guid? formID, Guid? ownerID, Guid? fieldID)
        {
            var query = CreateRecordsQuery(array, formID, ownerID, fieldID);

            foreach (var document in query)
            {
                var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
                yield return formData;
            }
        }

        protected IQueryable<BsonDocument> CreateRecordsQuery(String[] array, Guid? formID, Guid? ownerID, Guid? fieldID)
        {
            var fieldKey = Convert.ToString(fieldID);
            var bsonValues = array.Select(BsonValue.Create).ToArray();

            if (formID != ownerID)
            {
                var parentCollection = MongoDbUtil.GetCollection(formID);
                var childCollection = MongoDbUtil.GetCollection(ownerID);

                var query = (from n in childCollection.AsQueryable()
                             where bsonValues.Contains(n[fieldKey])
                             join m in parentCollection.AsQueryable() on n[FormDataConstants.ParentIDField] equals m[FormDataConstants.IDField]
                             where m[FormDataConstants.DateDeletedField] == (DateTime?)null
                             select m);

                return query;
            }
            else
            {
                var collection = MongoDbUtil.GetCollection(ownerID);

                var query = (from n in collection.AsQueryable()
                             where n[FormDataConstants.DateDeletedField] == (DateTime?)null &&
                                   bsonValues.Contains(n[fieldKey])
                             select n);

                return query;
            }
        }
    }
}