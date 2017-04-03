using System;
using System.Xml.Linq;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_ReportLogic : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid? ReportID { get; set; }
        public virtual Guid? LogicID { get; set; }
        public virtual String Type { get; set; }
        public virtual String GeneralType { get; set; }
        public virtual XDocument ConfigXml { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}