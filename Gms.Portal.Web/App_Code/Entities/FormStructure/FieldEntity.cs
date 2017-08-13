using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("Field")]
    public class FieldEntity : ControlEntity
    {
        [XmlElement("Type")]
        public String Type { get; set; }

        [XmlElement("Mask")]
        public String Mask { get; set; }

        [XmlElement("Tag")]
        public String Tag { get; set; }

        [XmlElement("Unique")]
        public bool? Unique { get; set; }

        [XmlElement("ReadOnly")]
        public bool? ReadOnly { get; set; }

        [XmlElement("Privacy")]
        public bool? Privacy { get; set; }

        [XmlElement("Inversion")]
        public bool? Inversion { get; set; }

        [XmlElement("Mandatory")]
        public bool? Mandatory { get; set; }

        [XmlElement("Description")]
        public String Description { get; set; }

        [XmlElement("CaptionSize")]
        public int? CaptionSize { get; set; }

        [XmlElement("ControlSize")]
        public int? ControlSize { get; set; }

        [XmlElement("DisplayOnGrid")]
        public String DisplayOnGrid { get; set; }

        [XmlElement("FilterByUser")]
        public bool? FilterByUser { get; set; }

        [XmlElement("RequiresApproval")]
        public bool? RequiresApproval { get; set; }

        [XmlElement("DisplayOnFilter")]
        public bool? DisplayOnFilter { get; set; }

        [XmlElement("ResetDataOnHide")]
        public bool? ResetDataOnHide { get; set; }

        [XmlElement("ValidationExp")]
        public String ValidationExp { get; set; }

        [XmlElement("DependentFillExp")]
        public String DependentFillExp { get; set; }

        [XmlElement("DataSourceFilterExp")]
        public String DataSourceFilterExp { get; set; }

        [XmlElement("DataSourceSortExp")]
        public String DataSourceSortExp { get; set; }

        [XmlElement("ErrorMessage")]
        public String ErrorMessage { get; set; }

        [XmlElement("DataSourceID")]
        public String DataSourceID { get; set; }

        [XmlElement("TextExpression")]
        public String TextExpression { get; set; }

        [XmlElement("ValueExpression")]
        public String ValueExpression { get; set; }

        [XmlElement("GridFieldSummary")]
        public String GridFieldSummary { get; set; }

        [XmlElement("FieldValueExpression")]
        public String FieldValueExpression { get; set; }

        [XmlElement("Parameters")]
        public List<ParameterEntity> Parameters { get; set; }
    }
}