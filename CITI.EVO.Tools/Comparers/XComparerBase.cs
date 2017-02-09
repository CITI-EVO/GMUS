using System;
using System.Xml.Linq;

namespace CITI.EVO.Tools.Comparers
{
    public abstract class XComparerBase<TObject> : ComparerBase<TObject>
    {
        protected static readonly ComparerBase<XName> xNameComparer = new XNameComparer();
        protected static readonly ComparerBase<XAttribute> xAttributeComparer = new XAttributeComparer();

        protected static readonly StringComparer valueComparer = StringComparer.Ordinal;
        protected static readonly StringComparer nameComparer = StringComparer.OrdinalIgnoreCase;
    }
}
