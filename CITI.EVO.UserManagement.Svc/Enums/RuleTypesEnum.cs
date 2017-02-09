using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Enums
{
    [Flags, DataContract]
    public enum RuleTypesEnum
    {
        [EnumMember]
        Other = 0,

        [EnumMember]
        Entire = 10,

        [EnumMember]
        Class = 20,

        [EnumMember]
        Method = 30,

        [EnumMember]
        Property = 40,

        [EnumMember]
        Page = 50,

        [EnumMember]
        Control = 60,

        [EnumMember]
        Form = 70,

        [EnumMember]
        FormField = 80,

        [EnumMember]
        Grid = 90,

        [EnumMember]
        GridField = 100,

        [EnumMember]
        Table = 110,

        [EnumMember]
        TableColumn = 120,
    }

    
}