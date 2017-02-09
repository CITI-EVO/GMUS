using System;
using System.Runtime.Serialization;

namespace CITI.EVO.CommonData.Svc.Enums
{
    [Flags, DataContract]
    public enum AreaTypesEnum
    {
        [EnumMember]
        Region = 0,

        [EnumMember]
        Municipality = 1,

        [EnumMember]
        MunicipalCenter = 2, //ქალაქი

        [EnumMember]
        Village = 3,

        [EnumMember]
        Country = 4,

        [EnumMember]
        Town = 5, //დაბა 
    }
}