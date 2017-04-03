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
            if (source == null)
                return null;

            var list = IAsyncCursorSourceExtensions.ToList(source);
            return ConvertToDictionary(list);
        }
        public static IEnumerable<IDictionary<String, Object>> ConvertToDictionary(IEnumerable<BsonDocument> source)
        {
            if (source == null)
                yield break;

            foreach (var document in source)
            {
                var formDataUnit = ConvertToDictionary(document);
                yield return formDataUnit;
            }
        }
        public static IDictionary<String, Object> ConvertToDictionary(BsonDocument source)
        {
            if (source == null)
                return null;

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
            if (source == null)
                return null;

            var list = source.ToList();
            return ConvertToBsonDocument(list);
        }
        public static IEnumerable<BsonDocument> ConvertToBsonDocument(IEnumerable<IDictionary<String, Object>> source)
        {
            if (source == null)
                yield break;

            foreach (var document in source)
            {
                var formDataUnit = ConvertToBsonDocument(document);
                yield return formDataUnit;
            }
        }
        public static BsonDocument ConvertToBsonDocument(IDictionary<String, Object> source)
        {
            if (source == null)
                return null;

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
            if (source == null)
                return null;

            var list = IAsyncCursorSourceExtensions.ToList(source);
            return ConvertToFormDataUnit(list);
        }

        public static IEnumerable<FormDataUnit> ConvertToFormDataUnit(IEnumerable<BsonDocument> source)
        {
            if (source == null)
                yield break;

            foreach (var document in source)
            {
                var formDataUnit = ConvertToFormDataUnit(document);
                yield return formDataUnit;
            }
        }

        public static FormDataUnit ConvertToFormDataUnit(BsonDocument source)
        {
            if (source == null)
                return null;

            var recordID = source[FormDataConstants.IDField].AsNullableGuid;
            var formDataUnit = new FormDataUnit();

            foreach (var element in source.Elements)
            {
                var bsonValue = element.Value;

                if (bsonValue.IsBsonDocument)
                {
                    var subDoc = bsonValue.AsBsonDocument;

                    var docType = TryGetValue<String>(subDoc, FormDataConstants.DocTypeField);
                    if (docType == FormDataConstants.BinaryDocType)
                    {
                        var fileName = subDoc[FormDataConstants.FileNameField].AsString;

                        var binaryData = subDoc[FormDataConstants.FileBytesField].AsBsonBinaryData;
                        var fileBytes = (binaryData != null ? binaryData.Bytes : null);

                        var formBinary = new FormDataBinary(fileName, fileBytes);

                        formDataUnit[element.Name] = formBinary;
                    }
                    else if (docType == FormDataConstants.ReferenceDocType || String.IsNullOrWhiteSpace(docType))
                    {
                        var subFormID = subDoc[FormDataConstants.FormIDField].AsNullableGuid;
                        var subOwnerID = subDoc[FormDataConstants.OwnerIDField].AsNullableGuid;
                        var subParentID = recordID;

                        var listRef = new FormDataListRef(subFormID, subOwnerID, subParentID);
                        formDataUnit[element.Name] = listRef;
                    }
                }
                else if (element.Name == FormDataConstants.PrivacyField)
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
            if (source == null)
                return null;

            var list = source.ToList();
            return ConvertToBsonDocument(list);
        }

        public static IEnumerable<BsonDocument> ConvertToBsonDocument(IEnumerable<FormDataUnit> source)
        {
            if (source == null)
                yield break;

            foreach (var document in source)
            {
                var formDataUnit = ConvertToBsonDocument(document);
                yield return formDataUnit;
            }
        }

        public static BsonDocument ConvertToBsonDocument(FormDataUnit source)
        {
            if (source == null)
                return null;

            var document = new BsonDocument();

            foreach (var pair in source)
            {
                if (pair.Value is FormDataListRef)
                {
                    var listRef = (FormDataListRef)pair.Value;

                    var listRefDoc = new BsonDocument
                    {
                        {FormDataConstants.DocTypeField, FormDataConstants.ReferenceDocType },

                        {FormDataConstants.FormIDField, listRef.FormID },
                        {FormDataConstants.OwnerIDField, listRef.OwnerID },
                        {FormDataConstants.ParentIDField, source.ID }
                    };

                    document[pair.Key] = listRefDoc;
                }
                else if (pair.Value is FormDataBinary)
                {
                    var binary = (FormDataBinary)pair.Value;

                    var binaryDoc = new BsonDocument
                    {
                        {FormDataConstants.DocTypeField, FormDataConstants.BinaryDocType },
                        {FormDataConstants.FileNameField, binary.FileName },
                        {FormDataConstants.FileBytesField, BsonBinaryData.Create(binary.FileBytes) },
                    };

                    document[pair.Key] = binaryDoc;
                }
                else if (pair.Key == FormDataConstants.PrivacyField)
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

        private static TValue TryGetValue<TValue>(BsonDocument document, String name)
        {
            BsonValue bsonValue;
            if (!document.TryGetValue(name, out bsonValue))
                return default(TValue);

            return (TValue)BsonTypeMapper.MapToDotNetValue(bsonValue);
        }
    }
}