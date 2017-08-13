using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [XmlRoot("Rate")]
    [Serializable]
    public class RateEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("ParentID")]
        public Guid? ParentID { get; set; }

        [XmlElement("Number")]
        public String Number { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("MinScore")]
        public int? MinScore { get; set; }

        [XmlElement("MaxScore")]
        public int? MaxScore { get; set; }
    }
}