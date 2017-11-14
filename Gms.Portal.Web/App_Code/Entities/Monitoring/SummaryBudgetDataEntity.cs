using System;
using System.Collections.Generic;
using Gms.Portal.Web.Entities.DataContainer;

namespace Gms.Portal.Web.Entities.Monitoring
{
    [Serializable]
    public class SummaryBudgetDataEntity
    {
        public Guid? Key { get; set; }

        public String Name { get; set; }

        public FormDataListBase Data { get; set; }

        public IList<BudgetAmountFieldEntity> Fields { get; set; }
    }
}