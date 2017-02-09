using System;
using CITI.EVO.Tools.ExpressionEngine.Common;

namespace CITI.EVO.Tools.ExpressionEngine
{
	public class ExpressionEvaluator
	{
		internal readonly Func<string, object> variableResolver;

		public ExpressionEvaluator()
		{
		}
		public ExpressionEvaluator(Func<String, Object> variableResolver)
		{
			this.variableResolver = variableResolver;
		}

		public Object Eval(String expression)
		{
			var node = ExpressionParser.Parse(expression);
			return Eval(node);
		}

		public Object Eval(ExpressionNode node)
		{
			if (node == null)
			{
				return null;
			}

			switch (node.ActionType)
			{
				case ActionTypes.Function:
					return FunctionEvaluator.Eval(node, this);
				case ActionTypes.Operator:
					return OperatorEvaluator.Eval(node, this);
			}

			switch (node.ValueType)
			{
				case ValueTypes.Variable:
				{
					if (String.Equals(node.Action, "e", StringComparison.OrdinalIgnoreCase))
					{
						return Math.E;
					}

					if (String.Equals(node.Action, "pi", StringComparison.OrdinalIgnoreCase))
					{
						return Math.PI;
					}

					if (String.Equals(node.Action, "true", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

					if (String.Equals(node.Action, "false", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}

					return variableResolver(node.Action);
				}
			}

			return node.Value;
		}
	}
}