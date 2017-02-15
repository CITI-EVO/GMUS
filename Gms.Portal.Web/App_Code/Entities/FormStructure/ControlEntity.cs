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
    }
}