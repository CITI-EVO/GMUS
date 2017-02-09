using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true, Namespace = "http://www.govtalk.gov.uk/CM/envelope")]
	public class AuthenticationEntity
	{
		[XmlElement("Method")]
		public string Method { get; set; }

		[XmlElement("Value")]
		public string Value { get; set; }
	}
}