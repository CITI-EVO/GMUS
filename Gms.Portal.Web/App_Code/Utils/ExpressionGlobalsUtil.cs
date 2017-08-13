using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using NHibernate.Linq;

namespace Gms.Portal.Web.Utils
{
    public class ExpressionGlobalsUtil
    {
        private const String formsIDCache = "@{formsIDCache}";
        private const String formModelsCache = "@{formModelsCache}";
        private const String formFieldsCache = "@{formFieldsCache}";

        private readonly Guid? _userID;
        private readonly ContentEntity _entity;

        private readonly IEnumerable<ControlEntity> _fields;
        private readonly ILookup<String, ControlEntity> _fieldsLp;
        private readonly IDictionary<Guid?, ControlEntity> _fieldsMap;

        private readonly IDictionary<String, String> _associations;
        private readonly ISet<IDictionary<String, Object>> _sources;

        private readonly IDictionary<String, Guid?> _formsIDCache;
        private readonly IDictionary<String, FormModel> _formModelsCache;
        private readonly IDictionary<Guid?, ILookup<String, ControlEntity>> _formFieldsCache;

        public ExpressionGlobalsUtil(params IDictionary<String, Object>[] sources)
            : this((Guid?)null, sources)
        {
        }
        public ExpressionGlobalsUtil(ContentEntity entity, params IDictionary<String, Object>[] sources)
            : this(null, entity, (IEnumerable<IDictionary<String, Object>>)sources)
        {
        }
        public ExpressionGlobalsUtil(IEnumerable<ControlEntity> fields, params IDictionary<String, Object>[] sources)
            : this(null, fields, (IEnumerable<IDictionary<String, Object>>)sources)
        {
        }

        public ExpressionGlobalsUtil(IEnumerable<IDictionary<String, Object>> sources)
            : this(null, (ContentEntity)null, sources)
        {
        }
        public ExpressionGlobalsUtil(ContentEntity entity, IEnumerable<IDictionary<String, Object>> sources)
            : this(null, entity, sources)
        {
        }

        public ExpressionGlobalsUtil(IEnumerable<ControlEntity> fields, IEnumerable<IDictionary<String, Object>> sources)
            : this(null, fields, sources)

        {
        }

        public ExpressionGlobalsUtil(Guid? userID, params IDictionary<String, Object>[] sources)
            : this(userID, (ContentEntity)null, (IEnumerable<IDictionary<String, Object>>)sources)
        {
        }
        public ExpressionGlobalsUtil(Guid? userID, ContentEntity entity, params IDictionary<String, Object>[] sources)
            : this(userID, entity, (IEnumerable<IDictionary<String, Object>>)sources)
        {
        }
        public ExpressionGlobalsUtil(Guid? userID, IEnumerable<ControlEntity> fields, params IDictionary<String, Object>[] sources)
            : this(userID, fields, (IEnumerable<IDictionary<String, Object>>)sources)
        {
        }

        public ExpressionGlobalsUtil(Guid? userID, IEnumerable<IDictionary<String, Object>> sources)
            : this(userID, (ContentEntity)null, sources)
        {
        }

        public ExpressionGlobalsUtil(Guid? userID, ContentEntity entity, IEnumerable<IDictionary<String, Object>> sources)
            : this(userID, entity, FormStructureUtil.PreOrderTraversal(entity), sources)
        {
        }

        public ExpressionGlobalsUtil(Guid? userID, IEnumerable<ControlEntity> fields, IEnumerable<IDictionary<String, Object>> sources)
            : this(userID, null, fields, sources)
        {
        }

        private ExpressionGlobalsUtil(Guid? userID, ContentEntity entity, IEnumerable<ControlEntity> fields, IEnumerable<IDictionary<String, Object>> sources)
        {
            _userID = userID;
            _fields = fields;
            _entity = entity;

            _sources = sources.ToHashSet();

            _fieldsLp = GetControlsLp(_fields);
            _fieldsMap = _fields.ToDictionary(n => (Guid?)n.ID);

            _associations = new Dictionary<String, String>();

            _formsIDCache = CommonObjectCache.InitObject(formsIDCache, CommonCacheStore.Request, () => new Dictionary<String, Guid?>());
            _formModelsCache = CommonObjectCache.InitObject(formModelsCache, CommonCacheStore.Request, () => new Dictionary<String, FormModel>());
            _formFieldsCache = CommonObjectCache.InitObject(formFieldsCache, CommonCacheStore.Request, () => new Dictionary<Guid?, ILookup<String, ControlEntity>>());
        }

