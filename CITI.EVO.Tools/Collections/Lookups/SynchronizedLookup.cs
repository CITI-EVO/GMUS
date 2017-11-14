using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Collections.Lookups
{
    [Serializable]
    public class SynchronizedLookup<TKey, TElement> : ILookupEx<TKey, TElement>
    {
        private readonly ILookup<TKey, TElement> _lookup;

        public SynchronizedLookup(ILookup<TKey, TElement> lookup)
        {
            _lookup = lookup;
        }

        public int Count
        {
            get
            {
                lock (_lookup)
                    return _lookup.Count;
            }
        }

        public void Clear()
        {
            lock (_lookup)
            {
                var lp = _lookup as ILookupEx<TKey, TElement>;
                if (lp == null)
                    throw new NotSupportedException();

                lp.Clear();
            }
        }

        public bool Contains(TKey key)
        {
            lock (_lookup)
                return _lookup.Contains(key);
        }
        public bool Contains(TKey key, TElement element)
        {
            lock (_lookup)
            {
                var lp = _lookup as ILookupEx<TKey, TElement>;
                if (lp == null)
                    throw new NotSupportedException();

                return lp.Contains(key, element);
            }
        }

        public void Add(TKey key, TElement element)
        {
            lock (_lookup)
            {
                var lp = _lookup as ILookupEx<TKey, TElement>;
                if (lp == null)
                    throw new NotSupportedException();

                lp.Add(key, element);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_lookup)
            {
                var lp = _lookup as ILookupEx<TKey, TElement>;
                if (lp == null)
                    throw new NotSupportedException();

                return lp.Remove(key);
            }
        }
        public bool Remove(TKey key, TElement element)
        {
            lock (_lookup)
            {
                var lp = _lookup as ILookupEx<TKey, TElement>;
                if (lp == null)
                    throw new NotSupportedException();

                return lp.Remove(key, element);
            }
        }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                lock (_lookup)
                    return _lookup[key];
            }
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            lock (_lookup)
            {
                foreach (var pair in _lookup)
                    yield return pair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}