using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [XmlRoot("MonitoringFlew")]
    [Serializable]
    public class MonitoringFlewEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Score")]
        public double? Score { get; set; }

        [XmlElement("Type")]
        public String Type { get; set; }
    }
}