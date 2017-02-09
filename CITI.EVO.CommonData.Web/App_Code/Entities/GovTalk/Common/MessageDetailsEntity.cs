using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("MessageDetails", IsNullable = false)]
	public class MessageDetailsEntity
	{
		[XmlElement("Class")]
		public String Class { get; set; }

		[XmlElement("Qualifier")]
		public String Qualifier { get; set; }

		[XmlElement("Function")]
		public String Function { get; set; }

		[XmlElement("CorrelationID")]
		public String CorrelationID { get; set; }

		[XmlElement("TransactionID")]
		public String TransactionID { get; set; }

		[XmlElement("GatewayTimestamp")]
		public String GatewayTimestamp { get; set; }

		[XmlElement("ResponseEndPoint")]
		public ResponseEndPointEntity ResponseEndPoint { get; set; }
	}
}