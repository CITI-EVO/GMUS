using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_Logic 
    {
        public Guid ID { get; set; }

        public Guid? TableID { get; set; }

        public String Name { get; set; }

        public String Type { get; set; }

        public Guid? LogicID { get; set; }

        public String SourceType { get; set; }

        public XDocument RawData { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateChanged { get; set; }

        public DateTime? DateDeleted { get; set; }

        public GM_Table Table { get; set; }

        public GM_Logic Child { get; set; }

        public ICollection<GM_Logic> Parents { get; set; }

        public ICollection<GM_ReportLogic> ReportLogics { get; set; }
    }
}