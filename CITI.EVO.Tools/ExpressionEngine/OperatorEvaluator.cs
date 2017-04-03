using System;
using CITI.EVO.Tools.ExpressionEngine.Common;

namespace CITI.EVO.Tools.ExpressionEngine
{
    internal static class OperatorEvaluator
    {
        public static Object Eval(ExpressionNode node, Func<String, Object> varResolver)
        {
            var operatorNode = (OperatorNode)node;

            if (String.Equals(node.Action, "||"))
            {
                var leftFlag = Convert.ToBoolean(ExpressionEvaluator.Eval(operatorNode.Left, varResolver));
                if (leftFlag)
                    return true;

                return Convert.ToBoolean(ExpressionEvaluator.Eval(operatorNode.Right, varResolver));
            }

            if (String.Equals(node.Action, "&&"))
            {
                var leftFlag = Convert.ToBoolean(ExpressionEvaluator.Eval(operatorNode.Left, varResolver));
                if (!leftFlag)
                {
                    return false;
                }

                return Convert.ToBoolean(ExpressionEvaluator.Eval(operatorNode.Right, varResolver));
            }

            var leftNode = operatorNode.Left;
            var rightNode = operatorNode.Right;

            var leftValue = ExpressionEvaluator.Eval(leftNode, varResolver);
            var rightValue = ExpressionEvaluator.Eval(rightNode, varResolver);

            switch (node.Action.ToLower())
            {
                case "!":
                    return !Convert.ToBoolean(rightValue);
                case "+":
                    {
                        if (leftNode == null)
                            return +Convert.ToDouble(rightValue);

                        if (IsStringNode(leftNode, leftValue) || IsStringNode(rightNode, rightValue))
                            return String.Format("{0}{1}", leftValue, rightValue);

                        if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
                        {
                            var leftDate = ExpressionHelper.GetDateTime(leftValue);
                            var rightDate = ExpressionHelper.GetDateTime(rightValue);

                            return leftDate.AddTicks(rightDate.Ticks);
                        }

                        return Convert.ToDouble(leftValue) + Convert.ToDouble(rightValue);
                    }
                case "++":
                    return Convert.ToDouble(rightValue) + 1D;
                case "-":
                    {
                        if (leftNode == null)
                            return -Convert.ToDouble(rightValue);

                        if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
                        {
                            var leftDate = ExpressionHelper.GetDateTime(leftValue);
                            var rightDate = ExpressionHelper.GetDateTime(rightValue);

                            return leftDate.AddTicks(-rightDate.Ticks);
                        }

                        return Convert.ToDouble(leftValue) - Convert.ToDouble(rightValue);
                    }
                case "--":
                    return Convert.ToDouble(rightValue) - 1D;
                case "=":
                    break;
                case "==":
                    return IsEquals(leftValue, rightValue);
                case "!=":
                case "<>":
                    {
                        if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
                        {
                            var leftDate = ExpressionHelper.GetDateTime(leftValue);
                            var rightDate = ExpressionHelper.GetDateTime(rightValue);

                            return leftDate != rightDate;
                        }

                        return !IsEquals(leftValue, rightValue);
                    }
                case "<=":
                    {
                        if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
                        {
                            var leftDate = ExpressionHelper.GetDateTime(leftValue);
                            var rightDate = ExpressionHelper.GetDateTime(rightValue);

                            return leftDate <= rightDate;
                        }

                        return Convert.ToDouble(leftValue) <= Convert.ToDouble(rightValue);
                    }
                case ">=":
                    {
                        if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
                        {
                            var leftDate = ExpressionHelper.GetDateTime(leftValue);
                            var rightDate = ExpressionHelper.GetDateTime(rightValue);

                            return leftDate >= rightDate;
                        }
                        return Convert.ToDouble(leftValue) >= Convert.ToDouble(rightValue);
                    }
                case "&&":
                    return Convert.ToBoolean(leftValue) && Convert.ToBoolean(rightValue);
                case "||":
                    return Convert.ToBoolean(leftValue) || Convert.ToBoolean(rightValue);
                case "^":
                    return Math.Pow(Convert.ToDouble(leftValue), Convert.ToDouble(rightValue));
                case "&":
                    return Convert.ToInt64(leftValue) & Convert.ToInt64(rightValue);
                case "|":
                    return Convert.ToInt64(leftValue) | Convert.ToInt64(rightValue);
                case "%":
                    return Convert.ToDouble(leftValue) / 100D * Convert.ToDouble(rightValue);
                case "*":
                    return Convert.ToDouble(leftValue) * Convert.ToDouble(rightValue);
                case "/":
                case "\\":
                    return Convert.ToDouble(leftValue) / Convert.ToDouble(rightValue);
                case "<":
                    {
                        if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
                        {
                            var leftDate = ExpressionHelper.GetDateTime(leftValue);
                            var rightDate = ExpressionHelper.GetDateTime(rightValue);

                            return leftDate < rightDate;
                        }
                        return Convert.ToDouble(leftValue) < Convert.ToDouble(rightValue);
                    }
                case ">":
                    {
                        if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
                        {
                            var leftDate = ExpressionHelper.GetDateTime(leftValue);
                            var rightDate = ExpressionHelper.GetDateTime(rightValue);

                            return leftDate > rightDate;
                        }

                        return Convert.ToDouble(leftValue) > Convert.ToDouble(rightValue);
                    }
            }

            throw new Exception("Unknown operation");
        }

        private static bool IsEquals(Object x, Object y)
        {
            var xVal = Convert.ToString(x);
            var yVal = Convert.ToString(y);

            return StringComparer.Ordinal.Equals(xVal, yVal);
        }

        private static bool IsDateTimeNode(ExpressionNode node, Object value)
        {
            if ((node.ValueType == ValueTypes.DateTime) ||
                (node.ValueType == ValueTypes.Variable && value is DateTime) ||
                (node.ActionType == ActionTypes.Function && value is DateTime))
                return true;

            return false;
        }

        private static bool IsStringNode(ExpressionNode node, Object value)
        {
            return (node.ValueType == ValueTypes.String || (node.ValueType == ValueTypes.Variable && value is String));
        }
    }
}