using System;
using System.Collections.Generic;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_Report 
    {
        public virtual Guid ID { get; set; }
        public virtual Guid CategoryID { get; set; }
        public virtual String Type { get; set; }
        public virtual String Name { get; set; }
        public virtual bool? Public { get; set; }
        public virtual int? XLabelAngle { get; set; }
        public virtual String Language { get; set; }
        public virtual String Description { get; set; }
        public virtual String Interpretation { get; set; }
        public virtual String InformationSource { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual ICollection<GM_ReportLogic> ReportLogics { get; set; }
    }
}
