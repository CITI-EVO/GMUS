using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlInclude(typeof(FieldEntity))]
    [XmlRoot("Tree")]
    public class TreeEntity : ContentEntity
    {
        [XmlElement("DetailsFormID")]
        public Guid? DetailsFormID { get; set; }

        [XmlElement("ValidationExp")]
        public String ValidationExp { get; set; }

        [XmlElement("ErrorMessage")]
        public String ErrorMessage { get; set; }
    }
}