using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class MonitoringProjectItemModel
    {
        public Guid? ID { get; set; }

        public Guid? RecordID { get; set; }

        public Guid? TaskID { get; set; }
    }
}