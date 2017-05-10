using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class CloneFormGridModel
    {
        public CloneFormGridModel()
        {
        }

        public String Mode { get; set; }

        public Guid? SourceGridID { get; set; }

        public Guid? TargetGridID { get; set; }
    }
}