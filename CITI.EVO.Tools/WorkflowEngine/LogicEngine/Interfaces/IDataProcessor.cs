using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.WorkflowEngine.LogicEngine.Interfaces
{
	public interface IDataProcessor
	{
		IList<String> FilterByFields { get; }
		IList<String> OrderByFields { get; }
		IList<String> GroupByFields { get; }
		IList<String> SelectFields { get; }

		IList<String> OutputFields { get; }

		IEnumerable<IDataItem> Load(IEnumerable<IDataItem> collection);
	}
}
