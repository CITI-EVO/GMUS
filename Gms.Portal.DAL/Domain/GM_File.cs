using System;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public class GM_File : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid ParentID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String FileName { get; set; }
        public virtual byte[] FileData { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}
