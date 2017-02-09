using System;
using CITI.EVO.Tools.ExpressionEngine.Common;

namespace CITI.EVO.Tools.ExpressionEngine
{
	internal static class OperatorEvaluator
	{
		public static Object Eval(ExpressionNode node, ExpressionEvaluator expEvaluator)
		{
			var operatorNode = (OperatorNode)node;

			if (String.Equals(node.Action, "||"))
			{
				var leftFlag = Convert.ToBoolean(expEvaluator.Eval(operatorNode.Left));
				if (leftFlag)
				{
					return true;
				}

				return Convert.ToBoolean(expEvaluator.Eval(operatorNode.Right));
			}

			if (String.Equals(node.Action, "&&"))
			{
				var leftFlag = Convert.ToBoolean(expEvaluator.Eval(operatorNode.Left));
				if (!leftFlag)
				{
					return false;
				}

				return Convert.ToBoolean(expEvaluator.Eval(operatorNode.Right));
			}

			var leftNode = operatorNode.Left;
			var rightNode = operatorNode.Right;

			var leftValue = expEvaluator.Eval(leftNode);
			var rightValue = expEvaluator.Eval(rightNode);

			switch (node.Action.ToLower())
			{
				case "!":
					return !Convert.ToBoolean(rightValue);
				case "+":
				{
					if (leftNode == null)
					{
						return +Convert.ToDouble(rightValue);
					}

					if (IsStringNode(leftNode, leftValue) || IsStringNode(rightNode, rightValue))
					{
						return String.Format("{0}{1}", leftValue, rightValue);
					}

					if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
					{
						var leftDate = ExpressionHelper.GetDateTime(leftValue);
						var rightDate = ExpressionHelper.GetDateTime(rightValue);

						return leftDate.AddTicks(rightDate.Ticks);
					}

					return Convert.ToDouble(leftValue) + Convert.ToDouble(rightValue);
				}
				case "++":
					return Convert.ToDouble(rightValue) + 1;
				case "-":
				{
					if (leftNode == null)
					{
						return -Convert.ToDouble(rightValue);
					}

					if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
					{
						var leftDate = ExpressionHelper.GetDateTime(leftValue);
						var rightDate = ExpressionHelper.GetDateTime(rightValue);

						return leftDate.AddTicks(-rightDate.Ticks);
					}

					return Convert.ToDouble(leftValue) - Convert.ToDouble(rightValue);
				}
				case "--":
					return Convert.ToDouble(rightValue) - 1;
				case "=":
					//return leftValue + rightValue;
					break;
				case "==":
					return Equals(leftValue, rightValue);
				case "!=":
				case "<>":
					if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
					{
						var leftDate = ExpressionHelper.GetDateTime(leftValue);
						var rightDate = ExpressionHelper.GetDateTime(rightValue);

						return leftDate != rightDate;
					}
					return !Equals(leftValue, rightValue);
				case "<=":
					if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
					{
						var leftDate = ExpressionHelper.GetDateTime(leftValue);
						var rightDate = ExpressionHelper.GetDateTime(rightValue);

						return leftDate <= rightDate;
					}

					return Convert.ToDouble(leftValue) <= Convert.ToDouble(rightValue);
				case ">=":
					if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
					{
						var leftDate = ExpressionHelper.GetDateTime(leftValue);
						var rightDate = ExpressionHelper.GetDateTime(rightValue);

						return leftDate >= rightDate;
					}
					return Convert.ToDouble(leftValue) >= Convert.ToDouble(rightValue);
				//case "&&":
				//    return Convert.ToBoolean(leftValue) && Convert.ToBoolean(rightValue);
				//case "||":
				//    return Convert.ToBoolean(leftValue) || Convert.ToBoolean(rightValue);
				case "^":
					return Math.Pow(Convert.ToDouble(leftValue), Convert.ToDouble(rightValue));
				case "&":
					return Convert.ToInt64(leftValue) & Convert.ToInt64(rightValue);
				case "|":
					return Convert.ToInt64(leftValue) | Convert.ToInt64(rightValue);
				case "%":
					return Convert.ToDouble(leftValue) / 100 * Convert.ToDouble(rightValue);
				case "*":
					return Convert.ToDouble(leftValue) * Convert.ToDouble(rightValue);
				case "/":
				case "\\":
					return Convert.ToDouble(leftValue) / Convert.ToDouble(rightValue);
				case "<":
					if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
					{
						var leftDate = ExpressionHelper.GetDateTime(leftValue);
						var rightDate = ExpressionHelper.GetDateTime(rightValue);

						return leftDate < rightDate;
					}
					return Convert.ToDouble(leftValue) < Convert.ToDouble(rightValue);
				case ">":
					if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
					{
						var leftDate = ExpressionHelper.GetDateTime(leftValue);
						var rightDate = ExpressionHelper.GetDateTime(rightValue);

						return leftDate > rightDate;
					}

					return Convert.ToDouble(leftValue) > Convert.ToDouble(rightValue);
			}

			throw new Exception("Unknown operation");
		}

		private static bool IsDateTimeNode(ExpressionNode node, Object value)
		{
			return node.ValueType == ValueTypes.DateTime || (node.ValueType == ValueTypes.Variable && value is DateTime);
		}

		private static bool IsStringNode(ExpressionNode node, Object value)
		{
			return node.ValueType == ValueTypes.String || (node.ValueType == ValueTypes.Variable && value is String);
		}
	}
}