        public Object this[String fieldKey]
        {
            get { return Eval(fieldKey); }
        }

        public Object Eval(String fieldKey)
        {
            Object value;
            if (TryGetValue(_sources, fieldKey, out value))
                return value;

            if (RegexUtil.DataCollFuncParserRx.IsMatch(fieldKey))
            {
                var match = RegexUtil.DataCollFuncParserRx.Match(fieldKey);

                var form = match.Groups["form"].Value;
                var coll = match.Groups["coll"].Value;
                var field = match.Groups["field"].Value;

                if (String.IsNullOrWhiteSpace(field))
                    value = GetValue(form, coll);
                else
                    value = GetValue(form, coll, field);

                return value;
            }

            var escapeKey = ExpressionParser.Escape(fieldKey);

            if (!FormDataBase.DefaultFields.Contains(escapeKey))
            {
                var fields = _fieldsLp[escapeKey];

                var fieldEntity = GetCorrectControl(escapeKey, fields);
                if (fieldEntity != null)
                    fieldKey = Convert.ToString(fieldEntity.ID);
            }

            value = GetValue(_sources, fieldKey);
            return value;
        }

        public void AddSource(IDictionary<String, Object> source)
        {
            _sources.Add(source);
        }
        public void RemoveSource(IDictionary<String, Object> source)
        {
            _sources.Remove(source);
        }
        public bool ContainsSource(IDictionary<String, Object> source)
        {
            return _sources.Contains(source);
        }
        public IEnumerable<IDictionary<String, Object>> GetSources()
        {
            foreach (var source in _sources)
                yield return source;
        }

        public void ClearSources()
        {
            _sources.Clear();
        }

        public void SetAssociation(String source, String target)
        {
            _associations[source] = target;
        }
        public void RemoveAssociation(String source)
        {
            _associations.Remove(source);
        }
        public bool ContainsAssociation(String source)
        {
            return _associations.ContainsKey(source);
        }
        public void ClearAssociations()
        {
            _associations.Clear();
        }

        private bool TryGetGlobal(String fieldKey, out Object value)
        {
            value = null;

            if (String.IsNullOrWhiteSpace(fieldKey))
                return false;

            switch (fieldKey.ToLower())
            {
                case "@lang":
                    value = LanguageUtil.GetLanguage();
                    return true;
                case "@islogged":
                    value = UmUtil.Instance.IsLogged;
                    return true;
                case "@is{user}":
                    value = UmUtil.Instance.HasAccess("Submit");
                    return true;
                case "@is{admin}":
                    value = UmUtil.Instance.HasAccess("Admin");
                    return true;
                case "@is{org}":
                    value = UmUtil.Instance.HasAccess("Org");
                    return true;
                case "@is{geocitizen}":
                    value = UmUtil.Instance.HasAccess("Geocitizen");
                    return true;
                case "@is{foreinger}":
                    value = UmUtil.Instance.HasAccess("Foreinger");
                    return true;
                case "@is{sa}":
                    value = UmUtil.Instance.CurrentUser.IsSuperAdmin;
                    return true;
                case "@userlogin":
                    value = UmUtil.Instance.CurrentUser.LoginName;
                    return true;
                case "@useremail":
                    value = UmUtil.Instance.CurrentUser.Email;
                    return true;
                case "@userfirstname":
                    value = UmUtil.Instance.CurrentUser.FirstName;
                    return true;
                case "@userlastname":
                    value = UmUtil.Instance.CurrentUser.LastName;
                    return true;
                case "@userid":
                    value = UmUtil.Instance.CurrentUser.ID;
                    return true;
                case "@formid":
                    value = HttpServerUtil.RequestUrl["FormID"];
                    return true;
                case "@formname":
                    value = LanguageUtil.GetLanguage();
                    return true;
            }

            return false;
        }

