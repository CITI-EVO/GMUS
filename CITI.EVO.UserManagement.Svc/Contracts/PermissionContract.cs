using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CITI.EVO.UserManagement.Svc.Enums;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class PermissionContract
    {
        public PermissionContract()
        {
        }

        [DataMember]
        public Guid? ProjectID { get; set; }

        [DataMember]
        public Guid ResourceID { get; set; }

        [DataMember]
        public String ResourcePath { get; set; }

        [DataMember]
        public RulePermissionsEnum RuleValue { get; set; }

        [DataMember]
        public Dictionary<String, String> PermissionParameter { get; set; }
    }
}