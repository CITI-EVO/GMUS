using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Models
{
    public class MonitoringProjectFlawItemModel
    {
        public Guid? ID { get; set; }

        public String Status { get; set; }

        public ISet<Guid?> FlawsID { get; set; }

        public DateTime? ExpireDate { get; set; }

        public String Comment { get; set; }
    }
}