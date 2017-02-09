using System.Xml.Linq;

namespace CITI.EVO.Tools.Comparers
{
    public class XNameComparer : XComparerBase<XName>
    {
        public override int Compare(XName x, XName y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x == null && y != null)
            {
                return -1;
            }

            if (x != null && y == null)
            {
                return 1;
            }

            var order = nameComparer.Compare(x.NamespaceName, y.NamespaceName);
            if (order == 0)
            {
                order = nameComparer.Compare(x.LocalName, y.LocalName);
            }

            return order;
        }

        public override bool Equals(XName x, XName y)
        {
            return Compare(x, y) == 0;
        }

        public override int GetHashCode(XName obj)
        {
            if (obj == null)
            {
                return 0;
            }

            var nsHashCode = nameComparer.GetHashCode(obj.NamespaceName);
            var nameHashCode = nameComparer.GetHashCode(obj.LocalName);

            return nsHashCode ^ nameHashCode;
        }
    }
}
