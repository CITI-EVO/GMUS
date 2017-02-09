using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class ProjectContract
    {
        public ProjectContract()
        {
        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}