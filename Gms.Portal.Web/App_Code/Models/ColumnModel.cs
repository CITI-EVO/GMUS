using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class ColumnModel
    {
        public Guid? ID { get; set; }

        public Guid? TableID { get; set; }

        public bool IsPrimary { get; set; }

        public String Name { get; set; }

        public String Type { get; set; }
    }
}