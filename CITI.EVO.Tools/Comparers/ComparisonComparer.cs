using System;

namespace CITI.EVO.Tools.Comparers
{
    public class ComparisonComparer<TObject> : ComparerBase<TObject>
    {
        private readonly Comparison<TObject> comparison;

        public ComparisonComparer(Comparison<TObject> comparison)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException("comparison");
            }

            this.comparison = comparison;
        }

        public override int Compare(TObject x, TObject y)
        {
            return comparison(x, y);
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
