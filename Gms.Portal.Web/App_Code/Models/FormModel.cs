using System;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class FormModel
    {
        public Guid ID { get; set; }

        public String Name { get; set; }

        public String Number { get; set; }

        public int? OrderIndex { get; set; }

        public bool? Visible { get; set; }

        public FormEntity Entity { get; set; }
    }
}