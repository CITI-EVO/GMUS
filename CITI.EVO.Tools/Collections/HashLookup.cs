using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class HashLookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        private readonly IEqualityComparer<TKey> _comparer;

        private Grouping[] _groupings;
        private Grouping _lastGrouping;
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

            for (var group = _groupings[index]; group != null; parentGroup = group, group = group.HashNext)
            {
                if (group.HashCode == hashCode && _comparer.Equals(group.Key, key))
                {
                    if (parentGroup == null)
                    {
                        _groupings[index] = null;
                    }
                    else
                    {
                        parentGroup.NextGroup = group.NextGroup;
                        parentGroup.HashNext = group.HashNext;
                    }

                    _count--;

                    return true;
                }
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
            var index = hashCode % _groupings.Length;

            for (var group = _groupings[index]; group != null; group = group.HashNext)
            {
                if (group.HashCode == hashCode && _comparer.Equals(group.Key, key))
                    return group;
            }

            if (create)
            {
                if (_count >= _groupings.Length)
                {
                    EnsureCapacity();
                    index = hashCode % _groupings.Length;
                }

                var group = new Grouping
                {
                    Key = key,
                    HashCode = hashCode,
                    HashNext = _groupings[index]
                };

                _groupings[index] = group;

                if (_lastGrouping == null)
                {
                    group.NextGroup = group;
                }
                else
                {
                    group.NextGroup = _lastGrouping.NextGroup;
                    _lastGrouping.NextGroup = group;
                }

                _lastGrouping = group;
                _count++;

                return group;
            }

            return null;
        }

        private int GetHashCode(TKey key)
        {
            if (key == null)
                return 0;

            return _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        private void EnsureCapacity()
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
            private TKey _key;
            private int _hashCode;

            private Grouping _hashNext;
            private Grouping _nextGroup;

            public TKey Key
            {
                get { return _key; }
                set { _key = value; }
            }

            public int HashCode
            {
                get { return _hashCode; }
                set { _hashCode = value; }
            }

            public Grouping HashNext
            {
                get { return _hashNext; }
                set { _hashNext = value; }
            }

            public Grouping NextGroup
            {
                get { return _nextGroup; }
                set { _nextGroup = value; }
            }
        }
    }
}
