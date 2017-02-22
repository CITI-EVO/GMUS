using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.LogicStructure
{
    [Serializable]
    [XmlRoot("Expressions")]
    public class LogicExpressionsEntity : LogicContainerEntity
    {
        [XmlElement("Select")]
        public List<NamedExpressionEntity> Select { get; set; }

        [XmlElement("FilterBy")]
        public List<ExpressionEntity> FilterBy { get; set; }

        [XmlElement("OrderBy")]
        public List<ExpressionEntity> OrderBy { get; set; }

        [XmlElement("GroupBy")]
        public List<ExpressionEntity> GroupBy { get; set; }
    }
}