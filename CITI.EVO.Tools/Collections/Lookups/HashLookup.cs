using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Collections.Lookups
{
    [Serializable]
    public class HashLookup<TKey, TElement> : ILookupEx<TKey, TElement>
    {
        private readonly IEqualityComparer<TKey> _comparer;

        private Grouping[] _groupings;
        private Grouping _lastGrouping;

        private int _capacity;
        private int _count;

        public HashLookup()
            : this(EqualityComparer<TKey>.Default)
        {
        }
        public HashLookup(int capacity)
            : this(EqualityComparer<TKey>.Default, capacity)
        {
        }
        public HashLookup(IEqualityComparer<TKey> comparer)
            : this(comparer, 7)
        {
        }
        public HashLookup(IEqualityComparer<TKey> comparer, int capacity)
        {
            if (comparer == null)
                comparer = EqualityComparer<TKey>.Default;

            _capacity = capacity;
            _comparer = comparer;
            _groupings = new Grouping[capacity];
        }

        public int Count
        {
            get { return _count; }
        }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                var grouping = GetGrouping(key, false);
                if (grouping != null)
                    return grouping;

                return Enumerable.Empty<TElement>();
            }
        }

        public void Add(TKey key, TElement element)
        {
            var group = GetGrouping(key, true);
            group.Add(element);
        }

        public bool Remove(TKey key)
        {
            var hashCode = GetHashCode(key);
            var index = hashCode % _groupings.Length;

            var parentGroup = (Grouping)null;
            var group = _groupings[index];

            while (group != null)
            {
                if (group.HashCode == hashCode && _comparer.Equals(group.Key, key))
                {
                    if (parentGroup == null)
                        _groupings[index] = null;
                    else
                    {
                        parentGroup.NextGroup = group.NextGroup;
                        parentGroup.HashNext = group.HashNext;
                    }

                    _count--;

                    return true;
                }

                parentGroup = group;
                group = group.HashNext;
            }

            return false;
        }
        public bool Remove(TKey key, TElement element)
        {
            var group = GetGrouping(key, false);
            if (group == null)
                return false;

            return group.Remove(element);
        }

        public bool Contains(TKey key)
        {
            return GetGrouping(key, false) != null;
        }
        public bool Contains(TKey key, TElement element)
        {
            var group = GetGrouping(key, false);
            if (group == null)
                return false;

            return group.Contains(element);
        }

        public void Clear()
        {
            _groupings = new Grouping[_capacity];
            _count = 0;
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            var group = _lastGrouping;
            if (group != null)
            {
                do
                {
                    group = group.NextGroup;
                    yield return group;
                } while (group != _lastGrouping);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Grouping GetGrouping(TKey key, bool create)
        {
            var hashCode = GetHashCode(key);

            var groupIndex = hashCode % _groupings.Length;
            var grouping = _groupings[groupIndex];

            while (grouping != null)
            {
                if (grouping.HashCode == hashCode && _comparer.Equals(grouping.Key, key))
                    return grouping;

                grouping = grouping.HashNext;
            }

            if (!create)
            {
                return null;
            }

            if (_groupings.Length == _count)
            {
                IncreaseSize();
                groupIndex = hashCode % _groupings.Length;
            }

            var newGrouping = new Grouping
            {
                Key = key,
                HashCode = hashCode,
                HashNext = _groupings[groupIndex]
            };

            _groupings[groupIndex] = newGrouping;

            if (_lastGrouping == null)
            {
                newGrouping.NextGroup = newGrouping;
            }
            else
            {
                newGrouping.NextGroup = _lastGrouping.NextGroup;
                _lastGrouping.NextGroup = newGrouping;
            }

            _lastGrouping = newGrouping;

            _count++;

            return newGrouping;
        }

        private int GetHashCode(TKey key)
        {
            if (key == null)
                return 0;

            return _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        private void IncreaseSize()
        {
            var newSize = checked(_groupings.Length * 2 + 1);

            var newGroupings = new Grouping[newSize];
            var group = _lastGrouping;

            do
            {
                group = group.NextGroup;

                var index = group.HashCode % newSize;
                group.HashNext = newGroupings[index];

                newGroupings[index] = group;
            } while (group != _lastGrouping);

            _groupings = newGroupings;
        }

        [Serializable]
        private class Grouping : List<TElement>, IGrouping<TKey, TElement>
        {
            public int HashCode;

            public Grouping HashNext;
            public Grouping NextGroup;

            private TKey _key;
            public TKey Key
            {
                get { return _key; }
                set { _key = value; }
            }
        }
    }
}
