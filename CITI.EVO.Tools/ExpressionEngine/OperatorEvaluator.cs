using System;
using System.Collections.Generic;
using CITI.EVO.Tools.ExpressionEngine.Common;
using System.Globalization;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Utils;

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
                            return +ExpressionHelper.GetNumber(rightValue);

                        if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
                        {
                            var leftDate = ExpressionHelper.GetDateTime(leftValue);
                            var rightDate = ExpressionHelper.GetDateTime(rightValue);

                            return leftDate.AddTicks(rightDate.Ticks);
                        }

                        return ExpressionHelper.GetNumber(leftValue) + ExpressionHelper.GetNumber(rightValue);
                    }
                case "++":
                    return ExpressionHelper.GetNumber(rightValue) + 1D;
                case "-":
                    {
                        if (leftNode == null)
                            return -ExpressionHelper.GetNumber(rightValue);

                        if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
                        {
                            var leftDate = ExpressionHelper.GetDateTime(leftValue);
                            var rightDate = ExpressionHelper.GetDateTime(rightValue);
                            var ticksDiff = Math.Abs(leftDate.Ticks - rightDate.Ticks);

                            return new DateTime(ticksDiff);
                        }

                        return ExpressionHelper.GetNumber(leftValue) - ExpressionHelper.GetNumber(rightValue);
                    }
                case "--":
                    return ExpressionHelper.GetNumber(rightValue) - 1D;
                case "=":
                    break;
                case "==":
                    return Compare(leftNode, leftValue, rightNode, rightValue) == 0;
                case "!=":
                case "<>":
                    return Compare(leftNode, leftValue, rightNode, rightValue) != 0;
                case "<=":
                    return Compare(leftNode, leftValue, rightNode, rightValue) <= 0;
                case ">=":
                    return Compare(leftNode, leftValue, rightNode, rightValue) >= 0;
                case "&&":
                    return Convert.ToBoolean(leftValue) && Convert.ToBoolean(rightValue);
                case "||":
                    return Convert.ToBoolean(leftValue) || Convert.ToBoolean(rightValue);
                case "^":
                    {
                        var a = ExpressionHelper.GetNumber(leftValue);
                        var b = ExpressionHelper.GetNumber(rightValue);

                        return Math.Pow(a, b);
                    }
                case "&":
                    return String.Concat(leftValue, rightValue);
                case "|":
                    return ExpressionHelper.GetNumber(leftValue) % ExpressionHelper.GetNumber(rightValue);
                case "%":
                    return ExpressionHelper.GetNumber(leftValue) / 100D * ExpressionHelper.GetNumber(rightValue);
                case "*":
                    return ExpressionHelper.GetNumber(leftValue) * ExpressionHelper.GetNumber(rightValue);
                case "/":
                case "\\":
                    return ExpressionHelper.GetNumber(leftValue) / ExpressionHelper.GetNumber(rightValue);
                case "<":
                    return Compare(leftNode, leftValue, rightNode, rightValue) < 0;
                case ">":
                    return Compare(leftNode, leftValue, rightNode, rightValue) > 0;
            }

            throw new Exception("Unknown operation");
        }

        private static bool IsDateTimeNode(ExpressionNode node, Object value)
        {
            if ((node.ValueType == ValueTypes.DateTime) ||
                (node.ValueType == ValueTypes.Variable && value is DateTime) ||
                (node.ActionType == ActionTypes.Function && value is DateTime))
                return true;

            return false;
        }

        private static int Compare(ExpressionNode leftNode, Object leftValue, ExpressionNode rightNode, Object rightValue)
        {
            if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
            {
                var leftDate = ExpressionHelper.GetDateTime(leftValue);
                var rightDate = ExpressionHelper.GetDateTime(rightValue);

                return leftDate.CompareTo(rightDate);
            }

            var leftDbl = DataConverter.ToNullableDouble(leftValue);
            var rightDbl = DataConverter.ToNullableDouble(rightValue);

            if (leftDbl != null && rightDbl != null)
                return leftDbl.Value.CompareTo(rightDbl.Value);

            return ExpressionHelper.Compare(leftValue, rightValue);
        }
    }
}