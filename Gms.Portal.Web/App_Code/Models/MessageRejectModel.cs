using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class MessageRejectModel
    {
        public Guid? MessageID { get; set; }

        public String Description { get; set; }
    }
}