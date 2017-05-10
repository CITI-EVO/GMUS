using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class SynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;

        public SynchronizedDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (_dictionary)
            {
                foreach (var pair in _dictionary)
                {
                    yield return pair;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (_dictionary)
            {
                _dictionary.Add(item);
            }
        }

        public void Clear()
        {
            lock (_dictionary)
            {
                _dictionary.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_dictionary)
            {
                return _dictionary.Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_dictionary)
            {
                _dictionary.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_dictionary)
            {
                return _dictionary.Remove(item);
            }
        }

        public int Count
        {
            get
            {
                lock (_dictionary)
                {
                    return _dictionary.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                lock (_dictionary)
                {
                    return _dictionary.IsReadOnly;
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (_dictionary)
            {
                return _dictionary.ContainsKey(key);
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (_dictionary)
            {
                _dictionary.Add(key, value);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_dictionary)
            {
                return _dictionary.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_dictionary)
            {
                return _dictionary.TryGetValue(key, out value);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_dictionary)
                {
                    return _dictionary[key];
                }
            }
            set
            {
                lock (_dictionary)
                {
                    _dictionary[key] = value;
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                lock (_dictionary)
                {
                    return _dictionary.Keys;
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (_dictionary)
                {
                    return _dictionary.Values;
                }
            }
        }
    }
}
