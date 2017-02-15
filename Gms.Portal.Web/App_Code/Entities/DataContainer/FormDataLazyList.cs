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
        private bool _initialized;

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

        public override IEnumerator<FormDataUnit> GetEnumerator()
        {
            InitializeItems();
            return base.GetEnumerator();
        }

        private void InitializeItems()
        {
            if (_initialized)
                return;

            if (FormID == null && OwnerID == null)
                throw new Exception();

            var collID = (OwnerID ?? FormID);

            var collection = MongoDbUtil.GetCollection(collID);

            var query = from doc in collection.AsQueryable()
                        where doc[FormDataUnit.DateDeletedField] == (DateTime?)null
                        select doc;

            if (ParentID != null)
            {
                query = (from doc in query
                         where doc[FormDataUnit.ParentIDField] == ParentID
                         select doc);
            }

            //var commonFilter = Builders<BsonDocument>.Filter.Eq("DateDeleted", (DateTime?)null);
            //if (ParentID != null)
            //{
            //    var parentFilter = Builders<BsonDocument>.Filter.Eq("ParentID", ParentID);
            //    commonFilter = Builders<BsonDocument>.Filter.And(commonFilter, parentFilter);
            //}

            //var documents = Enumerable.ToList(MongoDbUtil.FindObject(collection, commonFilter));

            var converter = new BsonDocumentToFormDataUnitConverter();
            foreach (var formDataUnit in converter.Convert(query))
            {
                formDataUnit.FormID = FormID;
                formDataUnit.OwnerID = OwnerID;

                Add(formDataUnit);
            }

            _initialized = true;
        }
    }
}