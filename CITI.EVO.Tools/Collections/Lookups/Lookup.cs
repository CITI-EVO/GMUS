using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Collections.Lookups
{
    [Serializable]
    internal class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        private readonly IEqualityComparer<TKey> _comparer;

        private Grouping[] _groupings;
        private Grouping _lastGroup;

        private int _count;

        public Lookup() : this(EqualityComparer<TKey>.Default)
        {
        }

        public Lookup(IEqualityComparer<TKey> comparer)
        {
            _comparer = comparer;
            _groupings = new Grouping[7];
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

        public void Add(TKey key, TElement item)
        {
            var group = GetGrouping(key, true);
            group.Add(item);
        }

        public bool Remove(TKey key)
        {
            var hashCode = GetHashCode(key);
            var index = hashCode % _groupings.Length;

            var parent = (Grouping)null;
            var group = _groupings[index];

            while (group != null)
            {
                if (group.HashCode == hashCode && _comparer.Equals(group.Key, key))
                {
                    if (parent == null)
                        _groupings[index] = null;
                    else
                    {
                        parent.NextGroup = group.NextGroup;
                        parent.HashNext = group.HashNext;
                    }

                    _count--;

                    return true;
                }

                parent = group;
                group = group.HashNext;
            }

            return false;
        }

        public bool Remove(TKey key, TElement item)
        {
            var group = GetGrouping(key, false);
            if (group == null)
                return false;

            return group.Remove(item);
        }

        public bool Contains(TKey key)
        {
            return GetGrouping(key, false) != null;
        }

        public bool Contains(TKey key, TElement item)
        {
            var group = GetGrouping(key, false);
            if (group == null)
                return false;

            return group.Contains(item);
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            var grp = _lastGroup;
            if (grp != null)
            {
                do
                {
                    grp = grp.NextGroup;
                    yield return grp;
                } while (grp != _lastGroup);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int GetHashCode(TKey key)
        {
            if (key == null)
                return 0;

            var hash = _comparer.GetHashCode(key) & 0x7FFFFFFF;
            return hash;
        }

        private Grouping GetGrouping(TKey key, bool create)
        {
            var hashCode = GetHashCode(key);
            var index = hashCode % _groupings.Length;

            var grp = _groupings[index];
            while (grp != null)
            {
                if (grp.HashCode == hashCode && _comparer.Equals(grp.Key, key))
                    return grp;

                grp = grp.HashNext;
            }

            if (!create)
                return null;

            if (_count == _groupings.Length)
            {
                IncreaseSize();
                index = hashCode % _groupings.Length;
            }

            var newGrp = new Grouping(1)
            {
                Key = key,
                HashCode = hashCode,
                HashNext = _groupings[index]
            };

            _groupings[index] = newGrp;

            if (_lastGroup == null)
                newGrp.NextGroup = newGrp;
            else
            {
                newGrp.NextGroup = _lastGroup.NextGroup;
                _lastGroup.NextGroup = newGrp;
            }

            _lastGroup = newGrp;
            _count++;

            return newGrp;
        }

        private void IncreaseSize()
        {
            int newSize = checked(_count * 2 + 1);
            var newArray = new Grouping[newSize];

            var grp = _lastGroup;
            do
            {
                grp = grp.NextGroup;

                var index = grp.HashCode % newSize;

                grp.HashNext = newArray[index];
                newArray[index] = grp;
            } while (grp != _lastGroup);

            _groupings = newArray;
        }

        [Serializable]
        private class Grouping : List<TElement>, IGrouping<TKey, TElement>
        {
            public int HashCode;

            public Grouping HashNext;
            public Grouping NextGroup;

            public Grouping(int capacity) : base(capacity)
            {
            }

            private TKey key;
            public TKey Key
            {
                get { return key; }
                set { key = value; }
            }
        }
    }
}
