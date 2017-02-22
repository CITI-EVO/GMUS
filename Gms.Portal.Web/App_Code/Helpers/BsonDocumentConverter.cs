using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Entities.DataContainer;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gms.Portal.Web.Helpers
{
    public static class BsonDocumentConverter
    {
        public static IEnumerable<IDictionary<String, Object>> ConvertToDictionary(IMongoQueryable<BsonDocument> source)
        {
            var list = IAsyncCursorSourceExtensions.ToList(source);
            return ConvertToDictionary(list);
        }
        public static IEnumerable<IDictionary<String, Object>> ConvertToDictionary(IEnumerable<BsonDocument> source)
        {
            foreach (var document in source)
            {
                var formDataUnit = ConvertToDictionary(document);
                yield return formDataUnit;
            }
        }
        public static IDictionary<String, Object> ConvertToDictionary(BsonDocument source)
        {
            var dictionary = new Dictionary<String, Object>();

            foreach (var element in source.Elements)
            {
                var bsonValue = element.Value;

                if (bsonValue.IsBsonDocument)
                {
                    var subDoc = bsonValue.AsBsonDocument;
                    var dictDict = ConvertToDictionary(subDoc);

                    dictionary[element.Name] = dictDict;
                }
                else if (bsonValue.IsBsonArray)
                {
                    var query = (from n in bsonValue.AsBsonArray
                                 where !n.IsBsonNull
                                 select n.AsString);

                    dictionary[element.Name] = query.ToArray();
                }
                else if (bsonValue.IsValidDateTime)
                {
                    dictionary[element.Name] = bsonValue.ToNullableLocalTime();
                }
                else if (bsonValue.IsObjectId)
                {
                    dictionary[element.Name] = bsonValue.AsNullableObjectId;
                }
                else
                {
                    dictionary[element.Name] = BsonTypeMapper.MapToDotNetValue(bsonValue);
                }
            }

            return dictionary;
        }

        public static IEnumerable<BsonDocument> ConvertToBsonDocument(IQueryable<IDictionary<String, Object>> source)
        {
            var list = source.ToList();
            return ConvertToBsonDocument(list);
        }
        public static IEnumerable<BsonDocument> ConvertToBsonDocument(IEnumerable<IDictionary<String, Object>> source)
        {
            foreach (var document in source)
            {
                var formDataUnit = ConvertToBsonDocument(document);
                yield return formDataUnit;
            }
        }
        public static BsonDocument ConvertToBsonDocument(IDictionary<String, Object> source)
        {
            var document = new BsonDocument();

            foreach (var pair in source)
            {
                if (pair.Value is IDictionary<String, Object>)
                {
                    var subDict = (IDictionary<String, Object>)pair.Value;
                    var subDoc = ConvertToBsonDocument(subDict);

                    document[pair.Key] = subDoc;
                }
                else
                {
                    document[pair.Key] = BsonValue.Create(pair.Value);
                }
            }

            return document;
        }

        public static IEnumerable<FormDataUnit> ConvertToFormDataUnit(IMongoQueryable<BsonDocument> source)
        {
            var list = IAsyncCursorSourceExtensions.ToList(source);
            return ConvertToFormDataUnit(list);
        }

        public static IEnumerable<FormDataUnit> ConvertToFormDataUnit(IEnumerable<BsonDocument> source)
        {
            foreach (var document in source)
            {
                var formDataUnit = ConvertToFormDataUnit(document);
                yield return formDataUnit;
            }
        }

        public static FormDataUnit ConvertToFormDataUnit(BsonDocument source)
        {
            var recordID = source[FormDataUnit.IDField].AsNullableGuid;
            var formDataUnit = new FormDataUnit();

            foreach (var element in source.Elements)
            {
                var bsonValue = element.Value;

                if (bsonValue.IsBsonDocument)
                {
                    var listRefDoc = bsonValue.AsBsonDocument;

                    var subFormID = listRefDoc[FormDataUnit.FormIDField].AsNullableGuid;
                    var subOwnerID = listRefDoc[FormDataUnit.OwnerIDField].AsNullableGuid;
                    var subParentID = recordID;

                    var listRef = new FormDataListRef(subFormID, subOwnerID, subParentID);
                    formDataUnit[element.Name] = listRef;
                }
                else if (element.Name == FormDataUnit.PrivacyField)
                {
                    if (!bsonValue.IsBsonNull)
                    {
                        var query = (from n in bsonValue.AsBsonArray
                                     where !n.IsBsonNull
                                     select n.AsString);

                        formDataUnit[element.Name] = query.ToHashSet();
                    }
                }
                else if (bsonValue.IsBsonArray)
                {
                    var query = (from n in bsonValue.AsBsonArray
                                 let val = BsonTypeMapper.MapToDotNetValue(n)
                                 select val);

                    formDataUnit[element.Name] = query.ToArray();
                }
                else if (bsonValue.IsObjectId)
                {
                    formDataUnit[element.Name] = bsonValue.AsNullableObjectId;
                }
                else if (bsonValue.IsValidDateTime)
                {
                    formDataUnit[element.Name] = bsonValue.ToNullableLocalTime();
                }
                else
                {
                    formDataUnit[element.Name] = BsonTypeMapper.MapToDotNetValue(bsonValue);
                }
            }

            return formDataUnit;
        }

        public static IEnumerable<BsonDocument> ConvertToBsonDocument(IQueryable<FormDataUnit> source)
        {
            var list = source.ToList();
            return ConvertToBsonDocument(list);
        }

        public static IEnumerable<BsonDocument> ConvertToBsonDocument(IEnumerable<FormDataUnit> source)
        {
            foreach (var document in source)
            {
                var formDataUnit = ConvertToBsonDocument(document);
                yield return formDataUnit;
            }
        }

        public static BsonDocument ConvertToBsonDocument(FormDataUnit source)
        {
            var document = new BsonDocument();

            foreach (var pair in source)
            {
                if (pair.Value is FormDataListRef)
                {
                    var listRef = (FormDataListRef)pair.Value;

                    var listRefDoc = new BsonDocument
                    {
                        {"FormID", listRef.FormID },
                        {"OwnerID", listRef.OwnerID },
                        {"ParentID", source.ID }
                    };

                    document[pair.Key] = listRefDoc;
                }
                else if (pair.Key == FormDataUnit.PrivacyField)
                {
                    var privacyDoc = BsonArray.Create(pair.Value);
                    document[pair.Key] = privacyDoc;
                }
                else
                {
                    document[pair.Key] = BsonValue.Create(pair.Value);
                }
            }

            return document;
        }
    }
}