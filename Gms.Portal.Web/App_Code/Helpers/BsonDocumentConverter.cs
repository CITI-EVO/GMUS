using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gms.Portal.Web.Helpers
{
    public static class BsonDocumentConverter
    {
        public static BsonDocument CreateClone(BsonDocument source)
        {
            var dict = ConvertToDictionary(source);

            dict[FormDataConstants.IDField] = Guid.NewGuid();
            dict[FormDataConstants.DocIDField] = ObjectId.GenerateNewId();
            dict[FormDataConstants.DateCreatedField] = DateTime.Now;
            dict[FormDataConstants.DateChangedField] = (DateTime?)null;

            var newBsonDoc = ConvertToBsonDocument(dict);
            return newBsonDoc;
        }

        public static String ComputeHashCode(BsonDocument source)
        {
            return ComputeHashCode(source, source.Names);
        }
        public static String ComputeHashCode(BsonDocument source, IEnumerable<String> fields)
        {
            var values = new List<String>(source.ElementCount);

            foreach (var fieldName in fields)
            {
                var bsonValue = source[fieldName];
                var objValue = BsonTypeMapper.MapToDotNetValue(bsonValue);
                var strValue = (Convert.ToString(objValue) ?? String.Empty);

                values.Add(strValue);
            }

            var hashCode = String.Join(",", values).ComputeMd5();
            return hashCode;
        }

        public static String ComputeHashCode(IDictionary<String, Object> source)
        {
            return ComputeHashCode(source, source.Keys);
        }
        public static String ComputeHashCode(IDictionary<String, Object> source, IEnumerable<String> fields)
        {
            var values = new List<String>(source.Count);

            foreach (var fieldName in fields)
            {
                var objValue = source[fieldName];
                var strValue = (Convert.ToString(objValue) ?? String.Empty);

                values.Add(strValue);
            }

            var hashCode = String.Join(",", values).ComputeMd5();
            return hashCode;
        }

        public static ILookup<String, BsonDocument> ComputeHashCodes(IEnumerable<BsonDocument> source)
        {
            var docsLp = source.ToLookup(ComputeHashCode);
            return docsLp;
        }
        public static ILookup<String, BsonDocument> ComputeHashCodes(IEnumerable<BsonDocument> source, IEnumerable<String> fields)
        {
            var docsLp = source.ToLookup(n => ComputeHashCode(n, fields));
            return docsLp;
        }

        public static ILookup<String, IDictionary<String, Object>> ComputeHashCodes(IEnumerable<IDictionary<String, Object>> source)
        {
            var docsLp = source.ToLookup(ComputeHashCode);
            return docsLp;
        }
        public static ILookup<String, IDictionary<String, Object>> ComputeHashCodes(IEnumerable<IDictionary<String, Object>> source, IEnumerable<String> fields)
        {
            var docsLp = source.ToLookup(n => ComputeHashCode(n, fields));
            return docsLp;
        }

        public static BsonArray ConvertToBsonArray(BsonValue bsonValue)
        {
            BsonArray array;
            if (TryConvertToBsonArray(bsonValue, out array))
                return array;

            return null;
        }

        public static bool TryConvertToBsonArray(BsonValue bsonValue, out BsonArray array)
        {
            array = null;

            if (bsonValue.IsBsonArray)
            {
                array = bsonValue.AsBsonArray;
                return true;
            }

            if (bsonValue.IsBsonDocument)
            {
                var arrayDoc = bsonValue.AsBsonDocument;
                if (arrayDoc.Contains("_t") && arrayDoc.Contains("_v"))
                {
                    array = arrayDoc["_v"].AsBsonArray;
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<FormStatusUnit> ConvertToFormStatuses(IEnumerable<BsonValue> bsonValues)
        {
            var documents = (from n in bsonValues
                             where !n.IsBsonNull &&
                                    n.IsBsonDocument
                             select n.AsBsonDocument);

            foreach (var document in documents)
            {
                var dict = ConvertToDictionary(document);

                var unit = new FormStatusUnit
                {
                    Step = DataConverter.ToNullableInt(dict.GetValueOrDefault(FormDataConstants.StepField)),
                    UserID = DataConverter.ToNullableGuid(dict.GetValueOrDefault(FormDataConstants.UserIDField)),
                    StatusID = DataConverter.ToNullableGuid(dict.GetValueOrDefault(FormDataConstants.StatusIDField)),
                    DateOfStatus = DataConverter.ToNullableDateTime(dict.GetValueOrDefault(FormDataConstants.StatusChangeDateField)),
                };

                yield return unit;
            }
        }

        public static IEnumerable<BsonDocument> ConvertToFormStatuses(IEnumerable<FormStatusUnit> formStatuses)
        {
            foreach (var item in formStatuses)
            {
                if (item == null)
                    continue;

                var doc = new BsonDocument
                {
                    [FormDataConstants.StepField] = BsonValue.Create(item.Step),
                    [FormDataConstants.UserIDField] = BsonValue.Create(item.UserID),
                    [FormDataConstants.StatusIDField] = BsonValue.Create(item.StatusID),
                    [FormDataConstants.StatusChangeDateField] = BsonValue.Create(item.DateOfStatus)
                };

                yield return doc;
            }
        }

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
                    var stringsQuery = (from n in bsonValue.AsBsonArray
                                        where !n.IsBsonNull && n.IsString
                                        select (Object)n.AsString);

                    var documentsQuery = (from n in bsonValue.AsBsonArray
                                          where !n.IsBsonNull && n.IsBsonDocument
                                          let d = ConvertToDictionary(n.AsBsonDocument)
                                          select (Object)d);

                    var valuesQuery = stringsQuery.Union(documentsQuery);
                    dictionary[element.Name] = valuesQuery.ToArray();
                }
                else if (bsonValue.IsValidDateTime)
                    dictionary[element.Name] = bsonValue.ToNullableLocalTime();
                else if (bsonValue.IsObjectId)
                    dictionary[element.Name] = bsonValue.AsNullableObjectId;
                else
                    dictionary[element.Name] = BsonTypeMapper.MapToDotNetValue(bsonValue);
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
                var value = (ReferenceEquals(pair.Value, FormNoData.Value) ? null : pair.Value);

                if (value is IDictionary<String, Object>)
                {
                    var subDict = (IDictionary<String, Object>)value;
                    var subDoc = ConvertToBsonDocument(subDict);

                    document[pair.Key] = subDoc;
                }
                else
                {
                    document[pair.Key] = BsonValue.Create(value);
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
            var formData = new FormDataUnit();

            foreach (var element in source.Elements)
            {
                var bsonValue = element.Value;

                if (element.Name == FormDataConstants.ReviewFields ||
                    element.Name == FormDataConstants.PrivacyFields)
                {
                    if (!bsonValue.IsBsonNull)
                    {
                        var bsonArray = ConvertToBsonArray(bsonValue);
                        if (bsonArray != null)
                        {
                            var query = (from n in bsonArray
                                         where !n.IsBsonNull
                                         select n.AsString);

                            formData[element.Name] = query.ToHashSet();
                        }
                    }
                }
                else if (element.Name == FormDataConstants.UserStatusesFields)
                {
                    if (!bsonValue.IsBsonNull)
                    {
                        var bsonArray = ConvertToBsonArray(bsonValue);
                        if (bsonArray != null)
                        {
                            var formStatuses = ConvertToFormStatuses(bsonArray);
                            formData[element.Name] = formStatuses.ToList();
                        }
                    }
                }
                else if (bsonValue.IsBsonArray)
                {
                    var query = (from n in bsonValue.AsBsonArray
                                 let val = BsonTypeMapper.MapToDotNetValue(n)
                                 select val);

                    formData[element.Name] = query.ToArray();
                }
                else if (bsonValue.IsObjectId)
                    formData[element.Name] = bsonValue.AsNullableObjectId;
                else if (bsonValue.IsValidDateTime)
                    formData[element.Name] = bsonValue.ToNullableLocalTime();
                else if (bsonValue.IsBsonDocument)
                {
                    var subDoc = bsonValue.AsBsonDocument;

                    var docType = TryGetValue<String>(subDoc, FormDataConstants.DocTypeField);
                    if (docType == FormDataConstants.BinaryDocType)
                    {
                        var fileName = subDoc[FormDataConstants.FileNameField].AsString;

                        var binaryData = subDoc[FormDataConstants.FileBytesField].AsBsonBinaryData;
                        var fileBytes = (binaryData != null ? binaryData.Bytes : null);

                        var formBinary = new FormDataBinary(fileName, fileBytes);
                        formData[element.Name] = formBinary;
                    }
                    else if (docType == FormDataConstants.ReferenceDocType || String.IsNullOrWhiteSpace(docType))
                    {
                        var subFormID = subDoc[FormDataConstants.FormIDField].AsNullableGuid;
                        var subOwnerID = subDoc[FormDataConstants.OwnerIDField].AsNullableGuid;
                        var subParentID = recordID;

                        var listRef = new FormDataListRef(subFormID, subOwnerID, subParentID);
                        formData[element.Name] = listRef;
                    }
                }
                else
                    formData[element.Name] = BsonTypeMapper.MapToDotNetValue(bsonValue);
            }

            return formData;
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
                var value = (ReferenceEquals(pair.Value, FormNoData.Value) ? null : pair.Value);

                if (value is FormDataListRef)
                {
                    var listRef = (FormDataListRef)value;

                    var listRefDoc = new BsonDocument
                    {
                        {FormDataConstants.DocTypeField, FormDataConstants.ReferenceDocType },

                        {FormDataConstants.FormIDField, listRef.FormID },
                        {FormDataConstants.OwnerIDField, listRef.OwnerID },
                        {FormDataConstants.ParentIDField, source.ID }
                    };

                    document[pair.Key] = listRefDoc;
                }
                else if (value is FormDataBinary)
                {
                    var binary = (FormDataBinary)value;

                    var binaryDoc = new BsonDocument
                    {
                        {FormDataConstants.DocTypeField, FormDataConstants.BinaryDocType },
                        {FormDataConstants.FileNameField, binary.FileName },
                        {FormDataConstants.FileBytesField, BsonBinaryData.Create(binary.FileBytes) },
                    };

                    document[pair.Key] = binaryDoc;
                }
                else if (pair.Key == FormDataConstants.ReviewFields ||
                         pair.Key == FormDataConstants.PrivacyFields)
                {
                    var array = (BsonValue)BsonNull.Value;
                    if (value != null)
                        array = BsonArray.Create(value);

                    document[pair.Key] = array;
                }
                else if (pair.Key == FormDataConstants.UserStatusesFields)
                {
                    var array = (BsonValue)BsonNull.Value;

                    var list = value as IList<FormStatusUnit>;
                    if (list != null)
                    {
                        var docs = ConvertToFormStatuses(list);
                        array = new BsonArray(docs);
                    }

                    document[pair.Key] = array;
                }
                else if (!(value is String) && value is IEnumerable)
                {
                    var collection = (IEnumerable)value;
                    document[pair.Key] = BsonArray.Create(collection);
                }
                else
                {
                    document[pair.Key] = BsonValue.Create(value);
                }
            }

            return document;
        }

        public static IDictionary<String, Object> TransferToNamedContainer(IDictionary<String, Object> source, ContentEntity entity)
        {
            if (source == null || entity == null)
                return null;

            var controls = FormStructureUtil.PreOrderFirstLevelTraversal(entity).ToDictionary(n => n.ID);

            var result = new Dictionary<String, Object>();

            foreach (var pair in source)
            {
                if (FormDataBase.DefaultFields.Contains(pair.Key))
                    result[pair.Key] = pair.Value;
                else
                {
                    var fieldID = DataConverter.ToNullableGuid(pair.Key);
                    if (fieldID == null)
                        continue;

                    var control = controls.GetValueOrDefault(fieldID.Value);
                    if (control == null)
                        continue;

                    result[control.Name] = pair.Value;
                }
            }

            return result;
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