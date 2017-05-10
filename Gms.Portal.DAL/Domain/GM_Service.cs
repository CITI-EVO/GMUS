using System;
using System.Xml.Linq;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_Service : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual String Url { get; set; }
        public virtual String Name { get; set; }
        public virtual XDocument XmlData { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}