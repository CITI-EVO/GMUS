using System;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class ElementTreeNodeEntity : TreeNodeEntity
    {
        public String Alias { get; set; }

        public String ElementType { get; set; }

        public String ControlType { get; set; }

        public int? OrderIndex { get; set; }

        public bool Visible { get; set; }
    }
}