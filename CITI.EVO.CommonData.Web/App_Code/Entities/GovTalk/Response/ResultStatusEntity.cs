using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Response
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("ResultStatus", IsNullable = false)]
	public class ResultStatusEntity
	{
		[XmlElement("StatusCode")]
		public String StatusCode { get; set; }

		[XmlElement("StatusDescription")]
		public String StatusDescription { get; set; }
	}
}