using System;

namespace Gms.Portal.Web.Entities.Monitoring
{
    [Serializable]
    public class ProjectsDataEntity
    {
        public String Task { get; set; }
        public String Status { get; set; }
        public String Flaws { get; set; }
        public double? FlawsScore { get; set; }
        public String FlawDescription { get; set; }
        public String DoneStatus { get; set; }
        public String DoneDescription { get; set; }
        public String CreatedBy { get; set; }

        public DateTime? SubmitDate { get; set; }
        public String SubmitUser { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public DateTime DateCreated { get; set; }
    }
}