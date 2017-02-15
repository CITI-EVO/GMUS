using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("NamedControl")]
    public class NamedControlEntity : ControlEntity
    {
        [XmlElement("Name")]
        public String Name { get; set; }
    }
}