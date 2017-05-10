using System;
using Gms.Portal.Web.Entities.EventStructure;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class EventModel
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public bool? Visible { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateChanged { get; set; }
        public DateTime? DateDeleted { get; set; }

        public EventEntity Entity { get; set; }
    }
}