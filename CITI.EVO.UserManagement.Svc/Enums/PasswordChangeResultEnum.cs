using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Enums
{
    [Flags, DataContract]
    public enum PasswordChangeResultEnum
    {
        [EnumMember]
        Success = 0,

        [EnumMember]
        TokenNotFound = 1,

        [EnumMember]
        UserNotFound = 2,

        [EnumMember]
        PasswordMismatch = 3,

        [EnumMember]
        InvalidPattern = 4,

        [EnumMember]
        NewAndOldPasswordMatch = 5,
    }
}