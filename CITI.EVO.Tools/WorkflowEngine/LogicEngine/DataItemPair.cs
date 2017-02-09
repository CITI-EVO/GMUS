using System;
using System.Collections.Generic;
using CITI.EVO.Tools.WorkflowEngine.LogicEngine.Interfaces;

namespace CITI.EVO.Tools.WorkflowEngine.LogicEngine
{
	public class DataItemPair : Dictionary<String, Object>, IDataItem
	{
		public DataItemPair()
		{
		}

		public DataItemPair(IEnumerable<KeyValuePair<String, Object>> collection)
		{
			foreach (var pair in collection)
				Add(pair.Key, pair.Value);
		}
	}
}