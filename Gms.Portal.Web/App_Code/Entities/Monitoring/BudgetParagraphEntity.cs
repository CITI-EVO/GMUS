using System;

namespace Gms.Portal.Web.Entities.Monitoring
{
    [Serializable]
    public class BudgetParagraphEntity
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public Guid? FieldID { get; set; }

        public Guid? ContentID { get; set; }

        public Guid? ParentID { get; set; }

        public bool Container { get; set; }

        public String Category { get; set; }

        public Object DependOnValue { get; set; }
    }
}