using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts.GovTalk.Response
{
    [Serializable]
    [XmlRoot("PersonAdditionalStatuses")]
    public class PersonAdditionalStatuses
    {

        [XmlElement]
        public string DeActID { get; set; }

        [XmlElement]
        public string DeActRegDate { get; set; }

        [XmlElement]
        public string DeDeathDate { get; set; }
    }
}