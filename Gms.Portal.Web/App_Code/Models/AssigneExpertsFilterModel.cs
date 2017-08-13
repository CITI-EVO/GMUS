using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class AssigneExpertsFilterModel
    {
        public Guid? GroupID { get; set; }

        public bool? AllUsers { get; set; }

        public bool? Assigneds { get; set; }
    }
}