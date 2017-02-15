using System;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Models
{
    public class FormModel
    {
        public Guid ID { get; set; }

        public String Name { get; set; }

        public String Number { get; set; }

        public String Language { get; set; }

        public FormEntity FormEntity { get; set; }
    }
}