using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("SenderDetails", IsNullable = false)]
	public class SenderDetailsEntity
	{
		[XmlElement("IDAuthentication")]
		public IDAuthenticationEntity IDAuthentication { get; set; }

		[XmlElement("EmailAddress")]
		public String EmailAddress { get; set; }
	}
}