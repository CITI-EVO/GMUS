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
            return Eval(expression, new DefaultDataResolver());
        }
        public static Object Eval(String expression, Func<String, Object> varResolver)
        {
            var node = ExpressionParser.Parse(expression);
            return Eval(node, varResolver);
        }
        public static Object Eval(String expression, IDataResolver dataResolver)
        {
            var node = ExpressionParser.Parse(expression);
            return Eval(node, dataResolver);
        }

        public static Object Eval(ExpressionNode node)
        {
            return Eval(node, new DefaultDataResolver());
        }
        public static Object Eval(ExpressionNode node, Func<String, Object> varResolver)
        {
            var advResolver = new AdvancedDataResolver(varResolver);
            return Eval(node, advResolver);
        }
        public static Object Eval(ExpressionNode node, IDataResolver dataResolver)
        {
            if (node == null)
                return null;

            if (!node.Container)
                return IntenalEval(node, dataResolver);

            if (node.Params == null || node.Params.Count == 0)
                return null;

            var result = (Object)null;

            foreach (var expNode in node.Params)
                result = IntenalEval(expNode, dataResolver);

            return result;
        }

        public static ExpressionResult TryEval(String expression)
        {
            var result = new ExpressionResult();

            try
            {
                result.Value = Eval(expression);
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }
        public static ExpressionResult TryEval(String expression, Func<String, Object> varResolver)
        {
            var advResolver = new AdvancedDataResolver(varResolver);
            return TryEval(expression, advResolver);
        }
        public static ExpressionResult TryEval(String expression, IDataResolver dataResolver)
        {
            var result = new ExpressionResult();

            try
            {
                result.Value = Eval(expression, dataResolver);
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }

        public static ExpressionResult TryEval(ExpressionNode node)
        {
            var result = new ExpressionResult();

            try
            {
                result.Value = Eval(node);
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }
        public static ExpressionResult TryEval(ExpressionNode node, Func<String, Object> varResolver)
        {
            var advResolver = new AdvancedDataResolver(varResolver);
            return TryEval(node, advResolver);
        }
        public static ExpressionResult TryEval(ExpressionNode node, IDataResolver dataResolver)
        {
            var result = new ExpressionResult();

            try
            {
                result.Value = Eval(node, dataResolver);
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }

        private static Object IntenalEval(ExpressionNode node, IDataResolver dataResolver)
        {
            switch (node.ActionType)
            {
                case ActionTypes.Function:
                    return FunctionEvaluator.Eval(node, dataResolver);
                case ActionTypes.Operator:
                    return OperatorEvaluator.Eval(node, dataResolver);
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

                        if (_ordinalComparer.Equals(node.Action, "null"))
                            return null;

                        return dataResolver.GetValue(node.Action);
                    }
            }

            return node.Value;
        }
    }
}