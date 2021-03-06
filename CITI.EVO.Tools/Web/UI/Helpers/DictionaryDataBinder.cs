﻿using CITI.EVO.Tools.ExpressionEngine;
using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.Tools.Web.UI.Helpers
{
    public class DictionaryDataBinder
    {
        private readonly ExpressionNode _textExpression;
        private readonly ExpressionNode _valueExpression;

        private readonly IEnumerable<IDictionary<String, Object>> _collection;

        public DictionaryDataBinder(IEnumerable<IDictionary<String, Object>> collection, String textExpression, String valueExpression)
        {
            _textExpression = ExpressionParser.GetOrParse(textExpression);
            _valueExpression = ExpressionParser.GetOrParse(valueExpression);

            _collection = collection;
        }

        public IEnumerable<DictionaryBinderItem> GetItems()
        {
            foreach (var item in _collection)
            {
                var advResolver = new AdvancedDataResolver(n => item[n]);

                var text = ExpressionEvaluator.Eval(_textExpression, advResolver);
                var value = ExpressionEvaluator.Eval(_valueExpression, advResolver);

                var binder = new DictionaryBinderItem(text, value);
                yield return binder;
            }
        }
    }
}
