using System;
using System.Collections.Generic;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormDataLazyList : FormDataBaseList
    {
        public FormDataLazyList(FormDataLazyList formDataLazyList) : base(formDataLazyList)
        {
        }
        public FormDataLazyList(FormDataListRef formDataListRef)
            : base(formDataListRef.FormID, formDataListRef.OwnerID, formDataListRef.ParentID)
        {
        }

        public FormDataLazyList(Guid? formID, Guid? ownerID) : base(formID, ownerID)
        {
        }

        public FormDataLazyList(Guid? formID, Guid? ownerID, Guid? parentID) : base(formID, ownerID, parentID)
        {
        }

        public FormDataLazyList(Guid? formID, Guid? ownerID, Guid? parentID, Guid? userID) : base(formID, ownerID, parentID, userID)
        {
        }

        protected override int InitializeCount()
        {
            var collID = (OwnerID ?? FormID);

            var collection = MongoDbUtil.GetCollection(collID);
            if (collection == null)
                return 0;

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq(FormDataConstants.DateDeletedField, (DateTime?)null);

            if (ParentID != null)
                filter = filter & builder.Eq(FormDataConstants.ParentIDField, ParentID);

            if (UserID != null)
                filter = filter & builder.Eq(FormDataConstants.UserIDField, UserID);

            var count = collection.Count(filter);
            return (int)count;
        }

        protected override IList<FormDataUnit> InitializeItems()
        {
            if (FormID == null && OwnerID == null)
                throw new Exception();

            var collID = (OwnerID ?? FormID);

            var results = new List<FormDataUnit>();

            var collection = MongoDbUtil.GetCollection(collID);
            if (collection == null)
                return results;

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq(FormDataConstants.DateDeletedField, (DateTime?)null);

            if (ParentID != null)
                filter = filter & builder.Eq(FormDataConstants.ParentIDField, ParentID);

            if (UserID != null)
                filter = filter & builder.Eq(FormDataConstants.UserIDField, UserID);

            //var sort = Builders<BsonDocument>.Sort.Descending(FormDataConstants.DateCreatedField);

            var cursor = collection.FindSync(filter);
            while (cursor.MoveNext())
            {
                var bsonDocuments = cursor.Current;

                var formDataUnits = BsonDocumentConverter.ConvertToFormDataUnit(bsonDocuments);
                foreach (var formDataUnit in formDataUnits)
                {
                    formDataUnit.FormID = FormID;
                    formDataUnit.OwnerID = OwnerID;

                    results.Add(formDataUnit);
                }
            }

            return results;

            //var query = from doc in collection.AsQueryable()
            //            where doc[FormDataConstants.DateDeletedField] == (DateTime?)null
            //            select doc;

            //if (ParentID != null)
            //{
            //    query = (from doc in query
            //             where doc[FormDataConstants.ParentIDField] == ParentID
            //             select doc);
            //}

            //if (UserID != null)
            //{
            //    query = (from doc in query
            //             where doc[FormDataConstants.UserIDField] == UserID
            //             select doc);
            //}

            //query = (from doc in query
            //         orderby doc[FormDataConstants.DateCreatedField] descending
            //         select doc);

            //var formDataUnits = BsonDocumentConverter.ConvertToFormDataUnit(query);
            //foreach (var formDataUnit in formDataUnits)
            //{
            //    formDataUnit.FormID = FormID;
            //    formDataUnit.OwnerID = OwnerID;

            //    results.Add(formDataUnit);
            //}

            //return results;
        }
    }
}