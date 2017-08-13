using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Entities.DataContainer;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Enums;
using Expression = System.Linq.Expressions.Expression;

namespace Gms.Portal.Web.Utils
{
    public static class MongoDbUtil
    {
        public const String HistoryCollectionName = "Collection_RecordHistories";
        public const String MonitoringCollectionName = "Collection_RecordMonitorings";

        public static IEnumerable<BsonDocument> Execute(IQueryable<BsonDocument> query)
        {
            var fieldsLp = ExtractQueryFields(query);
            if (fieldsLp != null)
            {
                foreach (var fieldsPair in fieldsLp)
                {
                    var whereFields = fieldsPair.Value["where"];
                    var orderbyFields = fieldsPair.Value["orderby"];

                    DbIndexer(fieldsPair.Key, whereFields);
                    DbIndexer(fieldsPair.Key, orderbyFields);
                }
            }

            foreach (var bsonDoc in query)
                yield return bsonDoc;
        }

        public static void DbIndexer(Guid? collectionID, IEnumerable<String> fields)
        {
            DbIndexer(collectionID, fields, false);
        }
        public static void DbIndexer(Guid? collectionID, IEnumerable<String> fields, bool combine)
        {
            var collection = GetCollection(collectionID);
            DbIndexer(collection, fields, combine);
        }

        public static void DbIndexer(String collectionName, IEnumerable<String> fields)
        {
            DbIndexer(collectionName, fields, false);
        }
        public static void DbIndexer(String collectionName, IEnumerable<String> fields, bool combine)
        {
            var collection = GetCollection(collectionName);
            DbIndexer(collection, fields, combine);
        }

        public static void DbIndexer(IMongoCollection<BsonDocument> collection, IEnumerable<String> fields)
        {
            DbIndexer(collection, fields, false);
        }
        public static void DbIndexer(IMongoCollection<BsonDocument> collection, IEnumerable<String> fields, bool combine)
        {
            var list = collection.Indexes.List().ToEnumerable();
            var @set = list.Select(n => n["name"].AsString).ToHashSet();

            try
            {
                DbIndexer(collection, fields, @set, combine);
            }
            catch
            {
                foreach (var name in @set)
                {
                    if (name.StartsWith("@idx_"))
                        collection.Indexes.DropOne(name);
                }

                DbIndexer(collection, fields, @set, combine);
            }
        }

        private static void DbIndexer(IMongoCollection<BsonDocument> collection, IEnumerable<String> fields, ISet<String> exists, bool combine)
        {
            var builder = Builders<BsonDocument>.IndexKeys;
            var @set = new SortedSet<String>(fields);

            if (combine)
            {
                var strVal = String.Join("§", @set);
                var hash = strVal.ComputeMd5();
                var name = $"@idx_{hash}";

                if (exists.Contains(name))
                    return;

                var index = (IndexKeysDefinition<BsonDocument>)null;

                foreach (var field in @set)
                {
                    if (index == null)
                        index = builder.Ascending(field);
                    else
                        index = index.Ascending(field);
                }

                var options = new CreateIndexOptions
                {
                    Name = name,
                    Unique = false
                };

                collection.Indexes.CreateOne(index, options);
            }
            else
            {
                foreach (var field in @set)
                {
                    var hash = field.ComputeMd5();
                    var name = $"@idx_{hash}";

                    if (exists.Contains(name))
                        return;

                    var index = builder.Ascending(field);
                    var options = new CreateIndexOptions
                    {
                        Name = name,
                        Unique = false,
                    };

                    collection.Indexes.CreateOne(index, options);
                }
            }
        }

        public static SortDefinition<BsonDocument> CreateDbSort(IEnumerable<String> sortFields)
        {
            if (sortFields == null)
                return null;

            var builder = Builders<BsonDocument>.Sort;
            var sorts = CreateDbSorts(sortFields);

            var result = builder.Combine(sorts);
            return result;
        }
        public static IEnumerable<SortDefinition<BsonDocument>> CreateDbSorts(IEnumerable<String> sortFields)
        {
            if (sortFields == null)
                yield break;

            var builder = Builders<BsonDocument>.Sort;

            foreach (var sortField in sortFields)
            {
                var match = RegexUtil.SortingFieldsParserRx.Match(sortField);

                var name = match.Groups["name"].Value;
                name = (name ?? String.Empty).Trim();

                var type = match.Groups["type"].Value;
                type = (type ?? String.Empty).Trim();

                var sortOrder = DataConverter.ToNullableEnum<SortOrder>(type, true);

                switch (sortOrder.GetValueOrDefault())
                {
                    case SortOrder.Desc:
                        yield return builder.Descending(name);
                        break;
                    default:
                        yield return builder.Ascending(name);
                        break;
                }
            }
        }

