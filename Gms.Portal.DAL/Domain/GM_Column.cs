using System;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_Column 
    {
        public Guid ID { get; set; }

        public Guid? TableID { get; set; }

        public String Name { get; set; }

        public String Type { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateChanged { get; set; }

        public DateTime? DateDeleted { get; set; }

        public GM_Table Table { get; set; }
    }
}