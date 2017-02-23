using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class ElementModel
    {
        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public String Name { get; set; }

        public String ParentType { get; set; }

        public String ElementType { get; set; }

        public int? OrderIndex { get; set; }

        public bool Privacy { get; set; }

        public String ControlType { get; set; }

        public int? GroupSize { get; set; }

        public String Mask { get; set; }

        public bool Visible { get; set; }

        public bool Enabled { get; set; }

        public bool Mandatory { get; set; }

        public String Description { get; set; }

        public bool DisplayOnGrid { get; set; }

        public String ValidationExp { get; set; }

        public String ErrorMessage { get; set; }

        public String Tag { get; set; }

        public Guid? DataSourceID { get; set; }

        public String TextExpression { get; set; }

        public String ValueExpression { get; set; }
    }
}