        public static FilterDefinition<BsonDocument> CreateDbFilter(IDictionary<String, Object> filterData)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Empty;

            if (filterData == null)
                return filter;

            foreach (var pair in filterData)
            {
                var fieldKey = Convert.ToString(pair.Key);

                if (pair.Value is Tuple<Object, Object>)
                {
                    var tuple = (Tuple<Object, Object>)pair.Value;

                    var startVal = tuple.Item1;
                    var endVal = tuple.Item2;

                    if (!IsNullOrEmpty(startVal))
                        filter = filter & builder.Gte(fieldKey, startVal);

                    if (!IsNullOrEmpty(endVal))
                        filter = filter & builder.Lte(fieldKey, endVal);
                }
                else if (pair.Value is Array)
                {
                    var array = (Array)pair.Value;
                    var enumerable = array.Cast<Object>();

                    filter = filter & builder.In(fieldKey, enumerable);
                }
                else
                {
                    if (pair.Value == null)
                        filter = filter & builder.Eq(fieldKey, BsonNull.Value);
                    else
                    {
                        var fieldVal = pair.Value;

                        if (IsNullOrEmpty(fieldVal))
                        {
                            filter = filter & builder.Eq(fieldKey, BsonString.Empty);
                        }
                        else if (pair.Value is String)
                        {
                            var strVal = Convert.ToString(fieldVal);
                            var rgxVal = Regex.Escape(strVal);

                            var rgxExp = BsonRegularExpression.Create(rgxVal);
                            filter = filter & builder.Regex(fieldKey, rgxExp);
                        }
                        else
                        {
                            filter = filter & builder.Eq(fieldKey, fieldVal);
                        }
                    }
                }
            }

            return filter;
        }

        public static void InsertDocument(Guid? collectionID, BsonDocument document)
        {
            var collection = GetCollection(collectionID);
            InsertDocument(collection, document);
        }
        public static void InsertDocument(String collectionName, BsonDocument document)
        {
            var collection = GetCollection(collectionName);
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
        public static BsonDocument GetDocument(String collectionName, Guid? recordID)
        {
            var collection = GetCollection(collectionName);
            return GetDocument(collection, recordID);
        }

        public static BsonDocument GetDocument(IMongoCollection<BsonDocument> collection, Guid? recordID)
        {
            if (collection == null || recordID == null)
                return null;

            var filter = new Dictionary<String, Object>
            {
                [FormDataConstants.IDField] = recordID
            };

            var document = FindDocuments(collection, filter).FirstOrDefault();
            return document;
        }

        public static long GetCount(Guid? collectionID)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;

            var collection = GetCollection(collectionID);
            return collection.Count(filter);
        }
        public static long GetCount(String collectionName)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;

