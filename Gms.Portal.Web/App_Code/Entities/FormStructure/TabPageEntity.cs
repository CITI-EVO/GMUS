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
    [XmlRoot("TabPage")]
    public class TabPageEntity : ContentEntity
    {
    }
}