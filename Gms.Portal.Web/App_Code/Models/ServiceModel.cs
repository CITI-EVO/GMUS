using System;
using Gms.Portal.Web.Entities.ServiceStructure;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class ServiceModel
    {
        public Guid? ID { get; set; }

        public String Url { get; set; }

        public String Name { get; set; }

        public ServiceEntity Entity { get; set; }
    }
}