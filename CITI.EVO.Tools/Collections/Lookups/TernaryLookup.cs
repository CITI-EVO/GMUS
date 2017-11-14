using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Collections.Lookups
{
    public class TernaryLookup<TElement> : ILookupEx<string, TElement>
    {
        private int _count;
        private TernaryNode _root;

        public int Count
        {
            get { return _count; }
        }

        public IEnumerable<TElement> this[String key]
        {
            get
            {
                var node = GetNode(key, false);
                if (node == null || node.Grouping == null)
                    return Enumerable.Empty<TElement>();

                return node.Grouping;
            }
        }

        public IEnumerator<IGrouping<String, TElement>> GetEnumerator()
        {
            if (_root == null)
                yield break;

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
            if (_root == null)
                _root = new TernaryNode();

            if (String.IsNullOrEmpty(key))
            {
                _root.Grouping = (_root.Grouping ?? new Grouping());
                _root.Grouping.Add(element);

                return;
            }

            var index = 0;
            var node = _root;
            var length = key.Length - 1;

            while (true)
            {
                var @char = key[index];

                if (@char < node.Char)
                {
                    node.Left = (node.Left ?? new TernaryNode { Char = @char });
                    node = node.Left;
                }
                else if (@char > node.Char)
                {
                    node.Right = (node.Right ?? new TernaryNode { Char = @char });
                    node = node.Right;
                }
                else if (index < length)
                {
                    node.Middle = (node.Middle ?? new TernaryNode { Char = key[index + 1] });
                    node = node.Middle;

                    index++;
                }
                else
                {
                    node.Leaf = true;

                    if (node.Grouping == null)
                    {
                        node.Grouping = new Grouping { Key = key };
                        _count++;
                    }

                    node.Grouping.Add(element);
                    return;
                }
            }
        }

        public bool Remove(String key)
        {
            var node = GetNode(key, false);
            if (node == null || !node.Leaf || node.Grouping == null)
                return false;

            node.Leaf = false;
            node.Grouping = null;

            return true;
        }
        public bool Remove(String key, TElement element)
        {
            var node = GetNode(key, false);
            if (node == null || !node.Leaf || node.Grouping == null)
                return false;

            return node.Grouping.Remove(element);
        }

        public bool Contains(String key)
        {
            var node = GetNode(key, false);
            return (node != null);
        }
        public bool Contains(String key, TElement element)
        {
            var node = GetNode(key, false);
            if (node == null || node.Grouping == null)
                return false;

            return node.Grouping.Contains(element);
        }

        public IEnumerable<IGrouping<String, TElement>> Search(String prefix)
        {
            var node = GetNode(prefix, true);
            foreach (var child in Traversal(node))
            {
                if (child.Grouping != null)
                    yield return child.Grouping;
            }
        }


        public void Clear()
        {
            _root = null;
            _count = 0;
        }

        private TernaryNode GetNode(String key, bool byPrefix)
        {
            var node = _root;
            var index = 0;

            while (node != null)
            {
                var @char = key[index];

                if (@char < node.Char)
                    node = node.Left;
                else if (@char > node.Char)
                    node = node.Right;
                else if (index < key.Length - 1)
                {
                    node = node.Middle;
                    index++;
                }
                else
                    break;
            }

            if (node != null && !node.Leaf && !byPrefix)
                return null;

            return node;
        }

        private IEnumerable<TernaryNode> Traversal(TernaryNode node)
        {
            var stack = new Stack<TernaryNode>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();

                if (n.Left != null)
                    stack.Push(n.Left);

                if (n.Middle != null)
                    stack.Push(n.Middle);

                if (n.Right != null)
                    stack.Push(n.Right);

                yield return n;
            }
        }

        [Serializable]
        private class TernaryNode
        {
            public char Char;
            public bool Leaf;

            public TernaryNode Left;
            public TernaryNode Middle;
            public TernaryNode Right;

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