using System;

namespace CITI.EVO.Tools.Comparers
{
    /// <summary>
    /// Default <see cref="IComparable"/> object comparer.
    /// </summary>
    public class ComparableComparer<TObject> : ComparerBase<TObject>
    {
        public override int Compare(TObject x, TObject y)
        {
            if (x is IComparable<TObject>)
            {
                var comparable = x as IComparable<TObject>;
                return comparable.CompareTo(y);
            }

            if (x is IComparable)
            {
                var comparable = x as IComparable;
                return comparable.CompareTo(y);
            }

            throw new Exception("Object is not comparable");
        }

        public override bool Equals(TObject x, TObject y)
        {
            return Compare(x, y) == 0;
        }

        public override int GetHashCode(TObject obj)
        {
            return obj.GetHashCode();
        }
    }
}
