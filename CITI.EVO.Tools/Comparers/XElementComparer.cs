using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CITI.EVO.Tools.Comparers
{
    public class XElementComparer : XComparerBase<XElement>
    {
        public override int Compare(XElement x, XElement y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            var elemetsOrder = CompareElements(x, y);
            if (elemetsOrder != 0)
            {
                return elemetsOrder;
            }

            var xChildren = x.Elements().OrderBy(n => n.Name, xNameComparer).ToList();
            var yChildren = y.Elements().OrderBy(n => n.Name, xNameComparer).ToList();

            if (xChildren.Count != yChildren.Count)
            {
                return xChildren.Count.CompareTo(yChildren.Count);
            }

            var xChildrenEnumerator = xChildren.GetEnumerator();
            var yChildrenEnumerator = yChildren.GetEnumerator();

            while (xChildrenEnumerator.MoveNext() && yChildrenEnumerator.MoveNext())
            {
                var xChild = xChildrenEnumerator.Current;
                var yChild = yChildrenEnumerator.Current;

                var childElementsOrder = Compare(xChild, yChild);
                if (childElementsOrder != 0)
                {
                    return childElementsOrder;
                }
            }

            return 0;
        }

        public override bool Equals(XElement x, XElement y)
        {
            return Compare(x, y) == 0;
        }

        public override int GetHashCode(XElement obj)
        {
            if (obj == null)
            {
                return 0;
            }

            var hashCode = xNameComparer.GetHashCode(obj.Name);

            var text = GetElementText(obj);

            var textHash = text.GetHashCode();
            hashCode ^= textHash;

            foreach (var item in obj.Attributes())
            {
                var attrHash = xAttributeComparer.GetHashCode(item);
                hashCode ^= attrHash;
            }

            foreach (var item in obj.Elements())
            {
                var childHash = GetHashCode(item);
                hashCode ^= childHash;
            }

            return hashCode;
        }

        private int CompareElements(XElement x, XElement y)
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

            var elementNameOrder = xNameComparer.Compare(x.Name, y.Name);
            if (elementNameOrder != 0)
            {
                return elementNameOrder;
            }

            var xValue = GetElementText(x);
            var yValue = GetElementText(y);

            var elementValueOrder = valueComparer.Compare(xValue, yValue);
            if (elementValueOrder != 0)
            {
                return elementValueOrder;
            }

            var attributesOrder = CompareAttributes(x, y);
            if (attributesOrder != 0)
            {
                return attributesOrder;
            }

            return 0;
        }

        private int CompareAttributes(XElement x, XElement y)
        {
            var xAttributesList = x.Attributes().ToList();
            xAttributesList.Sort(xAttributeComparer);

            var yAttributesList = y.Attributes().ToList();
            yAttributesList.Sort(xAttributeComparer);

            if (xAttributesList.Count != yAttributesList.Count)
            {
                return xAttributesList.Count.CompareTo(yAttributesList.Count);
            }

            var xAttributeEnumerator = xAttributesList.GetEnumerator();
            var yAttributeEnumerator = yAttributesList.GetEnumerator();

            while (xAttributeEnumerator.MoveNext() && yAttributeEnumerator.MoveNext())
            {
                var xAttribute = xAttributeEnumerator.Current;
                var yAttribute = yAttributeEnumerator.Current;

                var attributesOrder = xAttributeComparer.Compare(xAttribute, yAttribute);
                if (attributesOrder != 0)
                {
                    return attributesOrder;
                }
            }

            return 0;
        }

        private String GetElementText(XElement xElement)
        {
            var sb = new StringBuilder();

            foreach (var item in xElement.Nodes())
            {
                if (item.NodeType == XmlNodeType.Text)
                {
                    var xText = (XText)item;
                    sb.Append(xText.Value);
                }
            }

            return sb.ToString();
        }
    }
}
