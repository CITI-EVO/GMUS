using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;

namespace Gms.Portal.Web.Utils
{
    public class NVelocityUtil
    {
        private readonly ISet<String> _privates;
        private readonly ContentEntity _entity;
        private readonly FormDataUnit _formData;
        private readonly ExpressionGlobalsUtil _expGlobals;
        private readonly IDictionary<Guid, ControlEntity> _controls;
        private readonly ILookup<String, ControlEntity> _controlsLp;

        public NVelocityUtil(FormDataUnit formData, ContentEntity entity)
        {
            _formData = formData;
            _entity = entity;
            _privates = formData.PrivateFields;

            var controls = FormStructureUtil.PreOrderTraversal(entity);

            _expGlobals = new ExpressionGlobalsUtil(_formData.UserID, _entity);

            _controls = controls.ToDictionary(n => n.ID);
            _controlsLp = GetControlsLp(controls);
        }

        public bool IsField(Object obj)
        {
            return (obj is FieldEntity);
        }

        public bool IsGroup(Object obj)
        {
            return (obj is GroupEntity);
        }

        public bool IsGrid(Object obj)
        {
            return (obj is GridEntity);
        }

        public bool IsTree(Object obj)
        {
            return (obj is TreeEntity);
        }

        public bool IsEmpty(Object obj)
        {
            var str = Convert.ToString(obj);
            if (String.IsNullOrWhiteSpace(str) || str == "&nbsp;")
                return true;

            var coll = obj as IEnumerable;
            if (coll != null && !coll.Cast<Object>().Any())
                return true;

            var control = GetControl(obj);
            if (control != null)
            {
                if (control is FieldEntity)
                {
                    var value = GetFieldData(obj);
                    return IsEmpty(value);
                }

                if (control is GridEntity)
                {
                    var value = GetGridData(obj);
                    return IsEmpty(value);
                }

                if (control is TreeEntity)
                {
                    var value = GetTreeData(obj);
                    return IsEmpty(value);
                }

                if (control is ContentEntity)
                {
                    var content = (ContentEntity)control;

                    var query = (from n in FormStructureUtil.PreOrderTraversal(content)
                                 where n.ID != content.ID && IsPrintable(n) && !IsEmpty(n)
                                 select n);

                    return !query.Any();
                }
            }

            return false;
        }

        public bool IsInversed(Object obj)
        {
            var field = GetControl(obj) as FieldEntity;
            if (field == null)
                return false;

            return field.Inversion.GetValueOrDefault();
        }

        public bool IsCheckable(Object obj)
        {
            var field = GetControl(obj) as FieldEntity;
            if (field == null)
                return false;

            return field.Type == "CheckBox" || field.Type == "RagioButton";
        }

        public String GetCheckType(Object obj)
        {
            var field = GetControl(obj) as FieldEntity;
            if (field == null)
                return String.Empty;

            if (field.Type == "CheckBox")
                return "checkbox";

            if (field.Type == "RagioButton")
                return "radio";

            return String.Empty;
        }

        public String GetCheckValue(Object obj)
        {
            return GetCheckValue(_formData, obj);
        }

        public String GetCheckValue(Object item, Object obj)
        {
            var value = GetFieldData(item, obj);

            var @bool = DataConverter.ToNullableBool(value);
            if (@bool.GetValueOrDefault())
                return "checked";

            return String.Empty;
        }

        public String Translate(Object val, String lang)
        {
            var str = Convert.ToString(val);
            if (String.IsNullOrWhiteSpace(str))
                return null;

            var trnKey = CryptographyUtil.ComputeMD5(str);
            var trnText = TranslationUtil.GetTranslatedText(trnKey, str, lang);

            return trnText;
        }

