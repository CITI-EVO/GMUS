using System;
using System.Runtime.Serialization;

namespace CITI.EVO.CommonData.Svc.Enums
{
    [Flags, DataContract]
    public enum RecordTypesEnum
    {
        [EnumMember]
        All = 0,

        [EnumMember]
        Active = 1,

        [EnumMember]
        Inactive = 2
    }
}
