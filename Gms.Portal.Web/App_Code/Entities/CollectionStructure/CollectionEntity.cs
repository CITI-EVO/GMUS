using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Entities.CollectionStructure
{
    [Serializable]
    [XmlRoot("Collection")]
    public class CollectionEntity
    {
        [XmlAttribute("ID")]
        public Guid ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Field")]
        public List<FieldEntity> Fields { get; set; }
    }
}