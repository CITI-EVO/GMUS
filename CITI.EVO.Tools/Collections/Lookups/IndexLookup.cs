using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections.Lookups
{
    [Serializable]
    public class IndexLookup<TElement> : IEnumerable<KeyValuePair<int, TElement>>
    {
        private readonly int _capacity;

        private Entry[] _entries;
        private Entry _lastEnt;

        private int _count;

        public IndexLookup() : this(7)
        {
        }
        public IndexLookup(int capacity)
        {
            _capacity = capacity;
        }

        public int Count
        {
            get { return _count; }
        }

        public TElement this[int key]
        {
            get
            {
                var entity = GetEntry(key, false);
                if (entity != null)
                    return entity.Element;

                return default(TElement);
            }
            set
            {
                var entity = GetEntry(key, true);
                entity.Element = value;
            }
        }

        public bool Remove(int key)
        {
            if (_entries == null)
                return false;

            var index = key % _entries.Length;

            var prevEnt = (Entry)null;
            var entry = _entries[index];

            while (entry != null)
            {
                if (entry.Key == key)
                {
                    if (prevEnt == null)
                        _entries[index] = null;
                    else
                    {
                        prevEnt.NextEntry = entry.NextEntry;
                        prevEnt.HashNext = entry.HashNext;
                    }

                    _count--;

                    return true;
                }

                prevEnt = entry;
                entry = entry.HashNext;
            }

            return false;
        }

        public bool Contains(int key)
        {
            return GetEntry(key, false) != null;
        }

        public void Clear()
        {
            _entries = null;
            _count = 0;
        }

        public IEnumerator<KeyValuePair<int, TElement>> GetEnumerator()
        {
            var entity = _lastEnt;
            if (entity != null)
            {
                do
                {
                    entity = entity.NextEntry;

                    var pair = new KeyValuePair<int, TElement>(entity.Key, entity.Element);
                    yield return pair;

                } while (entity != _lastEnt);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Entry GetEntry(int key, bool create)
        {
            if (_entries == null)
            {
                if (create)
                    _entries = new Entry[_capacity];
                else
                    return null;
            }

            var index = key % _entries.Length;

            for (var entry = _entries[index]; entry != null; entry = entry.HashNext)
            {
                if (entry.Key == key)
                    return entry;
            }

            if (!create)
                return null;

            if (_count >= _entries.Length)
            {
                IncreaseSize();
                index = key % _entries.Length;
            }

            var newEntry = new Entry
            {
                Key = key,
                HashNext = _entries[index]
            };

            _entries[index] = newEntry;

            if (_lastEnt == null)
            {
                newEntry.NextEntry = newEntry;
            }
            else
            {
                newEntry.NextEntry = _lastEnt.NextEntry;
                _lastEnt.NextEntry = newEntry;
            }

            _lastEnt = newEntry;
            _count++;

            return newEntry;
        }

        private void IncreaseSize()
        {
            var newSize = checked(_count * 2 + 1);
            var newEntries = new Entry[newSize];

            var entity = _lastEnt;

            do
            {
                entity = entity.NextEntry;

                var index = entity.Key % newSize;
                entity.HashNext = newEntries[index];

                newEntries[index] = entity;
            } while (entity != _lastEnt);

            _entries = newEntries;
        }

        [Serializable]
        private class Entry
        {
            public int Key;
            public TElement Element;

            public Entry HashNext;
            public Entry NextEntry;
        }
    }
}
