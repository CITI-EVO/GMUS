using System;
using System.Xml.Serialization;

namespace CITI.EVO.Tools.WorkflowEngine.Configs
{
	[XmlRoot]
	public class ExpressionConfig
	{
		[XmlElement]
		public String Expression { get; set; }

		[XmlElement]
		public String OutputType { get; set; }
	}
}