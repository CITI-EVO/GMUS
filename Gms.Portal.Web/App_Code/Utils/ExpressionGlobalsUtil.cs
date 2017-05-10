using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.ExpressionEngine;
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

        private readonly IEnumerable<ControlEntity> _fields;
        private readonly ILookup<String, ControlEntity> _fieldsLp;

        private readonly IDictionary<String, String> _associations;
        private readonly IList<IDictionary<String, Object>> _sources;

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
            : this(userID, FormStructureUtil.PreOrderTraversal(entity), sources)
        {
        }

        public ExpressionGlobalsUtil(Guid? userID, IEnumerable<ControlEntity> fields, IEnumerable<IDictionary<String, Object>> sources)
        {
            _userID = userID;
            _fields = fields;
            _sources = sources.ToList();

            _fieldsLp = _fields.ToLookup(n => ExpressionParser.Escape(n.Name));
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

            if (RegexUtil.DataGridFuncParserRx.IsMatch(fieldKey))
            {
                var match = RegexUtil.DataGridFuncParserRx.Match(fieldKey);

                var form = match.Groups["form"].Value;
                var grid = match.Groups["grid"].Value;
                var column = match.Groups["col"].Value;

                if (String.IsNullOrWhiteSpace(column))
                    return GetValue(form, grid);

                return GetValue(form, grid, column);
            }

            var escapeKey = ExpressionParser.Escape(fieldKey);

            if (!FormDataBase.DefaultFields.Contains(escapeKey))
            {
                var fieldEntity = _fieldsLp[escapeKey].FirstOrDefault();
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
            return _associations.ContainsKey(source); ;
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
            if (escapeFormName != "@")
            {
                var formModel = GetFormModel(escapeFormName);
                if (formModel == null)
                    return null;

                sources = GetOtherFormDatas(formModel).ToList();

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
        private Object GetValue(String formName, String gridName, String fieldName)
        {
            var escapeFormName = ExpressionParser.Escape(formName);
            var escapeGridName = ExpressionParser.Escape(gridName);
            var escapeFieldName = ExpressionParser.Escape(fieldName);

            var fields = _fieldsLp;

            var sources = _sources;
            if (escapeFormName != "@")
            {
                var formModel = GetFormModel(escapeFormName);
                if (formModel == null)
                    return null;

                sources = GetOtherFormDatas(formModel).ToList();
                fields = GetOtherFormFields(formModel);
            }

            var contentEntity = fields[escapeGridName].OfType<ContentEntity>().FirstOrDefault();
            if (contentEntity == null)
                return null;

            var formGridData = GetValue(sources, Convert.ToString(contentEntity.ID));
            if (formGridData is FormDataListRef || formGridData is FormDataListBase)
            {
                FormDataListBase formDataList;

                if (formGridData is FormDataListRef)
                    formDataList = new FormDataLazyList((FormDataListRef)formGridData);
                else
                    formDataList = (FormDataListBase)formGridData;

                var fieldKey = escapeFieldName;


                var fieldEntity = contentEntity.Controls.FirstOrDefault(n => ExpressionParser.Escape(n.Name) == escapeFieldName);

                if (fieldEntity != null)
                    fieldKey = Convert.ToString(fieldEntity.ID);
                else if (!FormDataBase.DefaultFields.Contains(fieldKey))
                    return null;

                var valuesQuery = formDataList.Select(n => n[fieldKey]);

                var values = valuesQuery.ToArray();
                return values;
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
                    return true;
            }

            return false;
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
                var controls = FormStructureUtil.PreOrderTraversal(formModel.Entity);

                var entitiesQuery = (from n in controls
                                     let key = (!String.IsNullOrWhiteSpace(n.Alias) ? n.Alias : n.Name)
                                     where !String.IsNullOrWhiteSpace(key)
                                     select new
                                     {
                                         Key = ExpressionParser.Escape(key),
                                         Entity = n
                                     });

                //var namesQuery = (from n in controls
                //                  where !String.IsNullOrWhiteSpace(n.Name)
                //                  select new
                //                  {
                //                      Key = ExpressionParser.Escape(n.Name),
                //                      Entity = n
                //                  });

                //var aliasQuery = (from n in controls
                //                  where !String.IsNullOrWhiteSpace(n.Alias)
                //                  select new
                //                  {
                //                      Key = ExpressionParser.Escape(n.Alias),
                //                      Entity = n
                //                  });

                //var finalQuery = namesQuery.Union(aliasQuery);

                fieldsLp = entitiesQuery.ToLookup(n => n.Key, n => n.Entity);

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
    }
}