using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Enums
{
    [Flags, DataContract]
    public enum RulePermissionsEnum
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        View = 8,

        [EnumMember]
        Add = 4,

        [EnumMember]
        Edit = 2,

        [EnumMember]
        Delete = 1,

        [EnumMember]
        ViewAdd = 12,

        [EnumMember]
        ViewEdit = 10,

        [EnumMember]
        ViewDelete = 9,

        [EnumMember]
        AddEdit = 6,

        [EnumMember]
        AddDelete = 5,

        [EnumMember]
        EditDelete = 3,

        [EnumMember]
        ViewAddEdit = 14,

        [EnumMember]
        AddEditDelete = 7,

        [EnumMember]
        ViewEditDelete = 11,

        [EnumMember]
        ViewAddDelete = 13,

        [EnumMember]
        ViewAddEditDelete = 15,
    }
}