        private Object GetValue(String formName, String fieldName)
        {
            var escapeFormName = ExpressionParser.Escape(formName);
            var escapeFieldName = ExpressionParser.Escape(fieldName);

            var sources = _sources;
            if (IsCurrentEntityField(formName))
            {
                if (!FormDataBase.DefaultFields.Contains(escapeFieldName))
                {
                    var fields = _fieldsLp[escapeFieldName];

                    var fieldEntity = GetCorrectControl(escapeFieldName, fields);
                    if (fieldEntity != null)
                        escapeFieldName = Convert.ToString(fieldEntity.ID);
                }
            }
            else
            {
                var formModel = GetFormModel(escapeFormName);
                if (formModel == null)
                    return null;

                sources = GetOtherFormDatas(formModel).ToHashSet();

                if (!FormDataBase.DefaultFields.Contains(escapeFieldName))
                {
                    var fieldEntity = GetOtherFormField(formModel, escapeFieldName);
                    if (fieldEntity == null)
                        return null;

                    escapeFieldName = Convert.ToString(fieldEntity.ID);
                }
            }

            var value = GetValue(sources, escapeFieldName);
            return value;
        }
        private Object GetValue(String formName, String collName, String fieldName)
        {
            var escapeFormName = ExpressionParser.Escape(formName);
            var escapeCollName = ExpressionParser.Escape(collName);
            var escapeFieldName = ExpressionParser.Escape(fieldName);

            var fields = _fieldsLp;

            var sources = _sources;
            if (escapeFormName != "@")
            {
                var formModel = GetFormModel(escapeFormName);
                if (formModel == null)
                    return null;

                sources = GetOtherFormDatas(formModel).ToHashSet();
                fields = GetOtherFormFields(formModel);
            }

            var collections = fields[escapeCollName];

            var collEntity = GetCorrectControl(escapeCollName, collections);
            if (collEntity == null)
                throw new Exception($"Unable to find collection '{escapeCollName}'");

            var formGridData = GetValue(sources, Convert.ToString(collEntity.ID));
            if (formGridData is IEnumerable<FormDataBase>)
            {
                var formDataList = (IEnumerable<FormDataBase>)formGridData;

                var subControls = FormStructureUtil.PreOrderTraversal(collEntity);

                var fieldQuery = (from n in subControls
                                  where ExpressionParser.Escape(n.Name) == escapeFieldName ||
                                        ExpressionParser.Escape(n.Alias) == escapeFieldName
                                  select n);

                var fieldKey = escapeFieldName;
                var fieldAlias = escapeFieldName;
                var fieldEntity = GetCorrectControl(escapeFieldName, fieldQuery);

                if (fieldEntity != null)
                {
                    fieldKey = Convert.ToString(fieldEntity.ID);
                    fieldAlias = ExpressionParser.Escape(fieldEntity.Alias);
                }
                else if (!FormDataBase.DefaultFields.Contains(fieldKey))
                {
                    throw new Exception($"Unable to find field '{escapeFieldName}' of collection {escapeCollName}");
                }

                var valuesQuery = (from n in formDataList
                                   let v = GetValue(n, fieldKey, escapeFieldName, fieldAlias)
                                   select v);

                var values = valuesQuery.ToArray();
                return values;
            }

            return null;
        }

        private Object GetValue(IDictionary<String, Object> source, params String[] keys)
        {
            var @set = new HashSet<String>();

            foreach (var key in keys)
            {
                if (!@set.Add(key))
                    continue;

                Object val;
                if (source.TryGetValue(key, out val))
                    return val;
            }

            return null;
        }

        private Object GetValue(IEnumerable<IDictionary<String, Object>> sources, String fieldKey)
        {
            Object val;
            if (TryGetValue(sources, fieldKey, out val))
                return val;

            return null;
        }
        private bool TryGetValue(IEnumerable<IDictionary<String, Object>> sources, String fieldKey, out Object value)
        {
            String realName;
            if (_associations.TryGetValue(fieldKey, out realName))
                fieldKey = realName;

            if (TryGetGlobal(fieldKey, out value))
                return true;

            foreach (var source in sources)
            {
                if (source != null && source.TryGetValue(fieldKey, out value))
                {
                    var listRef = value as FormDataListRef;
                    if (listRef != null)
                        value = Transform(listRef);

                    var binary = value as FormDataBinary;
                    if (binary != null)
                        value = $"{binary.FileName}${GetBinarySize(binary)}";

                    return true;
                }
            }

            return false;
        }

        private int GetBinarySize(FormDataBinary binary)
        {
            if (binary == null || binary.FileBytes == null)
                return 0;

            return binary.FileBytes.Length;
        }

