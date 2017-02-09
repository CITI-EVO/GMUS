using System;
using System.Runtime.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class LanguageContract
    {
        public LanguageContract()
        {
        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public String DisplayName { get; set; }

        [DataMember]
        public String EngName { get; set; }

        [DataMember]
        public String NativeName { get; set; }

        [DataMember]
        public String Pair { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}