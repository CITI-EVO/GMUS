using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class SynchronizedList<TItem> : IList<TItem>
    {
        private readonly IList<TItem> _list;

        public SynchronizedList(IList<TItem> list)
        {
            _list = list;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            lock (_list)
            {
                foreach (var item in _list)
                    yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TItem item)
        {
            lock (_list)
                _list.Add(item);
        }

        public void Clear()
        {
            lock (_list)
                _list.Clear();
        }

        public bool Contains(TItem item)
        {
            lock (_list)
                return _list.Contains(item);
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            lock (_list)
                _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(TItem item)
        {
            lock (_list)
                return _list.Remove(item);
        }

        public int Count
        {
            get
            {
                lock (_list)
                    return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                lock (_list)
                    return _list.IsReadOnly;
            }
        }

        public int IndexOf(TItem item)
        {
            lock (_list)
                return _list.IndexOf(item);
        }

        public void Insert(int index, TItem item)
        {
            lock (_list)
                _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            lock (_list)
                _list.RemoveAt(index);
        }

        public TItem this[int index]
        {
            get
            {
                lock (_list)
                    return _list[index];
            }
            set
            {
                lock (_list)
                    _list[index] = value;
            }
        }
    }
}