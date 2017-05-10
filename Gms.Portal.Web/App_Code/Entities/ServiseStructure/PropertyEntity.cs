using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.ServiseStructure
{
    [Serializable]
    [XmlRoot("Property")]
    public class PropertyEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Type")]
        public String Type { get; set; }

        [XmlElement("IsPrimitive")]
        public bool IsPrimitive { get; set; }
    }
}