using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("ResponseEndPoint", IsNullable = false)]
	public class ResponseEndPointEntity
	{
		[XmlAttribute("PollInterval")]
		public int PollInterval { get; set; }

		[XmlText]
		public String Value { get; set; }
	}
}