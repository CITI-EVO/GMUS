using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.EventStructure
{
    [Serializable]
    [XmlRoot("Event")]
    public class EventEntity
    {
        [XmlElement]
        public Guid? ID { get; set; }

        [XmlElement]
        public String Name { get; set; }


        [XmlElement]
        public List<PhaseEntity> Phases { get; set; }

    }
}