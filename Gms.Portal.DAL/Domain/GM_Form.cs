using System;
using System.Xml;
using System.Xml.Linq;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    public class GM_Form : IDbEntity
    {
        public virtual Guid ID { get; set; }

        public virtual String Name { get; set; }

        public virtual String Number { get; set; }

        public virtual String Abbreviation { get; set; }

        public virtual String Year { get; set; }

        public virtual int? OrderIndex { get; set; }

        public virtual bool? Visible { get; set; }

        public virtual bool? RequiresApprove { get; set; }

        public virtual int? ApprovalDeadline { get; set; }

        public virtual String VisibleExpression { get; set; }

        public virtual String FillingValidationExpression { get; set; }

        public virtual String FillingValidationMessage { get; set; }

        public virtual String UserMode { get; set; }

        public virtual String FormType { get; set; }

        public virtual Guid? CategoryID { get; set; }

        public virtual XDocument XmlData { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}
