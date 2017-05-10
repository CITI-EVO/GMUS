using CITI.EVO.Tools.Extensions;
using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class ComparableGroup<TKey, TValue> : Dictionary<TKey, TValue>, IComparableGroup<TKey, TValue>
    {
        private readonly IComparer<TValue> _valueComparer;

        public ComparableGroup()
            : this(EqualityComparer<TKey>.Default, Comparer<TValue>.Default)
        {
        }

        public ComparableGroup(IEqualityComparer<TKey> keyComparer)
            : this(keyComparer, Comparer<TValue>.Default)
        {
        }

        public ComparableGroup(IComparer<TValue> valueComparer)
            : this(EqualityComparer<TKey>.Default, valueComparer)
        {
        }

        public ComparableGroup(IEqualityComparer<TKey> keyComparer, IComparer<TValue> valueComparer)
            : base(keyComparer)
        {
            _valueComparer = valueComparer;
        }

        public new TValue this[TKey key]
        {
            get { return this.GetValueOrDefault(key); }
            set { base[key] = value; }
        }

        public override int GetHashCode()
        {
            var hashCode = 1;

            foreach (var pair in this)
                hashCode ^= GetHashCode(pair.Key) ^ GetHashCode(pair.Value);

            return hashCode;
        }

        private int GetHashCode(Object obj)
        {
            if (obj == null)
                return 0;

            return obj.GetHashCode();
        }

        public int CompareTo(IDictionary<TKey, TValue> other)
        {
            if (ReferenceEquals(this, other))
                return 0;

            if (other == null)
                return 1;

            var @set = new HashSet<TKey>(Comparer);
            @set.UnionWith(Keys);
            @set.UnionWith(other.Keys);

            foreach (var key in @set)
            {
                var xValue = GetValue(this, key);
                var yValue = GetValue(other, key);

                var order = _valueComparer.Compare(xValue, yValue);
                if (order != 0)
                    return order;
            }

            return 0;
        }

        public override bool Equals(Object obj)
        {
            var other = obj as IDictionary<TKey, TValue>;
            return CompareTo(other) == 0;
        }

        private TValue GetValue(IDictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            if (dict.TryGetValue(key, out value))
                return value;

            return default(TValue);
        }
    }
}
