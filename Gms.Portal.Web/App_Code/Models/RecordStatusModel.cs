using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class RecordStatusModel
    {
        public Guid? RecordID { get; set; }

        public Guid? StatusID { get; set; }

        public String Description { get; set; }
    }
}