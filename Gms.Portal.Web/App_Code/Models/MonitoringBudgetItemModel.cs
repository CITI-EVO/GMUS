using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class MonitoringBudgetItemModel
    {
        public Guid? ID { get; set; }

        public Guid? RecordID { get; set; }

        public Guid? ParagraphID { get; set; }

        public String Goal { get; set; }

        public String Type { get; set; }

        public double? Amount { get; set; }

        public DateTime? DateOfTransfer { get; set; }
    }
}