using System;

namespace Gms.Portal.Web.Models
{
    public class RecipientGroupModel
    {
        public Guid? ID { get; set; }

        public String Type { get; set; }

        public String Name { get; set; }

        public Guid? FormID { get; set; }

        public String Expression { get; set; }

        public String Description { get; set; }
    }
}