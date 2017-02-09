using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Comparers
{
    public class KeySelectorComparer<TObject, TKey> : ComparerBase<TObject>
    {
        public KeySelectorComparer(Func<TObject, TKey> keySelector)
            : this(null, keySelector)
        {
        }

        public KeySelectorComparer(IComparer<TKey> keyComparer, Func<TObject, TKey> keySelector)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            KeySelector = keySelector;
            KeyComparer = (keyComparer ?? Comparer<TKey>.Default);
        }

        public IComparer<TKey> KeyComparer { get; private set; }
        public Func<TObject, TKey> KeySelector { get; private set; }

        public override int Compare(TObject x, TObject y)
        {
            var xValue = KeySelector(x);
            var yValue = KeySelector(y);

            return KeyComparer.Compare(xValue, yValue);
        }

        public override bool Equals(TObject x, TObject y)
        {
            var xHashCode = GetHashCode(x);
            var yHashCode = GetHashCode(y);

            return xHashCode == yHashCode;
        }

        public override int GetHashCode(TObject obj)
        {
            var value = KeySelector(obj);

            return value.GetHashCode();
        }
    }
}
