using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public class GM_Recipient : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid GroupID { get; set; }
        public virtual Guid UserID { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}
