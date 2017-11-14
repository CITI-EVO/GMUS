using System;

namespace Gms.Portal.Web.Models
{
    public class MonitoringFlawModel
    {
        public Guid? ID { get; set; }
        public String Name { get; set; }
        public double? Score { get; set; }
        public String Type { get; set; }
    }
}