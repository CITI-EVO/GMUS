using System;

namespace Gms.Portal.Web.Models
{
    public class FormElementModel
    {
        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public String Name { get; set; }

        public String Language { get; set; }

        public String ParentType { get; set; }

        public String ElementType { get; set; }

        public int? OrderIndex { get; set; }

        public String ControlType { get; set; }

        public String Mask { get; set; }

        public bool Visible { get; set; }

        public bool Enabled { get; set; }

        public String ValidationExp { get; set; }

        public String Tag { get; set; }
        public String Number { get; set; }

        //public List<NameValueEntity> PossibleValues { get; set; }
    }
}