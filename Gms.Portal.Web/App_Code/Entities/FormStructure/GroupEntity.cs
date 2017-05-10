using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlInclude(typeof(ContentEntity))]
    [XmlInclude(typeof(FieldEntity))]
    [XmlInclude(typeof(FormEntity))]
    [XmlInclude(typeof(GridEntity))]
    [XmlInclude(typeof(TreeEntity))]
    [XmlInclude(typeof(GroupEntity))]
    [XmlInclude(typeof(TabContainerEntity))]
    [XmlInclude(typeof(TabPageEntity))]
    [XmlRoot("Group")]
    public class GroupEntity : ContentEntity
    {
        [XmlElement("Size")]
        public int? Size { get; set; }

        [XmlElement("BgColor")]
        public String BgColor { get; set; }

        [XmlElement("TextColor")]
        public String TextColor { get; set; }
    }
}