using System;
using System.Collections.Generic;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public class GM_RecipientGroup : IDbEntity
    {
        public virtual Guid ID { get; set; }

        public virtual String Name { get; set; }
        public virtual String Description { get; set; }

        public virtual String Type { get; set; }

        public virtual Guid? FormID { get; set; }

        public virtual String Expression { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual ICollection<GM_Recipient> Recipients { get; set; }

    }
}