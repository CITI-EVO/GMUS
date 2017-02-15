using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("ContainerControl")]
    public class ContainerControlEntity : NamedControlEntity
    {
        [XmlAttribute("Visible")]
        public bool Visible { get; set; }

        [XmlElement("Grid", typeof(GridEntity))]
        [XmlElement("Field", typeof(FieldEntity))]
        [XmlElement("Group", typeof(GroupEntity))]
        [XmlElement("TabPage", typeof(TabPageEntity))]
        [XmlElement("TabContainer", typeof(TabContainerEntity))]
        public List<ControlEntity> Controls { get; set; }
    }
}