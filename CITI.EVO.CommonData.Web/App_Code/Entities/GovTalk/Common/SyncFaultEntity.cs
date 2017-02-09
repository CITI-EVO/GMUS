using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("syncfault", IsNullable = false)]
	public class SyncFaultEntity : BaseMessageEntity
	{
		[XmlElement("source")]
		public String Source { get; set; }

		[XmlElement("correlationid")]
		public String CorrelationID { get; set; }

		[XmlElement("message")]
		public String Message { get; set; }
	}
}