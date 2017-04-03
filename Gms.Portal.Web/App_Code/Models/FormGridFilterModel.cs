using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class FormGridFilterModel
    {
        public Guid? StatusID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}