using System;
using System.Collections.Generic;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gms.Portal.Web.Utils
{
    public static class FormDataDbUtil
    {
        public static bool ChangeStatus(Guid ownerID, RecordStatusModel model)
        {
            return ChangeStatus(ownerID, model.RecordID.GetValueOrDefault(), model.StatusID.GetValueOrDefault(), model.Description);
        }
        public static bool ChangeStatus(Guid ownerID, Guid recordID, Guid statusID, String description)
        {
            var collection = MongoDbUtil.GetCollection(ownerID);

            var update = Builders<BsonDocument>.Update.Set(FormDataConstants.StatusIDField, statusID);
            update = update.Set(FormDataConstants.DescriptionField, description);
            update = update.Set(FormDataConstants.StatusChangeDateField, DateTime.Now);

            var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);

            var result = collection.UpdateMany(filter, update);
            return (result.ModifiedCount > 0);
        }

        public static void UpdateFields(Guid ownerID, Guid? recordID, IDictionary<String, Object> fields)
        {
            var collection = MongoDbUtil.GetCollection(ownerID);

            var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);
            var update = (UpdateDefinition<BsonDocument>)null;

            foreach (var pair in fields)
            {
                if (update == null)
                    update = Builders<BsonDocument>.Update.Set(pair.Key, pair.Value);
                else
                    update = update.Set(pair.Key, pair.Value);
            }

            collection.UpdateMany(filter, update);
        }

        public static void UpdateField(Guid ownerID, Guid? recordID, String fieldName, Object value)
        {
            var collection = MongoDbUtil.GetCollection(ownerID);

            var update = Builders<BsonDocument>.Update.Set(fieldName, value);
            var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);

            collection.UpdateMany(filter, update);
        }
    }
}