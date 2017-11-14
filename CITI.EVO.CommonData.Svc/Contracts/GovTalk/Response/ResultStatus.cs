using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts.GovTalk.Response
{
    [Serializable]
    [XmlRoot("ResultStatus")]
    public class ResultStatus
    {
        [XmlElement]
        public String Code { get; set; }

        [XmlElement]
        public String Message { get; set; }
    }
}