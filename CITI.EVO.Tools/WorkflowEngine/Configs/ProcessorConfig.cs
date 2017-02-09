using System.Collections.Generic;
using System.Xml.Serialization;

namespace CITI.EVO.Tools.WorkflowEngine.Configs
{
	[XmlRoot]
	public class ProcessorConfig
	{
		[XmlElement]
		public List<ExpressionConfig> FilterByFields { get; set; }

		[XmlElement]
		public List<ExpressionConfig> OrderByFields { get; set; }

		[XmlElement]
		public List<NamedFieldConfig> GroupByFields { get; set; }

		[XmlElement]
		public List<NamedFieldConfig> SelectFields { get; set; }
	}
}
