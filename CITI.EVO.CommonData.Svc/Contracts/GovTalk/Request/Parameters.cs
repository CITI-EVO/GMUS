using System;
using System.Xml.Serialization;
using CITI.EVO.CommonData.Svc.Contracts.GovTalk.Response;

namespace CITI.EVO.CommonData.Svc.Contracts.GovTalk.Request
{
    [Serializable]
    [XmlRoot("Parameters")]
    public class Parameters
    {
        [XmlElement("Person", typeof(PersonInfo))]
        public ParameterItem Item { get; set; }
    }
} 
