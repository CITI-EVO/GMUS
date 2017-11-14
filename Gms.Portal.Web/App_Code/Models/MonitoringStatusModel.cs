using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class MonitoringStatusModel
    {
        public Guid? RecordID { get; set; }

        public String Status { get; set; }

        public DateTime? ExpireDate { get; set; }

        public ISet<Guid?> FlawsID { get; set; }

        public String Comment { get; set; }
    }
}