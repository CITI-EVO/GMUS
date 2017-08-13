using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [XmlRoot("Validation")]
    [Serializable]
    public class ValidationEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("ParentID")]
        public Guid? ParentID { get; set; }

        [XmlElement("ExecType")]
        public String ExecType { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("OwnerID")]
        public Guid? OwnerID { get; set; }

        [XmlElement("Expression")]
        public String Expression { get; set; }

        [XmlElement("ErrorMessage")]
        public String ErrorMessage { get; set; }
    }
}