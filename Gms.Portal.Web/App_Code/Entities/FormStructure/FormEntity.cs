using System;
using System.Collections.Generic;
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
    [XmlInclude(typeof(RatingEntity))]
    [XmlInclude(typeof(TemplateEntity))]
    [XmlInclude(typeof(TabContainerEntity))]
    [XmlInclude(typeof(TabPageEntity))]
    [XmlRoot("Form")]
    public class FormEntity : ContentEntity
    {
        [XmlElement("Ratings")]
        public RatingEntity Rating { get; set; }

        [XmlElement("Templates")]
        public List<TemplateEntity> Templates { get; set; }

        [XmlElement("Validations")]
        public List<ValidationEntity> Validations { get; set; }
    }
}