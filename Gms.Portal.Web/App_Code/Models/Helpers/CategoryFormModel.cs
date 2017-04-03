using System;

namespace Gms.Portal.Web.Models.Helpers
{
    [Serializable]
    public class CategoryFormModel
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public CategoriesFormsModel Children { get; set; }
    }
}