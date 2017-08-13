using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Enums;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.CollectionStructure;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using MongoDB.Bson;
using MongoDB.Driver;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gms.Portal.Web.Utils;
using FieldEntity = Gms.Portal.Web.Entities.FormStructure.FieldEntity;

namespace Gms.Portal.Web.Helpers
{
    public class DataSourceHelper
    {
        private const String TransferDataCacheKey = "@{FormDataControl_TransferData}";
        private const String FindDataRecordCacheKey = "@{FormDataControl_FindDataRecord}";
        private const String CollectionFieldsCacheKey = "@{FormDataControl_CollectionFields}";

        private readonly Guid? _userID;

        private readonly Guid? _parentID;
        private readonly Guid? _childID;

        private readonly bool? _filterByUser;
        private readonly Guid? _collectionID;

        private readonly String _dataSourceID;
        private readonly String _valueExpression;

        private readonly FieldEntity _fieldEntity;
        private readonly FormDataBase _formDataBase;
        private readonly ILookup<String, String> _fields;
        private readonly IDictionary<Guid, ControlEntity> _controls;

        public DataSourceHelper(FieldEntity fieldEntity) : this(null, fieldEntity)
        {
        }
        public DataSourceHelper(Guid? userID, FieldEntity fieldEntity) : this(userID, fieldEntity, null, null)
        {
        }
        public DataSourceHelper(Guid? userID, FieldEntity fieldEntity, FormDataBase formDataBase, IDictionary<Guid, ControlEntity> controls)
        {
            _userID = userID;
            _controls = controls;
            _fieldEntity = fieldEntity;
            _formDataBase = formDataBase;

            _filterByUser = fieldEntity.FilterByUser;
            _dataSourceID = fieldEntity.DataSourceID;
            _valueExpression = fieldEntity.ValueExpression;

            if (RegexUtil.DataSourceParserRx.IsMatch(_dataSourceID))
            {
                var match = RegexUtil.DataSourceParserRx.Match(_dataSourceID);

                _parentID = DataConverter.ToNullableGuid(match.Groups["parentID"].Value);
                _childID = DataConverter.ToNullableGuid(match.Groups["childID"].Value);
            }
            else
            {
                _parentID = DataConverter.ToNullableGuid(_dataSourceID);
            }

            _collectionID = (_childID ?? _parentID);

            _fields = GetCollectionFields();
        }

        public IEnumerable<FormDataBase> FindDataRecords(params Object[] values)
        {
            foreach (var value in values)
            {
                var record = FindDataRecord(value);
                if (record != null)
                    yield return record;
            }
        }

        public FormDataBase FindDataRecord(Object value)
        {
            var strVal = Convert.ToString(value);
            if (String.IsNullOrWhiteSpace(strVal))
                return null;

            var recordKey = CreateRecordKey(value);

            var cache = CommonObjectCache.InitObject(FindDataRecordCacheKey, CommonCacheStore.Request, ConcurrencyHelper.CreateDictionary<String, FormDataBase>);
            lock (cache)
            {
                FormDataBase record;
                if (cache.TryGetValue(recordKey, out record))
                    return record;

                if (_fields == null)
                    return null;

                var dictionaries = TransferDataRecords();

                var expNode = ExpressionParser.GetOrParse(_valueExpression);
                foreach (var dict in dictionaries)
                {
                    Object result;
                    if (!ExpressionEvaluator.TryEval(expNode, n => dict.GetValueOrDefault(n), out result))
                        continue;

                    if (GmsCommonUtil.Compare(result, value) == 0)
                    {
                        cache[recordKey] = dict;
                        return dict;
                    }
                }

                return null;
            }
        }

