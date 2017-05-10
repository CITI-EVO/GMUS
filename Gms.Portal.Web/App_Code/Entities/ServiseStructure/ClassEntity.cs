using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.ServiseStructure
{
    [Serializable]
    [XmlRoot("Class")]
    public class ClassEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Property")]
        public List<PropertyEntity> Properties { get; set; }
    }
}