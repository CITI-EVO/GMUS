using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class MessageContract
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public String Text { get; set; }

        [DataMember]
        public Guid ObjectID { get; set; }

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }


}
