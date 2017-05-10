using System;
using System.Collections.Generic;
using CITI.EVO.Tools.ExpressionEngine.Common;

namespace CITI.EVO.Tools.ExpressionEngine
{
	public class ExpressionNode
	{
		public ExpressionNode()
		{
			Params = new List<ExpressionNode>();
		}

		public String Action { get; set; }
		public ActionTypes ActionType { get; set; }

		public Object Value { get; set; }
		public ValueTypes ValueType { get; set; }

		public IList<ExpressionNode> Params { get; set; }

		public bool Adverse { get; set; }

		public override String ToString()
		{
			switch (ActionType)
			{
				case ActionTypes.Function:
				{
					if (Params == null || Params.Count == 0)
						return $"{Action}()";

					var strParams = new String[Params.Count];
					for (int i = 0; i < Params.Count; i++)
						strParams[i] = Params[i].ToString();

					var args = String.Join(", ", strParams);
					return $"{Action}({args})";
				}
				case ActionTypes.Operator:
				{
					if (!ExpressionHelper.IsEmptyOrSpace(Action))
						return $"{Params[0]} {Action} {Params[1]}";

					return Convert.ToString(Value);
				}
			}

			switch (ValueType)
			{
				case ValueTypes.Number:
					return Convert.ToString(Value);
				case ValueTypes.String:
					return $"'{Value}'";
				case ValueTypes.DateTime:
					return $"[{Value:dd.MM.yyyy HH:mm:ss}]";
				case ValueTypes.Variable:
					return Action;
			}

			return base.ToString();
		}
	}
}