using System;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class IDNameEntity
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }
    }
}