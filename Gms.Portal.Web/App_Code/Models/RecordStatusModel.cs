using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class RecordStatusModel
    {
        public Guid? FormID { get; set; }

        public Guid? RecordID { get; set; }

        public Guid? StatusID { get; set; }

        public String SourceField { get; set; }

        public String Description { get; set; }
    }
}