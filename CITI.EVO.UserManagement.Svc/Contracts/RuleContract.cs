using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class RuleContract
    {
        public RuleContract()
        {
        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public Guid ProjectID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public bool CanView { get; set; }

        [DataMember]
        public bool CanAdd { get; set; }

        [DataMember]
        public bool CanEdit { get; set; }

        [DataMember]
        public bool CanDelete { get; set; }

        [DataMember]
        public int? AccessLevel { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}