        private FormModel GetFormModel(String formName)
        {
            if (_formsIDCache.Count == 0)
            {
                var session = Hb8Factory.GetCurrentSession();

                var dbForms = (from n in session.Query<GM_Form>()
                               select new
                               {
                                   n.ID,
                                   n.Name,
                               });

                foreach (var item in dbForms)
                    _formsIDCache[item.Name] = item.ID;
            }

            if (!_formsIDCache.ContainsKey(formName))
                return null;

            FormModel formModel;
            if (!_formModelsCache.TryGetValue(formName, out formModel))
            {
                var session = Hb8Factory.GetCurrentSession();

                var dbForm = session.Query<GM_Form>().FirstOrDefault(n => n.Name == formName);
                if (dbForm == null)
                    return null;

                var formConverter = new FormEntityModelConverter(session);

                formModel = formConverter.Convert(dbForm);

                _formModelsCache.Add(formName, formModel);
            }

            return formModel;
        }

        private ControlEntity GetOtherFormField(FormModel formModel, String fieldName)
        {
            if (formModel == null || formModel.Entity == null)
                return null;

            var fieldsLp = GetOtherFormFields(formModel);

            var field = fieldsLp[fieldName].FirstOrDefault();
            return field;
        }

        private ILookup<String, ControlEntity> GetOtherFormFields(FormModel formModel)
        {
            if (formModel == null || formModel.Entity == null)
                return null;

            ILookup<String, ControlEntity> fieldsLp;
            if (!_formFieldsCache.TryGetValue(formModel.ID, out fieldsLp))
            {
                fieldsLp = GetControlsLp(formModel.Entity);

                _formFieldsCache[formModel.ID] = fieldsLp;
            }

            return fieldsLp;
        }

        private IEnumerable<IDictionary<String, Object>> GetOtherFormDatas(FormModel formModel)
        {
            if (formModel == null || formModel.Entity == null)
                yield break;

            var userID = (_userID ?? UserUtil.GetCurrentUserID());

            var filter = new Dictionary<String, Object>
            {
                [FormDataConstants.UserIDField] = userID,
                [FormDataConstants.DateDeletedField] = null
            };

            var document = MongoDbUtil.FindDocuments(formModel.ID, filter).FirstOrDefault();

            var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
            if (formData == null)
                yield break;

            yield return formData;
        }

        private ILookup<String, ControlEntity> GetControlsLp(ContentEntity contentEntity)
        {
            var controls = FormStructureUtil.PreOrderTraversal(contentEntity);

            var fieldsLp = GetControlsLp(controls);
            return fieldsLp;
        }

        private ILookup<String, ControlEntity> GetControlsLp(IEnumerable<ControlEntity> controls)
        {
            var namesQuery = from n in controls
                             where !String.IsNullOrWhiteSpace(n.Name)
                             select new
                             {
                                 Key = ExpressionParser.Escape(n.Name),
                                 Entity = n
                             };

            var aliasQuery = from n in controls
                             where !String.IsNullOrWhiteSpace(n.Alias)
                             select new
                             {
                                 Key = ExpressionParser.Escape(n.Alias),
                                 Entity = n
                             };

            var finalQuery = namesQuery.Union(aliasQuery);

            var fieldsLp = finalQuery.ToLookup(n => n.Key, n => n.Entity);
            return fieldsLp;
        }

        private ControlEntity GetCorrectControl(String fieldKey, IEnumerable<ControlEntity> controls)
        {
            var query = (from n in controls
                         where n is FieldEntity ||
                               n is GridEntity ||
                               n is TreeEntity
                         orderby n.OrderIndex, n.Name
                         select n).Distinct();

            var list = query.ToList();
            if (list.Count == 0)
                return null;

            if (list.Count > 1)
                throw new Exception($"Too many fields with name or alias '{fieldKey}'");

            return list[0];
        }

        private bool IsCurrentEntityField(String formName)
        {
            var escapeFormName = ExpressionParser.Escape(formName);
            if (escapeFormName == "@")
                return true;

            if (_entity == null)
                return false;

            var entityEscName = ExpressionParser.Escape(_entity.Name);
            var entityEscAlias = ExpressionParser.Escape(_entity.Alias);

            if (entityEscName == escapeFormName || entityEscAlias == escapeFormName)
                return true;

            return false;
        }

        private Object Transform(FormDataListRef listRef)
        {
            var list = new FormDataBaseList();
            if (listRef == null || listRef.ParentID == null)
                return list;

            var formList = new FormDataLazyList(listRef);

            var contentEntity = _fieldsMap.GetValueOrDefault(listRef.OwnerID) as ContentEntity;
            if (contentEntity == null)
                return formList;

            var compitability = FormStructureUtil.CreateCompitabilityMap(contentEntity);

            foreach (var formData in formList)
            {
                var dict = FormDataUtil.Transform(formData, compitability);
                list.Add(dict);
            }

            return list;
        }
    }
}