using System;

namespace Gms.Portal.Web.Entities.Monitoring
{
    [Serializable]
    public class SummaryBudgetEntity
    {
        public String Paragraph { get; set; }

        public double? PlanGrossing { get; set; }
        public double? PlanCurrent { get; set; }

        public double? RemainPrevious { get; set; }
        public double? RemainCurrent { get; set; }

        public double? EnrollGrossing { get; set; }
        public double? EnrollCurrent { get; set; }

        public double? CashExpenseGrossing { get; set; }
        public double? CashExpenseCurrent { get; set; }

        public double? ConfirmedExpenseGrossing { get; set; }
        public double? ConfirmedExpenseCurrent { get; set; }

        public double? UnconfirmedExpenseGrossing { get; set; }
        public double? UnconfirmedExpenseCurrent { get; set; }
    }
}