using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class MonitoringItemModel
    {
        public Guid? ID { get; set; }

        public Guid? RecordID { get; set; }

        public Guid? BudgetID { get; set; }

        public Guid? GoalID { get; set; }

        public String Type { get; set; }

        public double? Amount { get; set; }

        public DateTime? DateOfTransfer { get; set; }
    }
}