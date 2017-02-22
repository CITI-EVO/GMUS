using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("Content")]
    public class ContentEntity : ControlEntity
    {
        [XmlElement("Grid", typeof(GridEntity))]
        [XmlElement("Field", typeof(FieldEntity))]
        [XmlElement("Group", typeof(GroupEntity))]
        [XmlElement("TabPage", typeof(TabPageEntity))]
        [XmlElement("TabContainer", typeof(TabContainerEntity))]
        public List<ControlEntity> Controls { get; set; }
    }
}