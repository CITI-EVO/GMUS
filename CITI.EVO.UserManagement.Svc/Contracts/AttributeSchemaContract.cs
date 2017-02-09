using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class AttributeSchemaContract
    {
        public AttributeSchemaContract()
        {
        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public Guid? ProjectID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}