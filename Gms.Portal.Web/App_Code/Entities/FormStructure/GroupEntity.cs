using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("Group")]
    public class GroupEntity : ContentEntity
    {
        [XmlElement("Size")]
        public int? Size { get; set; }

    }
}