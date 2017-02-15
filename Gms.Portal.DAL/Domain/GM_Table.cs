using System;
using System.Collections.Generic;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_Table 
    {
        public Guid ID { get; set; }

        public String Name { get; set; }

        public String Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateChanged { get; set; }

        public DateTime? DateDeleted { get; set; }

        public ICollection<GM_Column> Columns { get; set; }

        public ICollection<GM_Logic> Logics { get; set; }
    }
}