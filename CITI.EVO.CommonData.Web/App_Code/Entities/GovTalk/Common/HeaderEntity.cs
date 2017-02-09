using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("Header", IsNullable = false)]
	public class HeaderEntity
	{
		[XmlElement("MessageDetails")]
		public MessageDetailsEntity MessageDetails { get; set; }

		[XmlElement("SenderDetails")]
		public SenderDetailsEntity SenderDetails { get; set; }
	}
}