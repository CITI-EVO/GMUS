using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts.GovTalk.Response
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2009/xmldsig11#")]
    [XmlRoot("Response", Namespace = "http://www.w3.org/2009/xmldsig11#", IsNullable = false)]
    public class ResponseItem
    {
        [XmlAttribute]
        public String Id { get; set; }

        [XmlAttribute]
        public String TimeStamp { get; set; }

        [XmlAttribute]
        public String ReferenceId { get; set; }

        [XmlElement]
        public ResultStatus ResultStatus { get; set; }

        [XmlElement("Person", typeof(PersonInfo))]
        public ResultData ReponseData { get; set; }

        [XmlElement]
        public String Signature { get; set; }
    }
}