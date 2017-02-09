using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("IDAuthentication", IsNullable = false)]
	public class IDAuthenticationEntity
	{
		[XmlElement("SenderID")]
		public String SenderID { get; set; }

		[XmlElement("Authentication")]
		public AuthenticationEntity Authentication { get; set; }
	}
}