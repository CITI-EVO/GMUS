using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Structures
{
    [Serializable]
    public class RWayNode<TValue> : IEnumerable<RWayNode<TValue>>
    {
        private RWayPage<TValue>[] _pages;

        public byte Byte { get; set; }

        public bool IsLeaf { get; set; }

        public String Key { get; set; }

        public TValue Value { get; set; }

        public RWayNode<TValue> this[int index]
        {
            get
            {
                if (_pages == null)
                    return null;

                var pageIdx = index / 16;
                var nodeIdx = index % 16;

                var page = _pages[pageIdx];
                if (page == null)
                    return null;

                return page[nodeIdx];
            }
            set
            {
                _pages = (_pages ?? new RWayPage<TValue>[16]);

                var pageIdx = index / 16;
                var nodeIdx = index % 16;

                var page = _pages[pageIdx];
                page[nodeIdx] = value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<RWayNode<TValue>> GetEnumerator()
        {
            if (_pages == null)
                yield break;

            foreach (var page in _pages)
            {
                if (page == null)
                    continue;

                foreach (var node in page)
                {
                    if (node == null)
                        continue;

                    yield return node;
                }
            }
        }
    }
}