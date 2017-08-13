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
    public class FormDataLazyList : FormDataListBase
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
            if (FormID == null && OwnerID == null)
                throw new Exception();

            if (OwnerID != null && OwnerID != FormID && ParentID == null)
                return 0;

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
            if (OwnerID != null && OwnerID != FormID && ParentID == null)
                return results;

            var filter = new Dictionary<String, Object>
            {
                [FormDataConstants.DateDeletedField] = null
            };

            if (ParentID != null)
                filter[FormDataConstants.ParentIDField] = ParentID;

            if (UserID != null)
                filter[FormDataConstants.UserIDField] = UserID;

            var documents = MongoDbUtil.FindDocuments(collID, filter);
            foreach (var document in documents)
            {
                var formDataUnit = BsonDocumentConverter.ConvertToFormDataUnit(document);
                formDataUnit.FormID = FormID;
                formDataUnit.OwnerID = OwnerID;

                results.Add(formDataUnit);
            }

            return results;
        }
    }
}