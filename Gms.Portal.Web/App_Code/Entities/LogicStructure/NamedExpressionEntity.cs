using System;
using System.Xml.Serialization;
using Gms.Portal.Web.Models.Common;

namespace Gms.Portal.Web.Entities.LogicStructure
{
    [Serializable]
    [XmlRoot("NamedExpression")]
    public class NamedExpressionEntity : ExpressionEntity
    {
        [XmlElement("Name")]
        public String Name { get; set; }
    }
}