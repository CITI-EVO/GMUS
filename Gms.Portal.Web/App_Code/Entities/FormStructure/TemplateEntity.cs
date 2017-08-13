using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [XmlRoot("Template")]
    [Serializable]
    public class TemplateEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Role")]
        public String Role { get; set; }

        [XmlElement("Type")]
        public String Type { get; set; }

        [XmlElement("Layout")]
        public String Layout { get; set; }

        [XmlElement("Template")]
        public String Template { get; set; }
    }
}