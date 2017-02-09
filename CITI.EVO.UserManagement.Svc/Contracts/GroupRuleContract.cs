using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class GroupRuleContract
    {
        public GroupRuleContract()
        {
        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public Guid GroupID { get; set; }

        [DataMember]
        public Guid RuleID { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}