using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Collections.Lookups
{
    public class LookupDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly EntityLookup<TKey, TValue> _lookup;

        public LookupDictionary() : this(7, EqualityComparer<TKey>.Default)
        {
        }

        public LookupDictionary(int capacity) : this(capacity, EqualityComparer<TKey>.Default)
        {
        }

        public LookupDictionary(IEqualityComparer<TKey> comparer) : this(7, comparer)
        {
        }

        public LookupDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _lookup = new EntityLookup<TKey, TValue>(comparer, capacity);
        }

        public LookupDictionary(IDictionary<TKey, TValue> dictionary) : this(dictionary, EqualityComparer<TKey>.Default)
        {
        }

        public LookupDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _lookup = new EntityLookup<TKey, TValue>(comparer, dictionary.Count + 1);

            foreach (var pair in dictionary)
                _lookup.Add(pair.Key, pair.Value);
        }

        public int Count
        {
            get { return _lookup.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _lookup[key];
            }
            set
            {
                _lookup[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                var items = _lookup.Select(n => n.Key);
                return items.ToList();
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                var items = _lookup.Select(n => n.Value);
                return items.ToList();
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _lookup.Contains(key);
        }

        public void Add(TKey key, TValue value)
        {
            _lookup.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return _lookup.Remove(key);
        }

        public void Clear()
        {
            _lookup.Clear();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _lookup.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _lookup.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _lookup.Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _lookup.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var pair in this)
            {
                if (arrayIndex >= array.Length)
                    break;

                array[arrayIndex++] = pair;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _lookup.Remove(item.Key);
        }
    }
}