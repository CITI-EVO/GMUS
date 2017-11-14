using System;

namespace Gms.Portal.Web.Entities.Monitoring
{
    [Serializable]
    public class ProjectItemInfoEntity
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}