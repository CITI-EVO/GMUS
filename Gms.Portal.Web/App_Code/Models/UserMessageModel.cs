using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class UserMessageModel
    {
        public Guid? ID { get; set; }

        public String Subject { get; set; }

        public Guid? ParentID { get; set; }

        public Guid? FormID { get; set; }
        public Guid? RecordID { get; set; }

        public Guid? FromUserID { get; set; }
        public Guid? ToUserID { get; set; }
        public Guid? StatusUserID { get; set; }

        public Guid? StatusID { get; set; }

        public bool? Readed { get; set; }

        public String Text { get; set; }

        public String Description { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateChanged { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}