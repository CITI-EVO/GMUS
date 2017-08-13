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
        public static Object Eval(ExpressionNode node, IDataResolver dataResolver)
        {
            var operatorNode = (OperatorNode)node;

            if (String.Equals(node.Action, "||"))
            {
                var leftFlag = Convert.ToBoolean(ExpressionEvaluator.Eval(operatorNode.Left, dataResolver));
                if (leftFlag)
                    return true;

                return Convert.ToBoolean(ExpressionEvaluator.Eval(operatorNode.Right, dataResolver));
            }

            if (String.Equals(node.Action, "&&"))
            {
                var leftFlag = Convert.ToBoolean(ExpressionEvaluator.Eval(operatorNode.Left, dataResolver));
                if (!leftFlag)
                    return false;

                return Convert.ToBoolean(ExpressionEvaluator.Eval(operatorNode.Right, dataResolver));
            }

            var leftNode = operatorNode.Left;
            var rightNode = operatorNode.Right;

            var leftValue = ExpressionEvaluator.Eval(leftNode, dataResolver);
            var rightValue = ExpressionEvaluator.Eval(rightNode, dataResolver);

            switch (node.Action.ToLower())
            {
                case "!":
                    return !Convert.ToBoolean(rightValue);
                case "+":
                    {
                        if (leftNode == null)
                            return +ExpressionHelper.GetNumber(rightValue);

                        if (ExpressionHelper.IsDateTimeNode(leftNode, leftValue) || ExpressionHelper.IsDateTimeNode(rightNode, rightValue))
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

                        if (ExpressionHelper.IsDateTimeNode(leftNode, leftValue) || ExpressionHelper.IsDateTimeNode(rightNode, rightValue))
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
                    {
                        var name = leftNode.Action;
                        dataResolver.SetValue(name, rightValue);

                        return rightValue;
                    }
                case "==":
                    return ExpressionHelper.Compare(leftNode, leftValue, rightNode, rightValue) == 0;
                case "!=":
                case "<>":
                    return ExpressionHelper.Compare(leftNode, leftValue, rightNode, rightValue) != 0;
                case "<=":
                    return ExpressionHelper.Compare(leftNode, leftValue, rightNode, rightValue) <= 0;
                case ">=":
                    return ExpressionHelper.Compare(leftNode, leftValue, rightNode, rightValue) >= 0;
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
                    return ExpressionHelper.Compare(leftNode, leftValue, rightNode, rightValue) < 0;
                case ">":
                    return ExpressionHelper.Compare(leftNode, leftValue, rightNode, rightValue) > 0;
            }

            var message = $"Unknown operator '{node.Action}'";
            throw new Exception(message);
        }
    }
}