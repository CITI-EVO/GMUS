using System;
using System.Runtime.Serialization;

namespace CITI.EVO.UserManagement.Svc.Contracts
{
    [Serializable]
    [DataContract]
    public class UserContract
    {
        public UserContract()
        {
        }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public String LoginName { get; set; }

        [DataMember]
        public String Password { get; set; }

        [DataMember]
        public String FirstName { get; set; }

        [DataMember]
        public String LastName { get; set; }

        [DataMember]
        public String Email { get; set; }

        [DataMember]
        public String Address { get; set; }

        [DataMember]
        public bool IsSuperAdmin { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public Guid? UserCategoryID { get; set; }

        [DataMember]
        public DateTime? PasswordExpirationDate { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateChanged { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}