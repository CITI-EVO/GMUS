using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.LogicStructure
{
    [Serializable]
    [XmlRoot("Expression")]
    public class ExpressionEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("Expression")]
        public String Expression { get; set; }

        [XmlElement("OutputType")]
        public String OutputType { get; set; }
    }
}