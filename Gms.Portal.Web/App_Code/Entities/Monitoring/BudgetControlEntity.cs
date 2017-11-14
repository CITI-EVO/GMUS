using System;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Entities.Monitoring
{
    public class BudgetControlEntity
    {
        public BudgetControlEntity()
        {
        }

        public Guid? Key { get; set; }

        public String Name { get; set; }

        public bool DependOnRow { get; set; }

        public Object DependOnValue { get; set; }

        public FieldEntity Field { get; set; }

        public ContentEntity Content { get; set; }
    }
}