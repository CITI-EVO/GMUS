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
    [XmlRoot("Control")]
    public class ControlEntity
    {
        [XmlAttribute("ID")]
        public Guid ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Alias")]
        public String Alias { get; set; }

        [XmlAttribute("OrderIndex")]
        public int OrderIndex { get; set; }

        [XmlAttribute("Visible")]
        public bool Visible { get; set; }

        [XmlAttribute("VisibleExpression")]
        public String VisibleExpression { get; set; }

        [XmlElement("DependentFieldID")]
        public Guid? DependentFieldID { get; set; }

        [XmlElement("DependentExp")]
        public String DependentExp { get; set; }

        [XmlElement("FirstTimeFill")]
        public bool FirstTimeFill { get; set; }

        [XmlElement("NotPrintable")]
        public bool? NotPrintable { get; set; }
    }
}