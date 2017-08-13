using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FullApproveDataGrid : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        protected void fullApproveDataGridControl_OnAccept(object sender, GenericEventArgs<String> e)
        {
            if (!RegexUtil.RecordFormFieldParserRx.IsMatch(e.Value))
                return;

            var match = RegexUtil.RecordFormFieldParserRx.Match(e.Value);

            var recordID = DataConverter.ToNullableGuid(match.Groups["recordID"].Value);
            var fieldID = DataConverter.ToNullableGuid(match.Groups["fieldID"].Value);
            var formID = DataConverter.ToNullableGuid(match.Groups["formID"].Value);

            var document = MongoDbUtil.GetDocument(formID, recordID);

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
                {FormDataConstants.FieldIDField, fieldID}
            };

            formStatus.Params = @params;
            formStatus.UserID = userID;
            formStatus.StatusID = DataStatusCache.Accepted.ID;
            formStatus.DateOfStatus = DateTime.Now;

            var statusDocs = BsonDocumentConverter.ConvertToFormStatuses(dictionary.Values);
            var newArray = new BsonArray(statusDocs);

            document[FormDataConstants.UserStatusesFields] = newArray;

            MongoDbUtil.UpdateDocument(formID, document);

            FillDataGrid();
        }

        protected void fullApproveDataGridControl_OnReject(object sender, GenericEventArgs<String> e)
        {
            if (!RegexUtil.RecordFormFieldParserRx.IsMatch(e.Value))
                return;

            var match = RegexUtil.RecordFormFieldParserRx.Match(e.Value);

            var recordID = DataConverter.ToNullableGuid(match.Groups["recordID"].Value);
            var fieldID = DataConverter.ToNullableGuid(match.Groups["fieldID"].Value);
            var formID = DataConverter.ToNullableGuid(match.Groups["formID"].Value);

            var document = MongoDbUtil.GetDocument(formID, recordID);

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
                {FormDataConstants.FieldIDField, fieldID}
            };

            formStatus.Params = @params;
            formStatus.UserID = userID;
            formStatus.StatusID = DataStatusCache.Rejected.ID;
            formStatus.DateOfStatus = DateTime.Now;

            var statusDocs = BsonDocumentConverter.ConvertToFormStatuses(dictionary.Values);
            var newArray = new BsonArray(statusDocs);

            document[FormDataConstants.UserStatusesFields] = newArray;

            MongoDbUtil.UpdateDocument(formID, document);

            FillDataGrid();
        }

        protected void FillDataGrid()
        {
            var fields = new HashSet<String>
            {
                "FormName",
                "FieldName",
                "OrganizationName",

                FormDataConstants.IDField,
                FormDataConstants.FormIDField,
                FormDataConstants.FieldIDField,
                FormDataConstants.UserIDField,
                FormDataConstants.OwnerIDField,
                FormDataConstants.IDNumberField,
                FormDataConstants.DateCreatedField,
                FormDataConstants.DateOfSubmitField,
                FormDataConstants.UserStatusesFields,
            };

            var formDatas = LoadUserData();
            var dataView = new DictionaryDataView(formDatas, fields);

            var model = new FullApproveDataGridModel
            {
                DataView = dataView
            };

            fullApproveDataGridControl.Model = model;
            fullApproveDataGridControl.DataBind();
        }

        protected IEnumerable<FormDataBase> LoadUserData()
        {
            var controls = GetFormFields();

            var legalRecords = GetLegalRecords();
            var legalRecordsDict = legalRecords.ToDictionary(n => n.ID);

            foreach (var item in controls)
            {
                var formID = item.FormID;
                var ownerID = item.OwnerID;
                var fieldID = item.FieldID;
                var fieldKey = Convert.ToString(fieldID);

                ownerID = (ownerID ?? formID);

                var myRecordsID = GetMyRecordID(item.DataSource);
                if (myRecordsID.IsNullOrEmpty())
                    continue;

                var formRecords = GetFormRecords(myRecordsID, formID, ownerID, fieldID);
                var recordsSet = new HashSet<Guid?>();

                foreach (var formRecord in formRecords)
                {
                    if (!recordsSet.Add(formRecord.ID))
                        continue;

                    var orgID = DataConverter.ToNullableGuid(formRecord["FieldValue"]);
                    var orgName = String.Empty;

                    var orgRecord = legalRecordsDict.GetValueOrDefault(orgID);
                    if (orgRecord != null)
                        orgName = Convert.ToString(orgRecord.GetValueOrDefault("OrganizationName"));

                    var formData = new FormDataBase
                    {
                        ["FormName"] = item.FormName,
                        ["FieldName"] = item.FieldName,
                        ["OrganizationName"] = orgName,

                        [FormDataConstants.FormIDField] = item.FormID,
                        [FormDataConstants.FieldIDField] = item.FieldID,

                        [FormDataConstants.IDField] = formRecord[FormDataConstants.IDField],
                        [FormDataConstants.UserIDField] = formRecord[FormDataConstants.UserIDField],
                        [FormDataConstants.OwnerIDField] = formRecord[FormDataConstants.OwnerIDField],
                        [FormDataConstants.IDNumberField] = formRecord[FormDataConstants.IDNumberField],
                        [FormDataConstants.DateCreatedField] = formRecord[FormDataConstants.DateCreatedField],
                        [FormDataConstants.DateOfSubmitField] = formRecord[FormDataConstants.DateOfSubmitField],
                        [FormDataConstants.UserStatusesFields] = formRecord[FormDataConstants.UserStatusesFields],
                    };

                    yield return formData;
                }
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

            var collectionID = (childID ?? parentID);

            if (!UmUtil.Instance.HasAccess("Admin"))
                myDataFilter[FormDataConstants.UserIDField] = UserUtil.GetCurrentUserID();

            var documents = MongoDbUtil.FindDocuments(collectionID, myDataFilter);

            var formDatas = BsonDocumentConverter.ConvertToFormDataUnit(documents).ToList();
            if (formDatas.Count == 0)
                return null;

            array = formDatas.Select(n => Convert.ToString(n.ID)).ToArray();
            cache[dataSource] = array;

            return array;
        }

        protected IEnumerable<FormFieldInfoEntity> GetFormFields()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

            var dbForms = (from n in HbSession.Query<GM_Form>()
                           where n.DateDeleted == null &&
                                 n.RequiresApprove == true &&
                                 n.Visible == true
                           orderby n.OrderIndex, n.Name
                           select n).ToList();

            var converter = new FormEntityModelConverter(HbSession);
            var models = dbForms.Select(n => converter.Convert(n));

            var expGlobals = new ExpressionGlobalsUtil();

            foreach (var formModel in models)
            {
                if (formModel.Entity == null)
                    continue;

                if (!formModel.Visible.GetValueOrDefault())
                    continue;

                if (!String.IsNullOrWhiteSpace(formModel.VisibleExpression))
                {
                    var expNode = ExpressionParser.GetOrParse(formModel.VisibleExpression);

                    Object eval;
                    if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out eval))
                        continue;

                    var result = DataConverter.ToNullableBoolean(eval);
                    if (!result.GetValueOrDefault())
                        continue;
                }

                var treeNodes = FormStructureUtil.CreateTree(formModel.Entity);
                var treeDict = treeNodes.ToDictionary(n => n.ID);

                var controls = FormStructureUtil.PreOrderTraversal(formModel.Entity);

                foreach (var control in controls)
                {
                    var field = control as FieldEntity;
                    if (field == null)
                        continue;

                    if (!field.RequiresApproval.GetValueOrDefault())
                        continue;

                    if (String.IsNullOrWhiteSpace(field.DataSourceID))
                        continue;

                    if (!comparer.Equals(field.ValueExpression, FormDataConstants.IDField))
                        continue;

                    if (!comparer.Equals(field.Type, "Lookup") && !comparer.Equals(field.Type, "ComboBox"))
                        continue;

                    var owner = GetControlOwner(treeDict, field.ID);
                    if (owner.ControlType == "Form")
                    {
                        owner.ID = formModel.ID;
                        owner.Name = formModel.Name;
                    }

                    var entity = new FormFieldInfoEntity
                    {
                        FormID = formModel.ID,
                        FormName = formModel.Name,
                        OwnerID = owner.ID,
                        OwnerName = owner.Name,
                        FieldID = field.ID,
                        FieldName = field.Name,
                        DataSource = field.DataSourceID,
                    };

                    yield return entity;
                }
            }
        }

        protected IEnumerable<FormDataUnit> GetLegalRecords()
        {
            var converter = new FormEntityModelConverter(HbSession);

            var formsQuery = (from n in HbSession.Query<GM_Form>()
                              where n.DateDeleted == null &&
                                     n.FormType == "OrganizationProfile"
                              select n);

            foreach (var dbForm in formsQuery)
            {
                var model = converter.Convert(dbForm);

                var controls = FormStructureUtil.PreOrderTraversal(model.Entity);
                var controlsDict = controls.ToDictionary(n => Convert.ToString(n.ID), n => (!String.IsNullOrWhiteSpace(n.Alias) ? n.Alias : n.Name));

                var collection = MongoDbUtil.GetCollection(dbForm.ID);

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
            var query = CreateQuery(array, formID, ownerID, fieldID);

            foreach (var item in query)
            {
                var statusArray = BsonDocumentConverter.ConvertToBsonArray(item.UserStatuses);

                var formData = new FormDataUnit
                {
                    ["FieldValue"] = BsonDocumentConverter.ConvertToObject(item.FieldValue),

                    [FormDataConstants.IDField] = BsonDocumentConverter.ConvertToObject(item.ID),
                    [FormDataConstants.UserIDField] = BsonDocumentConverter.ConvertToObject(item.UserID),
                    [FormDataConstants.OwnerIDField] = BsonDocumentConverter.ConvertToObject(item.OwnerID),
                    [FormDataConstants.IDNumberField] = BsonDocumentConverter.ConvertToObject(item.IDNumber),
                    [FormDataConstants.DateCreatedField] = BsonDocumentConverter.ConvertToObject(item.DateCreated),
                    [FormDataConstants.DateOfSubmitField] = BsonDocumentConverter.ConvertToObject(item.DateOfSubmit),
                    [FormDataConstants.UserStatusesFields] = BsonDocumentConverter.ConvertToFormStatuses(statusArray),
                };

                yield return formData;
            }
        }

        protected IQueryable<RecordInfoEntity> CreateQuery(String[] array, Guid? formID, Guid? ownerID, Guid? fieldID)
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
                             select new RecordInfoEntity
                             {
                                 ID = m[FormDataConstants.IDField],
                                 FormID = m[FormDataConstants.FormIDField],
                                 UserID = m[FormDataConstants.UserIDField],
                                 OwnerID = m[FormDataConstants.OwnerIDField],
                                 IDNumber = m[FormDataConstants.IDNumberField],
                                 DateCreated = m[FormDataConstants.DateCreatedField],
                                 DateOfSubmit = m[FormDataConstants.DateOfSubmitField],
                                 UserStatuses = m[FormDataConstants.UserStatusesFields],
                                 FieldValue = n[fieldKey],
                             });

                return query;
            }
            else
            {
                var collection = MongoDbUtil.GetCollection(ownerID);

                var query = (from n in collection.AsQueryable()
                             where n[FormDataConstants.DateDeletedField] == (DateTime?)null &&
                                   bsonValues.Contains(n[fieldKey])
                             select new RecordInfoEntity
                             {
                                 ID = n[FormDataConstants.IDField],
                                 FormID = n[FormDataConstants.FormIDField],
                                 UserID = n[FormDataConstants.UserIDField],
                                 OwnerID = n[FormDataConstants.OwnerIDField],
                                 IDNumber = n[FormDataConstants.IDNumberField],
                                 DateCreated = n[FormDataConstants.DateCreatedField],
                                 DateOfSubmit = n[FormDataConstants.DateOfSubmitField],
                                 UserStatuses = n[FormDataConstants.UserStatusesFields],
                                 FieldValue = n[fieldKey],
                             });

                return query;
            }
        }

        protected ElementTreeNodeEntity GetControlOwner(IDictionary<Guid?, ElementTreeNodeEntity> treeDict, Guid? controlID)
        {
            var parents = FormStructureUtil.ParentsTraversal(treeDict, controlID);

            foreach (var node in parents)
            {
                if (node.ControlType == "Form" || node.ControlType == "Grid" || node.ControlType == "Tree")
                    return node;
            }

            return null;
        }
    }
}