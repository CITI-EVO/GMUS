using System;

namespace Gms.Portal.Web.Entities.Helpers
{
    public class FormTreeNodeEntity : TreeNodeEntity
    {
        public Guid? FormID { get; set; }

        public String Type { get; set; }

        public String Number { get; set; }

        public int? OrderIndex { get; set; }

        public String Language { get; set; }
    }
}