        public IEnumerable<FormDataBase> LoadDataRecords()
        {
            if (String.IsNullOrWhiteSpace(_dataSourceID))
                return null;

            if (_fields == null)
                return null;

            DiagUtil.Current.StartOrStop("TransferDataRecords");
            var dictionaries = TransferDataRecords();
            DiagUtil.Current.StartOrStop("TransferDataRecords");

            DiagUtil.Current.StartOrStop("FilterDataRecords");
            dictionaries = FilterDataRecords(dictionaries);
            DiagUtil.Current.StartOrStop("FilterDataRecords");

            DiagUtil.Current.StartOrStop("SortDataRecords");
            dictionaries = SortDataRecords(dictionaries);
            DiagUtil.Current.StartOrStop("SortDataRecords");

            return dictionaries;
        }

        public IEnumerable<FormDataBase> SortDataRecords(IEnumerable<FormDataBase> dictionaries)
        {
            var expression = _fieldEntity.DataSourceSortExp;
            if (String.IsNullOrWhiteSpace(expression))
                return dictionaries;

            if (!RegexUtil.SortingFieldsParserRx.IsMatch(expression))
                return dictionaries;

            var fields = new Dictionary<String, SortOrder>();

            var matches = RegexUtil.SortingFieldsParserRx.Matches(expression);
            foreach (Match match in matches)
            {
                var name = match.Groups["name"].Value;
                name = (name ?? String.Empty).Trim();

                var type = match.Groups["type"].Value;
                type = (type ?? String.Empty).Trim();

                var sortOrder = DataConverter.ToNullableEnum<SortOrder>(type, true);

                fields[name] = sortOrder.GetValueOrDefault();
            }

            var comparer = new DictionaryComparer(fields);
            var ordered = dictionaries.OrderBy(n => n, comparer);

            return ordered;
        }

        public IEnumerable<FormDataBase> FilterDataRecords(IEnumerable<FormDataBase> formDataBases)
        {
            if (formDataBases == null)
                return null;

            var filterExp = _fieldEntity.DataSourceFilterExp;
            if (String.IsNullOrWhiteSpace(filterExp))
                return formDataBases;

            if (_fieldEntity.DependentFieldID == null)
            {
                var filteredData = ApplyFilter(formDataBases, filterExp);
                return filteredData;
            }

            if (_formDataBase == null || _controls == null)
                return formDataBases;

            var sourceField = _controls.GetValueOrDefault(_fieldEntity.DependentFieldID.Value) as FieldEntity;
            if (sourceField == null)
                return formDataBases;

            var sourceKey = Convert.ToString(sourceField.ID);
            var sourceValue = _formDataBase[sourceKey];

            if ((sourceField.Type == "ComboBox" || sourceField.Type == "Lookup") &&
                !String.IsNullOrWhiteSpace(sourceField.DataSourceID) &&
                !String.IsNullOrWhiteSpace(sourceField.ValueExpression))
            {
                var sourceHelper = new DataSourceHelper(sourceField);
                //var sourceData = new Dictionary<String, Object>();

                //var sourceRecord = sourceHelper.FindDataRecord(sourceValue);
                //if (sourceRecord != null)
                //{
                //    var prefix = sourceField.Alias;
                //    if (String.IsNullOrWhiteSpace(prefix))
                //        prefix = sourceField.Name;

                //    prefix = ExpressionParser.Escape(prefix);

                //    foreach (var pair in sourceRecord)
                //        sourceData.Add($"{prefix}.{pair.Key}", pair.Value);
                //}

                var dataRecord = sourceHelper.FindDataRecord(sourceValue);
                if (dataRecord != null)
                    formDataBases = ApplyFilter(formDataBases, dataRecord, filterExp).ToList();
            }
            else
            {
                formDataBases = ApplyFilter(formDataBases, sourceValue, filterExp).ToList();
            }

            return formDataBases;
        }

        public IEnumerable<FormDataBase> TransferDataRecords()
        {
            var cache = CommonObjectCache.InitObject(TransferDataCacheKey, CommonCacheStore.Request, ConcurrencyHelper.CreateDictionary<String, IEnumerable<FormDataBase>>);
            lock (cache)
            {
                IEnumerable<FormDataBase> records;
                if (cache.TryGetValue(_dataSourceID, out records))
                    return records;

                var results = GetSourceData().ToList();
                if (results.Count == 0)
                {
                    cache[_dataSourceID] = null;
                    return null;
                }

                cache[_dataSourceID] = results;
                return results;
            }
        }

