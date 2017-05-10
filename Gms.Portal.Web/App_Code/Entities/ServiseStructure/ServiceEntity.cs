using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.ServiseStructure
{
    [Serializable]
    [XmlRoot("Service")]
    public class ServiceEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Method")]
        public List<MethodEntity> Methods { get; set; }

        [XmlElement("Class")]
        public List<ClassEntity> Classes { get; set; }
    }
}