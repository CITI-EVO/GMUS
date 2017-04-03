using System;
using System.Collections.Generic;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gms.Portal.Web.Utils
{
    public static class MongoDbUtil
    {
        public const String IDField = "_id";

        public static void InsertDocument(Guid? collectionID, BsonDocument document)
        {
            var collection = GetCollection(collectionID);
            InsertDocument(collection, document);
        }
        public static void InsertDocument(IMongoCollection<BsonDocument> collection, BsonDocument document)
        {
            if (collection == null)
                return;

            collection.InsertOne(document);
        }

        public static BsonDocument GetDocument(Guid? collectionID, Guid? recordID)
        {
            var collection = GetCollection(collectionID);
            return GetDocument(collection, recordID);
        }
        public static BsonDocument GetDocument(IMongoCollection<BsonDocument> collection, Guid? recordID)
        {
            if (collection == null || recordID == null)
                return null;

            var query = (from n in collection.AsQueryable()
                         where n[FormDataConstants.IDField] == recordID
                         select n);

            var document = query.FirstOrDefault();
            return document;
        }

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

        public static IEnumerable<BsonDocument> FindDocuments(IMongoCollection<BsonDocument> collection, FilterDefinition<BsonDocument> filter)
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

        public static void MergeDocuments(BsonDocument target, BsonDocument source)
        {
            var names = new HashSet<String>();
            names.UnionWith(target.Names);
            names.UnionWith(source.Names);

            foreach (var name in names)
            {
                if (name != IDField)
                    target[name] = source[name];
            }
        }

        public static void UpdateDocument(Guid? collectionID, BsonDocument document)
        {
            var collection = GetCollection(collectionID);
            UpdateDocument(collection, document);
        }
        public static void UpdateDocument(IMongoCollection<BsonDocument> collection, BsonDocument document)
        {
            if (collection == null)
                return;

            var filter = Builders<BsonDocument>.Filter.Eq(IDField, document[IDField]);
            var update = Builders<BsonDocument>.Update.Set(IDField, document[IDField]);

            foreach (var name in document.Names)
            {
                if (name != IDField)
                    update = update.Set(name, document[name]);
            }

            collection.UpdateOne(filter, update);
        }

        public static void ClearCollection(Guid? collectionID)
        {
            var collection = GetCollection(collectionID);
            ClearCollection(collection);
        }
        public static void ClearCollection(IMongoCollection<BsonDocument> collection)
        {
            if (collection == null)
                return;

            var filter = Builders<BsonDocument>.Filter.Empty;
            collection.DeleteMany(filter);
        }
    }
}