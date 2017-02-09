using System;

namespace CITI.EVO.Tools.WorkflowEngine.LogicEngine
{
	[Serializable]
	public class TypedNameValueEntry
	{
		public String Name { get; set; }
		public Object Value { get; set; }
		public Type Type { get; set; }
	}
}