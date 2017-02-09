using System;
using System.Runtime.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class AreaTypeContract
    {
        public AreaTypeContract()
        {
           
        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public String GeoName { get; set; }

        [DataMember]
        public String EngName { get; set; }

        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public int Level { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}