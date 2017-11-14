using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [XmlRoot("Monitoring")]
    [Serializable]
    public class MonitoringEntity
    {
        [XmlElement("Flew")]
        public List<MonitoringFlewEntity> Flews { get; set; }

        [XmlElement("PrintField")]
        public List<MonitoringPrintFieldEntity> PrintFields { get; set; }
    }
}