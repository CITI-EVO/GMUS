using System;

namespace Gms.Portal.Web.Models.Common
{
    [Serializable]
    public class PersonInfoModel
    {
        public String PersonalId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public String Gender { get; set; }
    }
}