        public IEnumerable<FormDataBase> ApplyFilter(IEnumerable<FormDataBase> target, String filterExp)
        {
            return ApplyFilter(target, (Object)null, filterExp);
        }
        public IEnumerable<FormDataBase> ApplyFilter(IEnumerable<FormDataBase> target, Object sourceValue, String filterExp)
        {
            var sourceValDict = new Dictionary<String, Object>()
            {
                {"@", sourceValue},
                {"@val", sourceValue},
                {"@value", sourceValue}
            };

            var filterNode = ExpressionParser.GetOrParse(filterExp);
            var expGlobals = new ExpressionGlobalsUtil(_userID, sourceValDict);

            foreach (var targetRecord in target)
            {
                var adpTargetRecord = new Dictionary<String, Object>();
                foreach (var pair in targetRecord)
                    adpTargetRecord[$"${pair.Key}"] = pair.Value;

                expGlobals.AddSource(adpTargetRecord);

                Object result;
                if (ExpressionEvaluator.TryEval(filterNode, expGlobals.Eval, out result))
                {
                    var @bool = DataConverter.ToNullableBool(result);
                    if (@bool.GetValueOrDefault())
                        yield return targetRecord;
                }

                expGlobals.RemoveSource(adpTargetRecord);
            }
        }
        public IEnumerable<FormDataBase> ApplyFilter(IEnumerable<FormDataBase> target, FormDataBase sourceRecord, String filterExp)
        {
            var adpSourceRecord = new Dictionary<String, Object>();
            foreach (var pair in sourceRecord)
                adpSourceRecord[$"@{pair.Key}"] = pair.Value;

            var filterNode = ExpressionParser.GetOrParse(filterExp);

            foreach (var targetRecord in target)
            {
                var adpTargetRecord = new Dictionary<String, Object>();
                foreach (var pair in targetRecord)
                    adpTargetRecord[$"${pair.Key}"] = pair.Value;

                var expGlobals = new ExpressionGlobalsUtil(_userID, adpSourceRecord, adpTargetRecord);

                Object result;
                var flag = ExpressionEvaluator.TryEval(filterNode, expGlobals.Eval, out result);
                if (!flag)
                    continue;

                var @bool = DataConverter.ToNullableBool(result);
                if (@bool.GetValueOrDefault())
                    yield return targetRecord;
            }
        }

        public FormEntity GetFormEntity()
        {
            var session = Hb8Factory.GetCurrentSession();

            var dbEntity = session.Query<GM_Form>().FirstOrDefault(n => n.ID == _parentID);
            if (dbEntity == null)
                return null;

            var converter = new FormEntityModelConverter(session);
            var model = converter.Convert(dbEntity);

            return model.Entity;
        }

        public CollectionEntity GetCollectionEntity()
        {
            var session = Hb8Factory.GetCurrentSession();

            var dbEntity = session.Query<GM_Collection>().FirstOrDefault(n => n.ID == _parentID);
            if (dbEntity == null)
                return null;

            var converter = new CollectionEntityModelConverter(session);
            var model = converter.Convert(dbEntity);

            var entity = model.Entity;
            return entity;
        }

