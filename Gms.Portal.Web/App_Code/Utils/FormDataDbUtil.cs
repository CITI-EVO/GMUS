using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Helpers;

namespace Gms.Portal.Web.Utils
{
    public static class FormDataDbUtil
    {
        public static bool RestoreVersion(Guid? ownerID, Guid? lastID, int version)
        {
            var collection = MongoDbUtil.GetCollection(ownerID);
            if (collection == null)
                return false;

            var formDatas = GetFullHierarchy(ownerID, lastID);

            var formDatasQuery = (from n in formDatas
                                  orderby n.Version descending, n.DateCreated descending
                                  select n);

            var oldFormData = formDatasQuery.FirstOrDefault(n => n.Version == version);
            if (oldFormData == null)
                return false;

            var lastFormData = formDatasQuery.FirstOrDefault(n => n.ID == lastID);
            if (lastFormData == null)
                return false;

            var newFormData = new FormDataUnit(oldFormData)
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                Version = lastFormData.Version + 1,
                PreviousID = lastFormData.ID,
                DateDeleted = null,
            };

            var document = BsonDocumentConverter.ConvertToBsonDocument(newFormData);
            MongoDbUtil.InsertDocument(ownerID, document);

            var subListRefs = newFormData.Values.OfType<FormDataListRef>();
            foreach (var listRef in subListRefs)
            {
                var subCollectionID = (listRef.OwnerID ?? listRef.FormID);

                var subFilter = new Dictionary<String, Object>
                {
                    [FormDataConstants.ParentIDField] = null
                };

                var subDocuments = MongoDbUtil.FindDocuments(subCollectionID, subFilter);
                var subFormDatas = subDocuments.Select(BsonDocumentConverter.ConvertToFormDataUnit).ToList();

                var insertList = new List<FormDataUnit>();
                var deleteList = subFormDatas.Where(n => n.DateDeleted == null).ToList();

                var subFormDatasLp = subFormDatas.ToLookup(n => n.PreviousID);

                var restoreFormDatas = subFormDatas.Where(n => n.ParentVersion < oldFormData.Version).ToList();
                if (restoreFormDatas.Count == 0)
                    restoreFormDatas = deleteList.ToList();

                foreach (var subFormData in restoreFormDatas)
                {
                    var newSubFormData = new FormDataUnit(subFormData)
                    {
                        ID = Guid.NewGuid(),
                        ParentID = newFormData.ID,
                        DateCreated = DateTime.Now,
                        DateDeleted = null,
                        ParentVersion = newFormData.Version
                    };

                    var lastSubFormData = GetLastFormData(subFormDatasLp, subFormData);
                    if (lastSubFormData != null)
                    {
                        lastSubFormData.PreviousID = newSubFormData.ID;
                        newSubFormData.Version = lastSubFormData.Version.GetValueOrDefault() + 1;
                    }

                    insertList.Add(newSubFormData);
                }

                var subCollection = MongoDbUtil.GetCollection(subCollectionID);

                foreach (var subFormData in deleteList)
                {
                    var subFormUpdate = Builders<BsonDocument>.Update.Set(FormDataConstants.DateDeletedField, DateTime.Now);
                    var subFormFilter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, subFormData.ID);

                    subCollection.UpdateMany(subFormFilter, subFormUpdate);
                }

                foreach (var subFormData in insertList)
                {
                    var subDocument = BsonDocumentConverter.ConvertToBsonDocument(subFormData);
                    MongoDbUtil.InsertDocument(ownerID, subDocument);
                }
            }

