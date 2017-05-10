using System;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormStatusUnit
    {
        public int? Step { get; set; }

        public Guid? UserID { get; set; }

        public Guid? StatusID { get; set; }

        public DateTime? DateOfStatus { get; set; }
    }
}