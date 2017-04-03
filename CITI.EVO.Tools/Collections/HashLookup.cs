using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITI.EVO.Tools.Collections
{
    public class HashLookup<TKey, TValue> : ILookup<TKey, TValue>
    {
        private readonly IDictionary<TKey, LookupGroup> _lookup;

        public HashLookup() : this(EqualityComparer<TKey>.Default)
        {
        }

        public HashLookup(IEqualityComparer<TKey> comparer)
        {
            _lookup = new Dictionary<TKey, LookupGroup>(comparer);
        }

        public void Add(TKey key, TValue value)
        {
            LookupGroup group;
            if (!_lookup.TryGetValue(key, out group))
            {
                group = new LookupGroup(key);
                _lookup.Add(key, group);
            }

            group.Add(value);
        }

        public void Remove(TKey key)
        {
            _lookup.Remove(key);
        }

        public void Remove(TKey key, TValue value)
        {
            LookupGroup group;
            if (!_lookup.TryGetValue(key, out group))
                return;

            group.Remove(value);
        }

        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
        {
            foreach (var group in _lookup.Values)
                yield return group;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(TKey key)
        {
            return _lookup.ContainsKey(key);
        }

        public int Count
        {
            get { return _lookup.Count; }
        }

        public IEnumerable<TValue> this[TKey key]
        {
            get
            {
                LookupGroup group;
                if (_lookup.TryGetValue(key, out group))
                {
                    foreach (var item in group)
                        yield return item;
                }
            }
        }

        private class LookupGroup : List<TValue>, IGrouping<TKey, TValue>
        {
            public LookupGroup(TKey key)
            {
                Key = key;
            }

            public TKey Key { get; private set; }
        }
    }
}