        public ILookup<String, String> GetCollectionFields()
        {
            var dataSourceKey = $"{_parentID}_{_childID}";

            var cache = CommonObjectCache.InitObject(CollectionFieldsCacheKey, CommonCacheStore.Request, ConcurrencyHelper.CreateDictionary<String, ILookup<String, String>>);
            lock (cache)
            {
                ILookup<String, String> fields;
                if (cache.TryGetValue(dataSourceKey, out fields))
                    return fields;

                if (_childID == null)
                {
                    var collEntity = GetCollectionEntity();
                    if (collEntity != null)
                        fields = GetAllFields(collEntity).ToLookup(n => n.Key, n => n.Value);
                }

                if (fields == null)
                {
                    var contentEntity = (ContentEntity)GetFormEntity();
                    if (contentEntity != null)
                    {
                        var contentControls = FormStructureUtil.PreOrderFirstLevelTraversal(contentEntity);
                        if (_childID != null)
                            contentEntity = contentControls.OfType<ContentEntity>().FirstOrDefault(n => n.ID == _childID);

                        var formFields = GetAllFields(contentEntity);
                        fields = formFields.ToLookup(n => n.Key, n => n.Value);
                    }
                }

                if (fields != null)
                    cache[dataSourceKey] = fields;

                return fields;
            }
        }

        private IEnumerable<FormDataBase> GetSourceData()
        {
            var dbCollectionID = _collectionID.GetValueOrDefault();

            return GetCollectionData(dbCollectionID);
        }

        private IEnumerable<FormDataBase> GetCollectionData(Guid dbCollectionID)
        {
            var collection = MongoDbUtil.GetCollection(dbCollectionID);
            if (collection == null)
                yield break;

            var documents = (IEnumerable<BsonDocument>)collection.AsQueryable();

            if (_fields.Contains(FormDataConstants.DateDeletedField))
            {
                var filter = new Dictionary<String, Object>
                {
                    [FormDataConstants.DateDeletedField] = null
                };

                if (_userID != null && _filterByUser.GetValueOrDefault() && _fields.Contains(FormDataConstants.UserIDField))
                {
                    filter[FormDataConstants.UserIDField] = _userID;
                }

                documents = MongoDbUtil.FindDocuments(collection, filter);
            }

            var dicts = BsonDocumentConverter.ConvertToDictionary(documents);

            foreach (var dict in dicts)
            {
                var result = new FormDataBase();
                foreach (var fieldsGrp in _fields)
                {
                    var val = dict.GetValueOrDefault(fieldsGrp.Key);

                    foreach (var name in fieldsGrp)
                        result[name] = val;
                }

                yield return result;
            }
        }

        private IEnumerable<KeyValuePair<String, String>> GetAllFields(ContentEntity contentEntity)
        {
            var contentControls = FormStructureUtil.PreOrderFirstLevelTraversal(contentEntity);

            foreach (var field in FormDataBase.DefaultFields)
                yield return new KeyValuePair<String, String>(field, field);

            foreach (var entity in contentControls)
            {
                var key = Convert.ToString(entity.ID);

                if (!String.IsNullOrWhiteSpace(entity.Name))
                {
                    var value = ExpressionParser.Escape(entity.Name);
                    yield return new KeyValuePair<String, String>(key, value);
                }

                if (!String.IsNullOrWhiteSpace(entity.Alias))
                {
                    var value = ExpressionParser.Escape(entity.Alias);
                    yield return new KeyValuePair<String, String>(key, value);
                }
            }
        }

        private IEnumerable<KeyValuePair<String, String>> GetAllFields(CollectionEntity contentEntity)
        {
            yield return new KeyValuePair<String, String>("ID", "ID");

            foreach (var entity in contentEntity.Fields)
            {
                var key = Convert.ToString(entity.ID);

                if (!String.IsNullOrWhiteSpace(entity.Name))
                {
                    var value = ExpressionParser.Escape(entity.Name);
                    yield return new KeyValuePair<String, String>(key, value);
                }
            }
        }

        private String CreateRecordKey(Object value)
        {
            if (!(value is String) && value is IEnumerable)
            {
                var collection = (IEnumerable)value;
                var items = collection.Cast<Object>();
                var keys = String.Join("_", items);

                var recordKey = $"{_dataSourceID}_{_valueExpression}_{keys}";
                return recordKey;
            }
            else
            {
                var recordKey = $"{_dataSourceID}_{_valueExpression}_{value}";
                return recordKey;
            }
        }
    }
}