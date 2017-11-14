using System;
using System.Collections.Generic;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Models
{
    public class MonitoringPrintFieldsModel
    {
        public Guid? ID { get; set; }
        public Guid? FormID { get; set; }
        public String PrintType { get; set; }
        public String Name { get; set; }
        public String Template { get; set; }
        public Boolean? IsLendscape { get; set; }
        public Boolean? BudgetForm { get; set; }
        public Boolean? SummaryBudgetForm { get; set; }
        public Boolean? ProjectsForm { get; set; }
    }
}