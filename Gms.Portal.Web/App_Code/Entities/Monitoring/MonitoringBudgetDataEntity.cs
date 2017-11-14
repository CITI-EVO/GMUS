
using System;

namespace Gms.Portal.Web.Entities.Monitoring
{
    public class MonitoringBudgetDataEntity
    {
        public DateTime? DateOfTransfer { get; set; }
        public double? Incoming { get; set; }
        public double? Outgoing { get; set; }
        public double? Remain { get; set; }
        public string Paragraph { get; set; }
        public string Goal { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string StatusUser { get; set; }
        public DateTime? StatusDate { get; set; }
        public string Flaws { get; set; }
        public double? FlawsScore { get; set; }
        public DateTime DateCreated { get; set; }
    }
}