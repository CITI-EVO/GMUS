using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.LogicStructure
{
    [Serializable]
    [XmlRoot("Logic")]
    public class LogicEntity
    {
        [XmlElement("Query", typeof(LogicQueryEntity))]
        [XmlElement("Expressions", typeof(LogicExpressionsEntity))]
        public LogicContainerEntity Logic { get; set; }
    }
}