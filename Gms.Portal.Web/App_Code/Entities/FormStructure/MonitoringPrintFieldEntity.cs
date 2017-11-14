using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [XmlRoot("MonitoringPrintField")]
    [Serializable]
    public class MonitoringPrintFieldEntity
    {
        [XmlElement("ID")]
        public Guid? ID { get; set; }


        [XmlElement("FormID")]
        public Guid? FormID { get; set; }


        [XmlElement("PrintType")]
        public String PrintType { get; set; }


        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Template")]
        public String Template { get; set; }

        [XmlElement("BudgetForm")]
        public Boolean? BudgetForm { get; set; }


        [XmlElement("SummaryBudgetForm")]
        public Boolean? SummaryBudgetForm { get; set; }


        [XmlElement("ProjectsForm")]
        public Boolean? ProjectsForm { get; set; }

        [XmlElement("IsLendscape")]
        public bool? IsLendscape { get; set; }
    }
}