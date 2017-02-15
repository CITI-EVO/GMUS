using System;
using System.Collections.Generic;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_Report 
    {
        public Guid ID { get; set; }

        public Guid CategoryID { get; set; }

        public String Type { get; set; }

        public String Name { get; set; }

        public bool? Public { get; set; }

        public int? XLabelAngle { get; set; }

        public String Language { get; set; }

        public String Description { get; set; }

        public String Interpretation { get; set; }

        public String InformationSource { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateChanged { get; set; }

        public DateTime? DateDeleted { get; set; }

        public ICollection<GM_ReportLogic> ReportLogics { get; set; }
    }
}
