using System;
using CITI.EVO.Tools.ExpressionEngine.Common;

namespace CITI.EVO.Tools.ExpressionEngine
{
	public class StringValueNode : ExpressionNode
	{
		public StringValueNode()
		{
			ValueType = ValueTypes.String;
		}

		public String Quota { get; set; }

		public override String ToString()
		{
			return String.Concat(Quota, Value, Quota);
		}
	}

}
