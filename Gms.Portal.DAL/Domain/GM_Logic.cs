using System;
using System.Collections.Generic;
using System.Xml.Linq;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_Logic : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Type { get; set; }
        public virtual String SourceType { get; set; }
        public virtual XDocument RawData { get; set; }
        public virtual Guid? FormID { get; set; }
        public virtual Guid? LogicID { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual ICollection<GM_ReportLogic> ReportLogics { get; set; }
    }
}