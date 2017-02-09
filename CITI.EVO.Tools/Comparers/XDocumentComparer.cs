using System.Xml.Linq;

namespace CITI.EVO.Tools.Comparers
{
    public class XDocumentComparer : XComparerBase<XDocument>
    {
        protected static readonly XComparerBase<XElement> xElementComparer = new XElementComparer();

        public override int Compare(XDocument x, XDocument y)
        {
            return xElementComparer.Compare(x.Root, y.Root);
        }

        public override bool Equals(XDocument x, XDocument y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public override int GetHashCode(XDocument obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return xElementComparer.GetHashCode(obj.Root);
        }
    }
}
