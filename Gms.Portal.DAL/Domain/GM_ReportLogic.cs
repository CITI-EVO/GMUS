using System;
using System.Xml.Linq;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_ReportLogic 
    {
        public Guid ID { get; set; }

        public Guid? ReportID { get; set; }

        public Guid? LogicID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateChanged { get; set; }

        public DateTime? DateDeleted { get; set; }

        public XDocument ConfigXml { get; set; }

        public string Type { get; set; }

        public string GeneralType { get; set; }

        public GM_Logic Logic { get; set; }

        public GM_Report Report { get; set; }
    }
}