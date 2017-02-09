using System;
using System.Reflection;

namespace CITI.EVO.Tools.Comparers
{
    public class AssemblyNameComparer : ComparerBase<AssemblyName>
    {
        private static readonly StringComparer nameComparer = StringComparer.Ordinal; 

        public override int Compare(AssemblyName x, AssemblyName y)
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

            var order = nameComparer.Compare(x.FullName, y.FullName);
            return order;
        }

        public override bool Equals(AssemblyName x, AssemblyName y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public override int GetHashCode(AssemblyName obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return nameComparer.GetHashCode(obj.FullName);
        }
    }
}
