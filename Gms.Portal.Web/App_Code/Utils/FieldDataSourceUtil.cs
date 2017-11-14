using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.ExpressionEngine;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;

namespace Gms.Portal.Web.Utils
{
    public class FieldDataSourceUtil
    {
        private readonly ExpressionGlobalsUtil _expGlobals;
        private readonly DataSourceHelper _dataSourceHelper;

        private readonly ContentEntity _parent;
        private readonly ContentEntity _content;
        private readonly FieldEntity _field;

        public FieldDataSourceUtil(Guid? userID, ContentEntity content, Guid? fieldID)
        {
            var fields = FormStructureUtil.PreOrderTraversal(content).OfType<FieldEntity>();

            _content = content;
            _field = fields.Single(n => n.ID == fieldID);

            _expGlobals = new ExpressionGlobalsUtil(_content);
            _dataSourceHelper = new DataSourceHelper(userID, _content, _field);
        }

        public FieldDataSourceUtil(Guid? userID, ContentEntity content, FieldEntity field)
        {
            _content = content;
            _field = field;

            _expGlobals = new ExpressionGlobalsUtil(_content);
            _dataSourceHelper = new DataSourceHelper(userID, _content, _field);
        }

        public FieldDataSourceUtil(Guid? userID, ContentEntity parent, ContentEntity content, Guid? fieldID)
        {
            var fields = FormStructureUtil.PreOrderTraversal(content).OfType<FieldEntity>();

            _parent = parent;
            _content = content;
            _field = fields.Single(n => n.ID == fieldID);

            _expGlobals = new ExpressionGlobalsUtil(_parent);
            _dataSourceHelper = new DataSourceHelper(userID, _parent, _field);
        }

        public FieldDataSourceUtil(Guid? userID, ContentEntity parent, ContentEntity content, FieldEntity field)
        {
            _parent = parent;
            _content = content;
            _field = field;

            _expGlobals = new ExpressionGlobalsUtil(_parent);
            _dataSourceHelper = new DataSourceHelper(userID, _parent, _field);
        }

        public String GetFieldText(Object value)
        {
            var textExp = _field.TextExpression;
            var valueExp = _field.ValueExpression;

            if (_field.DataSourceID == null || String.IsNullOrWhiteSpace(textExp) || String.IsNullOrWhiteSpace(valueExp))
                return Convert.ToString(value);

            var values = new[] { value };
            if (value is IEnumerable && !(value is String))
            {
                var collection = (IEnumerable)value;
                values = collection.Cast<Object>().ToArray();
            }

            var dataRecords = _dataSourceHelper.FindDataRecords(values);
            if (dataRecords == null)
                return Convert.ToString(value);

            var recordsList = dataRecords.ToList();
            if (recordsList.Count == 0)
                return Convert.ToString(value);

            var texts = GetFieldTexts(recordsList, textExp);
            var result = String.Join("; ", texts);

            return result;
        }

        private IEnumerable<String> GetFieldTexts(IEnumerable<FormDataBase> dataRecords, String textExpression)
        {
            var expNode = ExpressionParser.GetOrParse(textExpression);

            foreach (var dataRecord in dataRecords)
            {
                _expGlobals.AddSource(dataRecord);

                var result = ExpressionEvaluator.TryEval(expNode, _expGlobals.Eval);
                if (result.Error != null)
                    yield return $"[TextExpression error] - {result.Error.Message}";
                else
                    yield return Convert.ToString(result.Value);

                _expGlobals.RemoveSource(dataRecord);
            }
        }
    }
}