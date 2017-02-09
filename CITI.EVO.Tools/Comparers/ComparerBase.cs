using System;

namespace CITI.EVO.Tools.Comparers
{
    public abstract class ComparerBase<TObject> : IComparerBase<TObject>
    {
        public virtual int Compare(Object x, Object y)
        {
            return Compare((TObject)x, (TObject)y);
        }

        public new virtual bool Equals(Object x, Object y)
        {
            return Equals((TObject)x, (TObject)y);
        }

        public virtual int GetHashCode(Object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            if (obj is TObject)
            {
                return GetHashCode((TObject)obj);
            }

            return obj.GetHashCode();
        }

        public abstract int Compare(TObject x, TObject y);
        public abstract bool Equals(TObject x, TObject y);
        public abstract int GetHashCode(TObject obj);
    }
}
