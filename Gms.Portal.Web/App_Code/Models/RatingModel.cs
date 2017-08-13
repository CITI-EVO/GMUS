using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class RatingModel
    {
        public Guid? ID { get; set; }

        public String PrintTemplate { get; set; }
        public String MailTemplate { get; set; }

        public String SelectorExpression { get; set; }
        public String SummaryExpression { get; set; }

        public List<RateModel> Rates { get; set; }
    }
}