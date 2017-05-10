using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.EventStructure
{
    [Serializable]
    [XmlRoot("Phase")]
    public class PhaseEntity
    {
        [XmlElement]
        public Guid? ID { get; set; }

        [XmlElement]
        public String Name { get; set; }

        [XmlElement]
        public String Description { get; set; }

        [XmlElement]
        public DateTime? StartDate { get; set; }

        [XmlElement]
        public DateTime? EndDate { get; set; }

        [XmlElement]
        public String Color { get; set; }

        [XmlElement]
        public Guid? FormID { get; set; }
    }
}