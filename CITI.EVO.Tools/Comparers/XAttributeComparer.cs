using System.Xml.Linq;

namespace CITI.EVO.Tools.Comparers
{
    public class XAttributeComparer : XComparerBase<XAttribute>
    {
        public override int Compare(XAttribute x, XAttribute y)
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

            var attributeNameOrder = xNameComparer.Compare(x.Name, y.Name);
            if (attributeNameOrder != 0)
            {
                return attributeNameOrder;
            }

            var attributeValueOrder = valueComparer.Compare(x.Value, y.Value);
            if (attributeValueOrder != 0)
            {
                return attributeValueOrder;
            }

            return 0;
        }

        public override bool Equals(XAttribute x, XAttribute y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public override int GetHashCode(XAttribute obj)
        {
            if (obj == null)
            {
                return 0;
            }

            var nameHash = xNameComparer.GetHashCode(obj.Name);
            var valueHash = valueComparer.GetHashCode(obj.Value);

            return nameHash ^ valueHash;
        }
    }
}
