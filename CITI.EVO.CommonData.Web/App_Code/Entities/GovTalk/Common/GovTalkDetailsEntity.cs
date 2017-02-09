using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("GovTalkDetails", IsNullable = false)]
	public class GovTalkDetailsEntity
	{
		//public ErrorEntity Error { get; set; }

		[XmlElement("GovTalkErrors")]
		public GovTalkErrorsEntity GovTalkErrors { get; set; }
	}
}