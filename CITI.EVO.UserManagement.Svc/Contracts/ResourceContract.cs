using System;
using System.Runtime.Serialization;
using CITI.EVO.UserManagement.Svc.Enums;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class ResourceContract
    {
        public ResourceContract()
        {
          
        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public Guid? ParentID { get; set; }

        [DataMember]
        public Guid? ProjectID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public RuleTypesEnum Type { get; set; }

        [DataMember]
        public string Value{ get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}