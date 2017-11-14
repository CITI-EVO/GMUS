using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts.GovTalk.Response
{
    [Serializable]
    [XmlRoot("Registration")]
    public class Registration
    {
        [XmlElement]
        public string OA_ID { get; set; }

        [XmlElement]
        public string AddrStatus { get; set; }

        [XmlElement]
        public string AddrStatusID { get; set; }

        [XmlElement]
        public string ActiveAddress { get; set; }

        [XmlElement]
        public string ActiveAddressAddressID { get; set; }

        [XmlElement]
        public string ActiveAddressAddressSource { get; set; }

        [XmlElement]
        public string ActiveAddressAddressSourceID { get; set; }

        [XmlElement]
        public string ActiveAddressCountry { get; set; }

        [XmlElement]
        public string ActiveAddressRegion { get; set; }

        [XmlElement]
        public string ActiveAddressRaion { get; set; }

        [XmlElement]
        public string ActiveAddressCity { get; set; }

        [XmlElement]
        public string ActiveAddressTownship { get; set; }

        [XmlElement]
        public string ActiveAddressVillage { get; set; }
    }
}