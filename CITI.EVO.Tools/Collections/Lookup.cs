using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class Lookup<TKey, TElement> : ILookup<TKey, TElement>
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
            var hashCode = GetKeyHashCode(key);
            var index = hashCode % _groupings.Length;

            var grp = _groupings[index];
            while (grp != null)
            {
                if (grp.hashCode == hashCode && _comparer.Equals(grp.key, key))
                    break;

                grp = grp.hashNext;
            }

            if (grp == null)
                return false;

            Grouping prevHash, prevGroup;
            foreach (var grouping in _groupings)
            {
                if (grouping != null)
                {
                    if (grouping.nextGroup == grp)
                        prevGroup = grouping;

                    if (grouping.hashNext == grp)
                        prevHash = grouping;
                }
            }

            throw new NotImplementedException();
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
                    grp = grp.nextGroup;
                    yield return grp;
                } while (grp != _lastGroup);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int GetKeyHashCode(TKey key)
        {
            if (key == null)
                return 0;

            var hash = _comparer.GetHashCode(key) & 0x7FFFFFFF;
            return hash;
        }

        private Grouping GetGrouping(TKey key, bool create)
        {
            var hashCode = GetKeyHashCode(key);
            var index = hashCode % _groupings.Length;

            var grp = _groupings[index];
            while (grp != null)
            {
                if (grp.hashCode == hashCode && _comparer.Equals(grp.key, key))
                    return grp;

                grp = grp.hashNext;
            }

            //for (var grp = groupings[hashCode % groupings.Length]; grp != null; grp = grp.hashNext)
            //{
            //    if (grp.hashCode == hashCode && comparer.Equals(grp.key, key))
            //        return grp;
            //}

            if (!create)
                return null;

            if (_count == _groupings.Length)
                IncreaseSize();

            var newGrp = new Grouping(1)
            {
                key = key,
                hashCode = hashCode,
                hashNext = _groupings[index]
            };

            _groupings[index] = newGrp;

            if (_lastGroup == null)
                newGrp.nextGroup = newGrp;
            else
            {
                newGrp.nextGroup = _lastGroup.nextGroup;
                _lastGroup.nextGroup = newGrp;
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
                grp = grp.nextGroup;

                var index = grp.hashCode % newSize;

                grp.hashNext = newArray[index];
                newArray[index] = grp;
            } while (grp != _lastGroup);

            _groupings = newArray;
        }

        [Serializable]
        private class Grouping : List<TElement>, IGrouping<TKey, TElement>
        {
            public TKey key;
            public int hashCode;

            public Grouping hashNext;
            public Grouping nextGroup;

            public Grouping(int capacity) : base(capacity)
            {
            }

            public TKey Key
            {
                get { return key; }
            }
        }
    }
}
