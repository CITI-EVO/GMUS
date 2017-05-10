using System;

namespace Gms.Portal.Web.Models
{
    public class PhaseModel
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public String StartDate { get; set; }

        public String StartTime { get; set; }

        public String EndDate { get; set; }

        public String EndTime { get; set; }

        public String Color { get; set; }

        public Guid? FormID { get; set; }
    }
}