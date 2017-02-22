using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.LogicStructure
{
    [Serializable]
    [XmlRoot("LogicQuery")]
    public class LogicQueryEntity : LogicContainerEntity
    {
        [XmlElement("Text")]
        public String Text { get; set; }
    }
}