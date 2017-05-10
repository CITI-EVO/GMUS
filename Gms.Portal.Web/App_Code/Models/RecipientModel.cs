using System;

namespace Gms.Portal.Web.Models
{
    public class RecipientModel
    {
        public Guid ID { get; set; }

        public Guid GroupID { get; set; }

        public Guid? UserID { get; set; }

        public String UserName { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public String Phone { get; set; }

    }
}