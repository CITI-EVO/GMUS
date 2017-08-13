using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class RateModel
    {
        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public String Number { get; set; }

        public String Name { get; set; }

        public int? MinScore { get; set; }

        public int? MaxScore { get; set; }
    }
}