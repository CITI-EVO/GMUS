using System;
using System.Collections.Generic;
using Gms.Portal.Web.Entities.DataContainer;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gms.Portal.Web.Utils
{
    public static class MongoDbUtil
    {
        public const String IDField = "_id";

        public static IMongoCollection<BsonDocument> GetCollection(Guid? collectionID)
        {
            if (collectionID == null)
                throw new ArgumentNullException("collectionID");

            var collectionName = String.Format("Collection_{0:n}", collectionID);

            var client = new MongoClient(ConfigUtil.MongoConnectionString);
            var database = client.GetDatabase(ConfigUtil.MongoDatabaseName);

            var collection = database.GetCollection<BsonDocument>(collectionName);
            return collection;
        }

        public static IEnumerable<BsonDocument> FindObject(IMongoCollection<BsonDocument> collection, FilterDefinition<BsonDocument> filter)
        {
            using (var cursorTask = collection.FindAsync(filter))
            {
                cursorTask.Wait();

                var cursor = cursorTask.Result;
                while (cursor.MoveNext())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        yield return document;
                    }
                }
            }
        }
    }
}