            return true;
        }

        public static bool RestoreVersion(Guid? ownerID, Guid? lastID, Guid? restoreID)
        {
            var collection = MongoDbUtil.GetCollection(ownerID);
            if (collection == null)
                return false;

            var formDatas = GetFullHierarchy(ownerID, lastID);

            var formDatasQuery = (from n in formDatas
                                  orderby n.Version descending, n.DateCreated descending
                                  select n);

            var oldFormData = formDatasQuery.FirstOrDefault(n => n.ID == restoreID);
            if (oldFormData == null)
                return false;

            var lastFormData = formDatasQuery.FirstOrDefault(n => n.ID == lastID);
            if (lastFormData == null)
                return false;

            var newFormData = new FormDataUnit(oldFormData)
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                Version = lastFormData.Version + 1,
                PreviousID = lastFormData.ID,
                DateDeleted = null,
                DateOfStatus = DateTime.Now
            };

            var document = BsonDocumentConverter.ConvertToBsonDocument(newFormData);
            MongoDbUtil.InsertDocument(ownerID, document);

            var subListRefs = newFormData.Values.OfType<FormDataListRef>();
            foreach (var listRef in subListRefs)
            {
                var subCollectionID = (listRef.OwnerID ?? listRef.FormID);

                var subFilter = new Dictionary<String, Object>
                {
                    [FormDataConstants.ParentIDField] = lastID
                };

                var subDocuments = MongoDbUtil.FindDocuments(subCollectionID, subFilter);
                var subFormDatas = subDocuments.Select(BsonDocumentConverter.ConvertToFormDataUnit).ToList();

                var statusDate = oldFormData.DateOfStatus;

                var insertList = new List<FormDataUnit>();
                var deleteList = (from n in subFormDatas
                                  where n.DateCreated > statusDate &&
                                        n.DateDeleted == null
                                  select n).ToList();

                var subFormDatasLp = subFormDatas.ToLookup(n => n.PreviousID);

                var restoreFormDatas = (from n in subFormDatas
                                        where n.DateCreated < statusDate &&
                                              n.DateDeleted != null && n.DateDeleted > statusDate
                                        select n).ToList();

                if (restoreFormDatas.Count == 0)
                    restoreFormDatas = deleteList.ToList();

                foreach (var subFormData in restoreFormDatas)
                {
                    var newSubFormData = new FormDataUnit(subFormData)
                    {
                        ID = Guid.NewGuid(),
                        ParentID = newFormData.ID,
                        DateCreated = DateTime.Now,
                        DateDeleted = null,
                        ParentVersion = newFormData.Version
                    };

                    var lastSubFormData = GetLastFormData(subFormDatasLp, subFormData);
                    if (lastSubFormData != null)
                    {
                        lastSubFormData.PreviousID = newSubFormData.ID;
                        newSubFormData.Version = lastSubFormData.Version.GetValueOrDefault() + 1;
                    }

                    insertList.Add(newSubFormData);
                }

                var subCollection = MongoDbUtil.GetCollection(subCollectionID);

                foreach (var subFormData in deleteList)
                {
                    var subFormUpdate = Builders<BsonDocument>.Update.Set(FormDataConstants.DateDeletedField, DateTime.Now);
                    var subFormFilter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, subFormData.ID);

                    subCollection.UpdateMany(subFormFilter, subFormUpdate);
                }

                foreach (var subFormData in insertList)
                {
                    var subDocument = BsonDocumentConverter.ConvertToBsonDocument(subFormData);
                    MongoDbUtil.InsertDocument(ownerID, subDocument);
                }
            }

            return true;
        }

        public static IEnumerable<FormDataUnit> GetFullHierarchy(Guid? collectionID, Guid? lastID)
        {
            var documents = GetFullHierarchyDocs(collectionID, lastID);
            var formDatas = documents.Select(BsonDocumentConverter.ConvertToFormDataUnit);

            return formDatas;
        }

        private static IEnumerable<BsonDocument> GetFullHierarchyDocs(Guid? collectionID, Guid? lastID)
        {
            var previousID = lastID;
            while (previousID != null)
            {
                var document = MongoDbUtil.GetDocument(collectionID, lastID);
                if (document == null)
                    yield break;

                yield return document;

                var value = BsonTypeMapper.MapToDotNetValue(document[FormDataConstants.PreviousIDField]);
                previousID = DataConverter.ToNullableGuid(value);
            }
        }

        private static FormDataUnit GetLastFormData(ILookup<Guid?, FormDataUnit> formDataLp, FormDataUnit formData)
        {
            var prevFormData = (FormDataUnit)null;

            while (formData != null)
            {
                prevFormData = formData;
                formData = formDataLp[formData.ID].FirstOrDefault();
            }

            return prevFormData;
        }

        public static bool ChangeStatus(Guid? ownerID, RecordStatusModel model)
        {
            return ChangeStatus(ownerID, model.RecordID.GetValueOrDefault(), model.StatusID.GetValueOrDefault(), model.Description);
        }
        public static bool ChangeStatus(Guid? ownerID, Guid recordID, Guid statusID, String description)
        {
            var collection = MongoDbUtil.GetCollection(ownerID);

            var update = Builders<BsonDocument>.Update.Set(FormDataConstants.StatusIDField, statusID);
            update = update.Set(FormDataConstants.DescriptionField, description);
            update = update.Set(FormDataConstants.DateOfStatusField, DateTime.Now);

            if (statusID == DataStatusCache.Submit.ID)
                update = update.Set(FormDataConstants.DateOfSubmitField, DateTime.Now);

            if (statusID == DataStatusCache.Accepted.ID)
                update = update.Set(FormDataConstants.DateOfAcceptField, DateTime.Now);

            var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);

            var result = collection.UpdateMany(filter, update);
            return (result.ModifiedCount > 0);
        }

        public static IEnumerable<FormDataUnit> FilterByUserStatus(IEnumerable<FormDataUnit> source, Guid? userID)
        {
            var query = (from n in source
                         where IsUserInUserStatus(n, userID)
                         select n);

            return query;
        }

        public static bool IsUserInUserStatus(FormDataUnit formData, Guid? userID)
        {
            if (userID == null || formData == null || formData.UserStatuses == null)
                return false;

            if (formData.ID == null)
            {
                var query = from m in formData.UserStatuses
                            where m.UserID == userID
                            select m;

                return query.Any();
            }

            var cache = CommonObjectCache.InitObject("@UserStatuses", CommonCacheStore.Request, ConcurrencyHelper.CreateDictionary<Guid?, ILookup<Guid?, FormStatusUnit>>);

            var statusesLp = cache.GetValueOrDefault(formData.ID);
            if (statusesLp == null)
            {
                statusesLp = formData.UserStatuses.ToLookup(n => n.UserID);
                cache.Add(formData.ID, statusesLp);
            }

            return statusesLp.Contains(userID);
        }

        public static IEnumerable<FormDataUnit> GetPreviousFormDatas(FormDataUnit formData)
        {
            while (formData != null)
            {
                if (formData.PreviousID != null)
                    formData = GetFormData(formData.OwnerID, formData.PreviousID);

                if (formData != null)
                    yield return formData;
            }
        }

        public static FormDataUnit GetFormData(Guid? ownerID, Guid? recordID)
        {
            if (ownerID == null || recordID == null)
                return null;

            var filter = new Dictionary<String, Object>
            {
                {FormDataConstants.IDField, recordID }
            };

            var documents = MongoDbUtil.FindDocuments(ownerID, filter).ToList();

            var document = documents.FirstOrDefault();
            if (document == null)
                return null;

            var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
            return formData;
        }
    }
}