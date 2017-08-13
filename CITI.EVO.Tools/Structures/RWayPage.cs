using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Structures
{
    [Serializable]
    public class RWayPage<TValue> : IEnumerable<RWayNode<TValue>>
    {
        private RWayNode<TValue>[] _nodes;

        public RWayNode<TValue> this[int index]
        {
            get
            {
                if (_nodes == null)
                    return null;

                return _nodes[index];
            }
            set
            {
                _nodes = (_nodes ?? new RWayNode<TValue>[16]);
                _nodes[index] = value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<RWayNode<TValue>> GetEnumerator()
        {
            if (_nodes == null)
                yield break;

            foreach (var node in _nodes)
            {
                if (node == null)
                    continue;

                yield return node;
            }
        }
    }
}