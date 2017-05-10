using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.ServiseStructure
{
    [Serializable]
    [XmlRoot("Parameter")]
    public class ParameterEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Type")]
        public String Type { get; set; }

        [XmlElement("IsOut")]
        public bool IsOut { get; set; }

        [XmlElement("OrderIndex")]
        public int OrderIndex { get; set; }

        [XmlElement("ReturnType")]
        public String ReturnType { get; set; }

        [XmlElement("IsPrimitive")]
        public bool IsPrimitive { get; set; }
    }
}