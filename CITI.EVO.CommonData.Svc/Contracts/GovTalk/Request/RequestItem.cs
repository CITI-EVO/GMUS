using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts.GovTalk.Request
{
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2009/xmldsig11#")]
    [XmlRoot("Request", Namespace = "http://www.w3.org/2009/xmldsig11#", IsNullable = false)]
    public class RequestItem
    {
        [XmlAttribute]
        public String Id { get; set; }

        [XmlElement]
        public String SubcontractId { get; set; }

        [XmlElement]
        public String RequestReason { get; set; }

        [XmlElement]
        public String Signature { get; set; }

        [XmlElement]
        public Parameters Parameters { get; set; }
    }
}