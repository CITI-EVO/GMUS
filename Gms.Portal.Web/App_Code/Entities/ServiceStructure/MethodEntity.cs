using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.ServiceStructure
{
    [Serializable]
    [XmlRoot("Method")]
    public class MethodEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Return")]
        public ReturnEntity Return { get; set; }
        
        [XmlElement("Parameter")]
        public List<ParameterEntity> Parameters { get; set; }
    }
}