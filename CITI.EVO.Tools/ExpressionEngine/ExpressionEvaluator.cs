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

        public static bool TryEval(String expression, out Object result)
        {
            try
            {
                result = Eval(expression);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = null;
                return false;
            }
        }
        public static bool TryEval(String expression, Func<String, Object> varResolver, out Object result)
        {
            var advResolver = new AdvancedDataResolver(varResolver);
            return TryEval(expression, advResolver, out result);
        }
        public static bool TryEval(String expression, IDataResolver dataResolver, out Object result)
        {
            try
            {
                result = Eval(expression, dataResolver);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = null;
                return false;
            }
        }

        public static bool TryEval(ExpressionNode node, out Object result)
        {
            try
            {
                result = Eval(node);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = null;

                return false;
            }
        }
        public static bool TryEval(ExpressionNode node, Func<String, Object> varResolver, out Object result)
        {
            var advResolver = new AdvancedDataResolver(varResolver);
            return TryEval(node, advResolver, out result);
        }
        public static bool TryEval(ExpressionNode node, IDataResolver dataResolver, out Object result)
        {
            try
            {
                result = Eval(node, dataResolver);
                return true;
            }
            catch (Exception ex)
            {
                var logger = LogUtil.GetLogger("ExpressionLogger");
                if (logger != null)
                {
                    var message = String.Empty;
                    if (node != null)
                        message = node.ToString();

                    logger.Error(message, ex);
                }

                result = null;

                return false;
            }
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