        public Object GetFieldData(Object obj)
        {
            return GetFieldData(_formData, obj);
        }
        public Object GetFieldData(Object item, Object obj)
        {
            var formData = item as FormDataUnit;
            if (formData == null)
                return null;

            var defKey = Convert.ToString(obj);
            if (FormDataBase.DefaultFields.Contains(defKey))
                return formData[defKey];

            var entity = GetControl(obj) as FieldEntity;
            if (entity == null)
                return null;

            var fieldKey = Convert.ToString(entity.ID);
            var fieldVal = formData[fieldKey];

            if (ReferenceEquals(fieldVal, DBNull.Value))
                return null;

            fieldVal = GetLabelText(entity, fieldVal);

            var strVal = Convert.ToString(fieldVal);
            if (String.IsNullOrWhiteSpace(strVal))
                strVal = "&nbsp;";

            return strVal;
        }

        public IEnumerable<String> GetCommonFields(Object x, Object y)
        {
            var xEntity = GetControl(x) as ContentEntity;
            if (xEntity == null)
                return Enumerable.Empty<String>();

            var yEntity = GetControl(y) as ContentEntity;
            if (yEntity == null)
                return Enumerable.Empty<String>();

            var xControls = FormStructureUtil.PreOrderTraversal(xEntity);
            var xNamesSet = xControls.Select(n => n.Name).ToHashSet();

            var yControls = FormStructureUtil.PreOrderTraversal(yEntity);
            var yNamesSet = yControls.Select(n => n.Name).ToHashSet();

            xNamesSet.IntersectWith(yNamesSet);

            return xNamesSet;
        }

        public IEnumerable<FormDataBase> GetMergeData(Object x, Object y, IEnumerable<String> commonFields)
        {
            var xEntity = GetControl(x) as ContentEntity;
            if (xEntity == null)
                yield break;

            var yEntity = GetControl(y) as ContentEntity;
            if (yEntity == null)
                yield break;

            var treesAndGrids = new HashSet<String>
            {
                Convert.ToString(xEntity.ID),
                Convert.ToString(yEntity.ID)
            };

            var commonNames = commonFields.ToHashSet();

            var formDatas = FormDataUtil.MegreFormDatasFields(_entity, treesAndGrids, commonNames, _formData);
            foreach (var formData in formDatas)
                yield return formData;
        }

        public IEnumerable<FormDataUnit> GetGridData(Object obj)
        {
            var entity = GetControl(obj) as ContentEntity;
            if (entity == null)
                yield break;

            var fieldKey = Convert.ToString(entity.ID);
            var fieldVal = _formData[fieldKey];

            if (ReferenceEquals(fieldVal, DBNull.Value))
                yield break;

            var list = Enumerable.Empty<FormDataUnit>();

            if (fieldVal is FormDataListRef)
                list = new FormDataLazyList((FormDataListRef)fieldVal);
            else if (fieldVal is FormDataListBase)
                list = (FormDataListBase)fieldVal;

            foreach (var item in list)
                yield return item;
        }

        public IEnumerable<FormDataUnit> GetTreeData(Object obj)
        {
            var entity = GetControl(obj) as ContentEntity;
            if (entity == null)
                yield break;

            var fieldKey = Convert.ToString(entity.ID);
            var fieldVal = _formData[fieldKey];

            if (ReferenceEquals(fieldVal, DBNull.Value))
                yield break;

            var list = Enumerable.Empty<FormDataUnit>();

            if (fieldVal is FormDataListRef)
                list = new FormDataLazyList((FormDataListRef)fieldVal);
            else if (fieldVal is FormDataListBase)
                list = (FormDataListBase)fieldVal;

            foreach (var item in list)
                yield return item;
        }

        public IEnumerable<ControlEntity> GetChildren(Object obj)
        {
            var entity = GetControl(obj) as ContentEntity;
            if (entity == null || entity.Controls == null || entity.Controls.Count == 0)
                yield break;

            var query = (from n in entity.Controls
                         orderby n.OrderIndex, n.Name
                         select n);

            foreach (var control in query)
            {
                if (!IsPrintable(control))
                    continue;

                if (control is TabContainerEntity || control is TabPageEntity)
                {
                    foreach (var child in GetChildren((ContentEntity)control))
                        yield return child;
                }
                else
                {
                    yield return control;
                }
            }
        }

