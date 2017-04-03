using System;
using System.Collections.Generic;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    public class GM_Category : IDbEntity
    {
        public virtual Guid ID { get; set; }

        public virtual Guid? ParentID { get; set; }

        public virtual String Name { get; set; }

        public virtual int? OrderIndex { get; set; }

        public virtual bool? Visible { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual ICollection<GM_Form> Forms { get; set; }

        public virtual ICollection<GM_Category> Children { get; set; }
    }
}