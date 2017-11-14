using System;
using System.Collections.Generic;
using System.Web;
using Gms.Portal.Web.Entities.Others;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class MonitoringProjectFilesModel
    {
        public Guid? ParentID { get; set; }

        public String Description { get; set; }

        public String FileUrl { get; set; }

        public bool? Flaw { get; set; }
        public bool? Submited { get; set; }

        public IEnumerable<FileEntity> Files { get; set; }

        public IList<HttpPostedFile> PostedFiles { get; set; }
    }
}