using System;
using CITI.EVO.Tools.ExpressionEngine.Common;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.Tools.ExpressionEngine
{
    public static class ExpressionEvaluator
    {
        private static readonly StringComparer _ordinalComparer;

        static ExpressionEvaluator()
        {
            _ordinalComparer = StringComparer.OrdinalIgnoreCase;
        }

        public static Object Eval(String expression)
        {
            return Eval(expression, null);
        }
        public static Object Eval(String expression, Func<String, Object> varResolver)
        {
            var node = ExpressionParser.Parse(expression);
            return Eval(node, varResolver);
        }

        public static Object Eval(ExpressionNode node)
        {
            return Eval(node, null);
        }
        public static Object Eval(ExpressionNode node, Func<String, Object> varResolver)
        {
            if (node == null)
                return null;

            switch (node.ActionType)
            {
                case ActionTypes.Function:
                    return FunctionEvaluator.Eval(node, varResolver);
                case ActionTypes.Operator:
                    return OperatorEvaluator.Eval(node, varResolver);
            }

            switch (node.ValueType)
            {
                case ValueTypes.Variable:
                    {
                        if (_ordinalComparer.Equals(node.Action, "e"))
                            return Math.E;

                        if (_ordinalComparer.Equals(node.Action, "pi"))
                            return Math.PI;

                        if (_ordinalComparer.Equals(node.Action, "true"))
                            return true;

                        if (_ordinalComparer.Equals(node.Action, "false"))
                            return false;

                        if (_ordinalComparer.Equals(node.Action, "lang"))
                            return LanguageUtil.GetLanguage();

                        return varResolver(node.Action);
                    }
            }

            return node.Value;
        }
    }
}