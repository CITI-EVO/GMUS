using System;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class FormModel
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public String Number { get; set; }

        public String Abbreviation { get; set; }

        public String Year { get; set; }

        public Guid? CategoryID { get; set; }

        public int? OrderIndex { get; set; }

        public bool? Visible { get; set; }

        public bool? RequiresApprove { get; set; }

        public int? ApprovalDeadline { get; set; }

        public String VisibleExpression { get; set; }

        public String FillingValidationExpression { get; set; }

        public String FillingValidationMessage { get; set; }

        public String UserMode { get; set; }

        public String FormType { get; set; }

        public FormEntity Entity { get; set; }
    }
}