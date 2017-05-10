using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Models
{
    public class MailContactModel
    {
        public Guid? RecipientGroupID { get; set; }

        public List<Guid> RecipientsID { get; set; }

        public String TemplateName { get; set; }

        public Guid? TemplateID { get; set; }

        public String ContactType { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }

        public Guid? FormID { get; set; }
    }
}