using System;

namespace Gms.Portal.Web.Models
{
    public class NotificationHistoryModel
    {
        public Guid ID { get; set; }

        public Guid? RecipientID { get; set; }

        public Guid? UserID { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public String Phone { get; set; }

        public String ContactType { get; set; }

        public String Subject { get; set; }

        public String Body { get; set; }

        public DateTime? DateCreated { get; set; }

    }
}