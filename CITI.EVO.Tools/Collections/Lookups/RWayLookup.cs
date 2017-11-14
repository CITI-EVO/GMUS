using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Collections.Lookups
{
    [Serializable]
    public class RWayLookup<TElement> : ILookupEx<String, TElement>
    {
        private int _count;
        private RWayNode _root;

        public int Count
        {
            get { return _count; }
        }

        public IEnumerable<TElement> this[String key]
        {
            get
            {
                var node = Find(key);
                if (node == null || node.Grouping == null)
                    return Enumerable.Empty<TElement>();

                return node.Grouping;
            }
        }

        public IEnumerator<IGrouping<String, TElement>> GetEnumerator()
        {
            foreach (var node in Traversal(_root))
            {
                if (node.Grouping != null)
                    yield return node.Grouping;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(String key, TElement element)
        {
            key = (key ?? String.Empty);

            var node = _root;
            foreach (var @char in key)
            {
                var child = node[@char];
                if (child == null)
                {
                    child = new RWayNode();
                    node[@char] = child;
                }

                node = child;
            }

            if (node.Grouping == null)
            {
                node.Grouping = new Grouping();
                _count++;
            }

            node.Grouping.Key = key;
            node.Grouping.Add(element);
        }

        public bool Remove(String key)
        {
            var node = Find(key);
            if (node == null || node.Grouping == null)
                return false;

            node.Grouping = null;
            _count--;

            return true;
        }
        public bool Remove(String key, TElement element)
        {
            var node = Find(key);
            if (node != null && node.Grouping != null && node.Grouping.Remove(element))
                return true;

            return false;
        }

        public bool Contains(String key)
        {
            var node = Find(key);
            return (node != null && node.Grouping != null);
        }
        public bool Contains(String key, TElement element)
        {
            var node = Find(key);
            if (node != null && node.Grouping != null && node.Grouping.Contains(element))
                return true;

            return false;
        }

        public IEnumerable<IGrouping<String, TElement>> Search(String prefix)
        {
            var node = Find(prefix);
            foreach (var child in Traversal(node))
            {
                if (child.Grouping != null)
                    yield return child.Grouping;
            }
        }

        public void Clear()
        {
            _root = new RWayNode();
            _count = 0;
        }

        private RWayNode Find(String key)
        {
            key = (key ?? String.Empty);

            var node = _root;
            foreach (var @char in key)
            {
                if (node == null || node[@char] == null)
                    return null;

                node = node[@char];
            }

            return node;
        }

        private IEnumerable<RWayNode> Traversal(RWayNode node)
        {
            if (node == null)
                yield break;

            var stack = new Stack<RWayNode>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();
                foreach (var child in n)
                {
                    if (child.Value != null)
                        stack.Push(child.Value);
                }

                yield return n;
            }
        }

        [Serializable]
        private class RWayNode : IndexLookup<RWayNode>
        {
            public Grouping Grouping;
        }

        [Serializable]
        private class Grouping : List<TElement>, IGrouping<String, TElement>
        {
            private String _key;
            public String Key
            {
                get { return _key; }
                set { _key = value; }
            }
        }
    }
}
