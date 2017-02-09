using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("Error", IsNullable = false)]
	public class ErrorEntity
	{
		[XmlElement("RaisedBy")]
		public String RaisedBy { get; set; }

		[XmlElement("Number")]
		public int Number { get; set; }

		[XmlElement("Type")]
		public String Type { get; set; }

		[XmlElement("Text")]
		public String Text { get; set; }

		[XmlElement("Location")]
		public String EnvelopeVersion { get; set; }
	}
}