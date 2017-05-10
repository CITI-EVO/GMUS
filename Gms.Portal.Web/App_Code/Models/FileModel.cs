using System;

namespace Gms.Portal.Web.Models
{
    public class FileModel
    {
        public Guid ID { get; set; }

        public Guid? ParentID { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public String FileName { get; set; }

        public byte[] FileData { get; set; }

    }
}