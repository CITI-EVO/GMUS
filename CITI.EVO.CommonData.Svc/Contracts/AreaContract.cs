using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CITI.EVO.CommonData.Svc.Contracts;
using CITI.EVO.CommonData.Svc.Enums;

namespace CITI.EVO.CommonData.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class AreaContract
    {
        public AreaContract()
        {

        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public Decimal? OLD_ID { get; set; }

        [DataMember]
        public Guid? ParentID { get; set; }

        [DataMember]
        public AreaContract Parent { get; set; }

        [DataMember]
        public String Code { get; set; }

        [DataMember]
        public String CraCode { get; set; }

        [DataMember]
        public String GeoName { get; set; }

        [DataMember]
        public String EngName { get; set; }

        [DataMember]
        public String NewCode { get; set; }

        [DataMember]
        public Guid TypeID { get; set; }

        [DataMember]
        public AreaTypeContract AreaType { get; set; }

        [DataMember]
        public List<PhoneIndexContract> PhoneIndexes { get; set; }

        [DataMember]
        public RecordTypesEnum RecordType { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}