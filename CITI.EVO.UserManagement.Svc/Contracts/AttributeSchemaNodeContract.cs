using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class AttributeSchemaNodeContract
    {
        public AttributeSchemaNodeContract()
        {
        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public Guid AttributeCategoryID { get; set; }

        [DataMember]
        public Guid AttributeTypeID { get; set; }

        [DataMember]
        public Guid AttributeSchemaID { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}