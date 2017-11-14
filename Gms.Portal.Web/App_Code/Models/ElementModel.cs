using System;
using System.Collections.Generic;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class ElementModel
    {
        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public String Name { get; set; }

        public String Alias { get; set; }

        public bool? Unique { get; set; }

        public String ParentType { get; set; }

        public String ElementType { get; set; }

        public int? OrderIndex { get; set; }

        public bool? Privacy { get; set; }

        public String ControlType { get; set; }

        public int? GroupSize { get; set; }

        public String GroupBgColor { get; set; }

        public String GroupTextColor { get; set; }

        public String TextColor { get; set; }

        public String Mask { get; set; }

        public bool? Visible { get; set; }

        public bool? ReadOnly { get; set; }

        public bool? Mandatory { get; set; }

        public bool? Inversion { get; set; }

        public int? TotalSize { get; set; }

        public int? CaptionSize { get; set; }

        public int? ControlSize { get; set; }

        public int? TreeMaxLevel { get; set; }

        public String GridFieldSummary { get; set; }

        public String Description { get; set; }

        public String DisplayOnGrid { get; set; }

        public bool? ResetDataOnHide { get; set; }

        public bool? DisplayOnFilter { get; set; }

        public String ValidationExp { get; set; }

        public String ErrorMessage { get; set; }

        public String DataSourceID { get; set; }

        public String DataSourceFilterExp { get; set; }

        public String DataSourceSortExp { get; set; }

        public String TextExpression { get; set; }

        public String ValueExpression { get; set; }

        public Guid? DependentFieldID { get; set; }

        public bool? FilterByUser { get; set; }

        public bool? FirstTimeFill { get; set; }

        public bool? AllowBulkFill { get; set; }

        public bool? RequiresApproval { get; set; }

        public bool? NotPrintable { get; set; }

        public String DependentExp { get; set; }

        public String DependentFillExp { get; set; }

        public String Tag { get; set; }

        public String FieldValueExpression { get; set; }

        public String VisibleExpression { get; set; }

        public List<ParameterEntity> Parameters { get; set; }
    }
}