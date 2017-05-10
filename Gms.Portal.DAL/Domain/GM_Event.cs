using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    public class GM_Event : IDbEntity
    {
        public virtual Guid ID { get; set; }

        public virtual String Name { get; set; }

        public virtual bool? Visible { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual XDocument XmlData { get; set; }
    }
}