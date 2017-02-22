using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("Field")]
    public class FieldEntity : ControlEntity
    {
        [XmlElement("Type")]
        public String Type { get; set; }

        [XmlElement("Mask")]
        public String Mask { get; set; }

        [XmlElement("Tag")]
        public String Tag { get; set; }

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        [XmlElement("Privacy")]
        public bool? Privacy { get; set; }

        [XmlElement("Mandatory")]
        public bool? Mandatory { get; set; }

        [XmlElement("Description")]
        public String Description { get; set; }

        [XmlAttribute("DisplayOnGrid")]
        public bool DisplayOnGrid { get; set; }

        [XmlElement("ValidationExp")]
        public String ValidationExp { get; set; }

        [XmlElement("DataSourceID")]
        public Guid? DataSourceID { get; set; }

        [XmlElement("TextExpression")]
        public String TextExpression { get; set; }

        [XmlElement("ValueExpression")]
        public String ValueExpression { get; set; }
    }
}