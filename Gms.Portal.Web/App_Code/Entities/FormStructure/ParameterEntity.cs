using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("Parameter")]
    public class ParameterEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Expression")]
        public String Expression { get; set; }
    }
}