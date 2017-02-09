using CITI.EVO.Tools.ExpressionEngine.Common;

namespace CITI.EVO.Tools.ExpressionEngine
{
	public class OperatorNode : ExpressionNode
	{
		public OperatorNode()
		{
			Params.Add(null);
			Params.Add(null);

			ActionType = ActionTypes.Operator;
		}

		public ExpressionNode Left
		{
			get { return Params[0]; }
			set { Params[0] = value; }
		}

		public ExpressionNode Right
		{
			get { return Params[1]; }
			set { Params[1] = value; }
		}
	}
}