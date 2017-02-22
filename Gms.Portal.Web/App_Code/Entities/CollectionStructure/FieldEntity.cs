using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.CollectionStructure
{
    [Serializable]
    [XmlRoot("Collection")]
    public class FieldEntity
    {
        [XmlAttribute("ID")]
        public Guid ID { get; set; }

        [XmlElement("Name")]
        public String  Name { get; set; }
    }
}