            var collection = GetCollection(collectionName);
            return collection.Count(filter);
        }

        public static IMongoCollection<BsonDocument> GetCollection(Guid? collectionID)
        {
            if (collectionID == null)
                throw new ArgumentNullException("collectionID");

            var collectionName = $"Collection_{collectionID:n}";
            return GetCollection(collectionName);
        }
        public static IMongoCollection<BsonDocument> GetCollection(String collectionName)
        {
            if (String.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentNullException("collectionID");

            var client = new MongoClient(ConfigUtil.MongoConnectionString);
            var database = client.GetDatabase(ConfigUtil.MongoDatabaseName);

            var collection = database.GetCollection<BsonDocument>(collectionName);
            return collection;
        }

        public static IEnumerable<BsonDocument> FindDocuments(Guid? collectionID, IDictionary<String, Object> filters)
        {
            return FindDocuments(collectionID, filters, false);
        }
        public static IEnumerable<BsonDocument> FindDocuments(Guid? collectionID, IDictionary<String, Object> filters, bool optimize)
        {
            return FindDocuments(collectionID, filters, null, false);
        }
        public static IEnumerable<BsonDocument> FindDocuments(Guid? collectionID, IDictionary<String, Object> filters, IEnumerable<String> sorts)
        {
            return FindDocuments(collectionID, filters, sorts, false);
        }
        public static IEnumerable<BsonDocument> FindDocuments(Guid? collectionID, IDictionary<String, Object> filters, IEnumerable<String> sorts, bool optimize)
        {
            var collection = GetCollection(collectionID);
            return FindDocuments(collection, filters, sorts, optimize);
        }

        public static IEnumerable<BsonDocument> FindDocuments(String collectionName, IDictionary<String, Object> filters)
        {
            return FindDocuments(collectionName, filters, false);
        }
        public static IEnumerable<BsonDocument> FindDocuments(String collectionName, IDictionary<String, Object> filters, bool optimize)
        {
            return FindDocuments(collectionName, filters, null, false);
        }
        public static IEnumerable<BsonDocument> FindDocuments(String collectionName, IDictionary<String, Object> filters, IEnumerable<String> sorts)
        {
            return FindDocuments(collectionName, filters, sorts, false);
        }
        public static IEnumerable<BsonDocument> FindDocuments(String collectionName, IDictionary<String, Object> filters, IEnumerable<String> sorts, bool optimize)
        {
            var collection = GetCollection(collectionName);
            return FindDocuments(collection, filters, sorts, optimize);
        }


        public static IEnumerable<BsonDocument> FindDocuments(IMongoCollection<BsonDocument> collection, IDictionary<String, Object> filters)
        {
            return FindDocuments(collection, filters, false);
        }
        public static IEnumerable<BsonDocument> FindDocuments(IMongoCollection<BsonDocument> collection, IDictionary<String, Object> filters, bool optimize)
        {
            return FindDocuments(collection, filters, null, false);
        }
        public static IEnumerable<BsonDocument> FindDocuments(IMongoCollection<BsonDocument> collection, IDictionary<String, Object> filters, IEnumerable<String> sorts)
        {
            return FindDocuments(collection, filters, sorts, false);
        }
        public static IEnumerable<BsonDocument> FindDocuments(IMongoCollection<BsonDocument> collection, IDictionary<String, Object> filters, IEnumerable<String> sorts, bool optimize)
        {
            var filter = CreateDbFilter(filters);
            var sort = CreateDbSort(sorts);

            if (optimize)
            {
                var filterFields = filters.Keys.ToHashSet();
                DbIndexer(collection, filterFields, true);

                if (sorts != null)
                {
                    var sortFields = sorts.ToHashSet();
                    DbIndexer(collection, sortFields, false);
                }
            }

            return FindDocuments(collection, filter, sort);
        }

        public static IEnumerable<BsonDocument> FindDocuments(Guid? collectionID, FilterDefinition<BsonDocument> filter)
        {
            return FindDocuments(collectionID, filter, null);
        }
        public static IEnumerable<BsonDocument> FindDocuments(Guid? collectionID, FilterDefinition<BsonDocument> filter, SortDefinition<BsonDocument> sort)
        {
            var collection = GetCollection(collectionID);
            return FindDocuments(collection, filter, sort);
        }

        public static IEnumerable<BsonDocument> FindDocuments(String collectionName, FilterDefinition<BsonDocument> filter)
        {
            return FindDocuments(collectionName, filter, null);
        }
        public static IEnumerable<BsonDocument> FindDocuments(String collectionName, FilterDefinition<BsonDocument> filter, SortDefinition<BsonDocument> sort)
        {
            var collection = GetCollection(collectionName);
            return FindDocuments(collection, filter, sort);
        }

        public static IEnumerable<BsonDocument> FindDocuments(IMongoCollection<BsonDocument> collection, FilterDefinition<BsonDocument> filter)
        {
            return FindDocuments(collection, filter, null);
        }
        public static IEnumerable<BsonDocument> FindDocuments(IMongoCollection<BsonDocument> collection, FilterDefinition<BsonDocument> filter, SortDefinition<BsonDocument> sort)
        {
            var result = collection.Find(filter);

            if (sort != null)
                result = result.Sort(sort);

            using (var cursor = result.ToCursor())
            {
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
                if (name != FormDataConstants.DocIDField)
                    target[name] = source[name];
            }
        }

        public static void UpdateDocument(Guid? collectionID, BsonDocument document)
        {
            var collection = GetCollection(collectionID);
            UpdateDocument(collection, document);
        }
        public static void UpdateDocument(String collectionName, BsonDocument document)
        {
            var collection = GetCollection(collectionName);
            UpdateDocument(collection, document);
        }
        public static void UpdateDocument(IMongoCollection<BsonDocument> collection, BsonDocument document)
        {
            if (collection == null)
                return;

            var keyField = FormDataConstants.DocIDField;
            var keyValue = (Object)null;

            var bsonValue = document[keyField];
            if (bsonValue.IsObjectId)
                keyValue = bsonValue.AsObjectId;
            else
            {
                keyField = FormDataConstants.IDField;

                bsonValue = document[keyField];
                if (bsonValue.IsGuid)
                    keyValue = bsonValue.AsGuid;
                else
                    throw new Exception();
            }

            var filter = Builders<BsonDocument>.Filter.Eq(keyField, keyValue);

            collection.ReplaceOne(filter, document);
        }

        public static void UpdateFields(Guid? ownerID, Guid? recordID, IDictionary<String, Object> fields)
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
        public static void UpdateField(Guid? ownerID, Guid? recordID, String fieldName, Object value)
        {
            var collection = MongoDbUtil.GetCollection(ownerID);

            var update = Builders<BsonDocument>.Update.Set(fieldName, value);
            var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);

            collection.UpdateMany(filter, update);
        }

        public static void ClearCollection(Guid? collectionID)
        {
            var collection = GetCollection(collectionID);
            ClearCollection(collection);
        }
        public static void ClearCollection(String collectionName)
        {
            var collection = GetCollection(collectionName);
            ClearCollection(collection);
        }
        public static void ClearCollection(IMongoCollection<BsonDocument> collection)
        {
            if (collection == null)
                return;

            var filter = Builders<BsonDocument>.Filter.Empty;
            collection.DeleteMany(filter);
        }

        public static IDictionary<Guid, ILookup<String, String>> ExtractQueryFields(IQueryable<BsonDocument> query)
        {
            var result = new Dictionary<Guid, ILookup<String, String>>();

            var methodExp = (MethodCallExpression)query.Expression;

            foreach (var expression in methodExp.Arguments)
            {
                var unaryExp = expression as UnaryExpression;
                if (unaryExp == null)
                {

                    continue;
                }

                var fields = ExtractQueryFields(unaryExp);
                var fieldsLp = fields.ToLookup(n => n.Key, n => n.Value);

                result.Add(Guid.Empty, fieldsLp);
            }

            return result;
        }

        private static IEnumerable<KeyValuePair<String, String>> ExtractQueryFields(UnaryExpression expression)
        {
            var operand = expression.Operand as LambdaExpression;
            if (operand == null)
                yield break;

            var body = operand.Body as BinaryExpression;
            if (body == null || body.Method == null)
                yield break;

            var name = body.Method.Name;

            var argsQuery = from n in PreOrderTraversal(body).OfType<MethodCallExpression>()
                            where n.Method.Name == "get_Item" &&
                                  n.Method.DeclaringType == typeof(BsonValue)
                            from m in n.Arguments
                            let v = GetArgumentValue(m)
                            select v;

            foreach (var item in argsQuery)
                yield return new KeyValuePair<String, String>(name, item);
        }

        private static IEnumerable<Expression> PreOrderTraversal(Expression expression)
        {
            var stack = new Stack<Expression>();
            stack.Push(expression);

            while (stack.Count > 0)
            {
                var n = stack.Pop();
                yield return n;

                var binaryExp = n as BinaryExpression;
                if (binaryExp != null)
                {
                    stack.Push(binaryExp.Left);
                    stack.Push(binaryExp.Right);
                }
            }
        }

        private static String GetArgumentValue(Expression expression)
        {
            var lambda = Expression.Lambda<Func<String>>(expression);
            var compiled = lambda.Compile();
            var result = compiled();

            return result;
        }

        private static bool IsNullOrEmpty(Object value)
        {
            return String.IsNullOrEmpty(Convert.ToString(value));
        }
    }
}