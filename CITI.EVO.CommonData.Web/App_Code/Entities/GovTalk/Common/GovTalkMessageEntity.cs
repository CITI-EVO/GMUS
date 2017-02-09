using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("GovTalkMessage", IsNullable = false, Namespace = "http://www.govtalk.gov.uk/CM/envelope")]
	public class GovTalkMessageEntity
	{
		[XmlElement("EnvelopeVersion")]
		public decimal EnvelopeVersion { get; set; }

		[XmlElement("Header")]
		public HeaderEntity Header { get; set; }

		[XmlElement("Body")]
		public BodyEntity Body { get; set; }

		[XmlElement("GovTalkDetails")]
		public GovTalkDetailsEntity GovTalkDetails { get; set; }
	}
}