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
						return String.Format("{0}()", Action);

					var strParams = new String[Params.Count];
					for (int i = 0; i < Params.Count; i++)
						strParams[i] = Params[i].ToString();

					var args = String.Join(", ", strParams);
					return String.Format("{0}({1})", Action, args);
				}
				case ActionTypes.Operator:
				{
					if (!ExpressionHelper.IsEmptyOrSpace(Action))
						return String.Format("{0} {1} {2}", Params[0], Action, Params[1]);

					return Convert.ToString(Value);
				}
			}

			switch (ValueType)
			{
				case ValueTypes.Number:
					return Convert.ToString(Value);
				case ValueTypes.String:
					return String.Format("'{0}'", Value);
				case ValueTypes.DateTime:
					return String.Format("[{0:dd.MM.yyyy HH:mm:ss}]", Value);
				case ValueTypes.Variable:
					return Action;
			}

			return base.ToString();
		}
	}
}