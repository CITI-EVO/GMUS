using System;
using System.Runtime.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class MobileIndexesContract
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public int? Value { get; set; }

        [DataMember]
        public String GeoOperatorName { get; set; }

        [DataMember]
        public String EngOperatorName { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}