using System;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Entities.Monitoring
{
    [Serializable]
    public class BudgetAmountFieldEntity
    {
        public String Category { get; set; }

        public Guid? ParentID { get; set; }

        public int?  OrderIndex { get; set; }

        public ControlEntity Control { get; set; }
    }
}