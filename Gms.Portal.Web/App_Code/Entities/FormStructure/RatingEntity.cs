using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [XmlRoot("Rating")]
    [Serializable]
    public class RatingEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }

        [XmlElement("PrintTemplate")]
        public String PrintTemplate { get; set; }

        [XmlElement("MailTemplate")]
        public String MailTemplate { get; set; }

        [XmlElement("SummaryExpression")]
        public String SummaryExpression { get; set; }

        [XmlElement("SelectorExpression")]
        public String SelectorExpression { get; set; }

        [XmlElement("Rate")]
        public List<RateEntity> Rates { get; set; }
    }
}