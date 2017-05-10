using System;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class ParentChildEntity<TItem>
    {
        public ParentChildEntity()
        {
        }

        public ParentChildEntity(TItem parent, TItem child)
        {
            Parent = parent;
            Child = child;
        }

        public TItem Parent { get; set; }

        public TItem Child { get; set; }
    }
}