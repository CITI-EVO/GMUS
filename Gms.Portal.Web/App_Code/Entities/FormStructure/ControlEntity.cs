using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("Control")]
    public class ControlEntity
    {
        [XmlAttribute("ID")]
        public Guid ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlAttribute("OrderIndex")]
        public int OrderIndex { get; set; }

        [XmlAttribute("Visible")]
        public bool Visible { get; set; }

        [XmlElement("DependentFieldID")]
        public Guid? DependentFieldID { get; set; }

        [XmlElement("DependentExp")]
        public String DependentExp { get; set; }
    }
}