        public IEnumerable<int> Range(int from, int to)
        {
            return Enumerable.Range(from, to);
        }

        public Object Format(String format, params Object[] values)
        {
            return String.Format(format, values);
        }

        public double? ConvertToDouble(Object val)
        {
            return DataConverter.ToNullableDouble(val);
        }
        public double? ConvertToDouble(Object val, double @default)
        {
            var value = DataConverter.ToNullableDouble(val);
            return value.GetValueOrDefault(@default);
        }

        public int? ConvertToInt(Object val)
        {
            return DataConverter.ToNullableInt32(val);
        }
        public int? ConvertToInt(Object val, int @default)
        {
            var value = DataConverter.ToNullableInt32(val);
            return value.GetValueOrDefault(@default);
        }

        private bool IsPrintable(ControlEntity entity)
        {
            if (entity == null || !entity.Visible || entity.NotPrintable.GetValueOrDefault())
                return false;

            var key = Convert.ToString(entity.ID);
            if (_privates != null && _privates.Count > 0 && _privates.Contains(key))
                return false;

            return true;
        }

        public ControlEntity GetControl(Object obj)
        {
            var entity = obj as ControlEntity;
            if (entity != null)
                return entity;

            var itemID = DataConverter.ToNullableGuid(obj);
            if (itemID != null)
                entity = _controls.GetValueOrDefault(itemID.Value);

            var itemKey = DataConverter.ToString(obj);
            if (entity == null)
                entity = _controlsLp[itemKey].FirstOrDefault();

            return entity;
        }

        public ControlEntity GetControl(Object obj, String name)
        {
            var entity = GetControl(obj) as ContentEntity;
            if (entity == null)
                return null;

            var child = (from n in GetChildren(entity)
                         where n.Name == name ||
                               n.Alias == name
                         select n).FirstOrDefault();

            return child;
        }

        private ILookup<String, ControlEntity> GetControlsLp(IEnumerable<ControlEntity> controls)
        {
            var namesQuery = from n in controls
                             where !String.IsNullOrWhiteSpace(n.Name)
                             select new
                             {
                                 Key = n.Name,
                                 Entity = n
                             };

            var aliasQuery = from n in controls
                             where !String.IsNullOrWhiteSpace(n.Alias)
                             select new
                             {
                                 Key = n.Alias,
                                 Entity = n
                             };

            var finalQuery = namesQuery.Union(aliasQuery);

            var fieldsLp = finalQuery.ToLookup(n => n.Key, n => n.Entity);
            return fieldsLp;
        }

        private Object GetLabelText(FieldEntity field, Object value)
        {
            if (field == null)
                return value;

            if (field.Type != "ComboBox" && field.Type != "CheckBoxList")
                return value;

            var textExp = field.TextExpression;
            var valueExp = field.ValueExpression;

            if (String.IsNullOrWhiteSpace(field.DataSourceID) || String.IsNullOrWhiteSpace(textExp) || String.IsNullOrWhiteSpace(valueExp))
                return value;

            var userID = _formData.UserID;
            var dataSourceHelper = new DataSourceHelper(userID, field);

            var values = new[] { value };
            if (value is IEnumerable && !(value is String))
            {
                var collection = (IEnumerable)value;
                values = collection.Cast<Object>().ToArray();
            }

            var dataRecords = dataSourceHelper.FindDataRecords(values);
            if (dataRecords == null)
                return value;

            var texts = GetLabelTexts(dataRecords, textExp);
            var result = String.Join("; ", texts);

            return result;
        }

        private IEnumerable<String> GetLabelTexts(IEnumerable<FormDataBase> dataRecords, String textExpression)
        {
            var expNode = ExpressionParser.GetOrParse(textExpression);

            foreach (var dataRecord in dataRecords)
            {
                _expGlobals.AddSource(dataRecord);

                Object result;
                if (!ExpressionEvaluator.TryEval(expNode, _expGlobals.Eval, out result))
                    yield return "[TextExpression error]";

                yield return Convert.ToString(result);

                _expGlobals.RemoveSource(dataRecord);
            }
        }
    }
}