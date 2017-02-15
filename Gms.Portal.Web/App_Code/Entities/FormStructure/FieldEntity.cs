using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("Field")]
    public class FieldEntity : NamedControlEntity
    {
        [XmlAttribute("OrderIndex")]
        public int OrderIndex { get; set; }

        [XmlElement("Type")]
        public String Type { get; set; }

        [XmlElement("Mask")]
        public String Mask { get; set; }

        [XmlAttribute("Visible")]
        public bool Visible { get; set; }

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        [XmlAttribute("IsMandatory")]
        public bool IsMandatory { get; set; }

        [XmlElement("ValidationExp")]
        public String ValidationExp { get; set; }

        [XmlElement("Tag")]
        public String Tag { get; set; }

        [XmlElement("PossibleValues")]
        public List<NameValueEntity> PossibleValues { get; set; }
    }
}