using System;
using Gms.Portal.Web.Entities.CollectionStructure;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class CollectionModel
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public CollectionEntity Entity { get; set; }
    }
}