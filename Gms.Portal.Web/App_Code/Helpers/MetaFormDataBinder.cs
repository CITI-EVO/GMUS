using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Helpers
{
    public class MetaFormDataBinder
    {
        private readonly ExpressionNode _textExpression;
        private readonly ExpressionNode _valueExpression;

        private readonly IEnumerable<IDictionary<String, Object>> _collection;

        public MetaFormDataBinder(IEnumerable<IDictionary<String, Object>> collection, String textExpression, String valueExpression)
        {
            _textExpression = ExpressionParser.GetOrParse(textExpression);
            _valueExpression = ExpressionParser.GetOrParse(valueExpression);

            _collection = collection;
        }

        public IEnumerable<DictionaryBinderItem> GetItems()
        {
            var itemsLp = _collection.ToLookup(GetContainerID);
            if (itemsLp.Count > 1)
            {
                var binders = PreOrderTraversel(itemsLp);

                foreach (var binder in binders)
                    yield return binder;
            }
            else
            {
                foreach (var item in _collection)
                    yield return CreateBinderItem(item, 0);
            }
        }

        private String GetItemID(IDictionary<String, Object> item)
        {
            if (item == null)
                return String.Empty;

            var value = item.GetValueOrDefault(FormDataConstants.IDField);
            var strVal = Convert.ToString(value);

            return strVal;
        }

        private String GetContainerID(IDictionary<String, Object> item)
        {
            if (item == null)
                return String.Empty;

            var value = item.GetValueOrDefault(FormDataConstants.ContainerIDField);
            var strVal = Convert.ToString(value);

            return strVal;
        }

        private DictionaryBinderItem CreateBinderItem(IDictionary<String, Object> item, int level)
        {
            var text = ExpressionEvaluator.Eval(_textExpression, n => item[n]);
            var value = ExpressionEvaluator.Eval(_valueExpression, n => item[n]);

            if (level > 0)
            {
                const String separator = "---";

                var prefix = String.Empty;
                for (int i = 0; i < level; i++)
                    prefix += separator;

                text = $"{prefix}{text}";
            }

            var binder = new DictionaryBinderItem(text, value);
            return binder;
        }

        private IEnumerable<DictionaryBinderItem> PreOrderTraversel(ILookup<String, IDictionary<String, Object>> itemsLp)
        {
            var parents = itemsLp[String.Empty];

            foreach (var item in parents)
            {
                var children = PreOrderTraversel(item, itemsLp, 0);
                foreach (var childBinder in children)
                    yield return childBinder;
            }
        }

        private IEnumerable<DictionaryBinderItem> PreOrderTraversel(IDictionary<String, Object> parent, ILookup<String, IDictionary<String, Object>> itemsLp, int level)
        {
            var parentBinder = CreateBinderItem(parent, level);
            yield return parentBinder;

            var itemID = GetItemID(parent);
            var children = itemsLp[itemID];

            foreach (var item in children)
            {
                var childBinders = PreOrderTraversel(item, itemsLp, level + 1);
                foreach (var chuldBinder in childBinders)
                    yield return chuldBinder;
            }
        }
    }
}