using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class PrintTemplateModel
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public String Type { get; set; }

        public String Role { get; set; }

        public String Layout { get; set; }

        public String Template { get; set; }
    }
}