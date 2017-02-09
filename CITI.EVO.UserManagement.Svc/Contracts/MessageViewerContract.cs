using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class MessageViewerContract
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public Guid MessageID { get; set; }

        [DataMember]
        public Guid UserID { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}
