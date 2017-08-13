using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Structures
{
    public class TernaryTrie<TValue> : IEnumerable<KeyValuePair<String, TValue>>
    {
        private TernaryNode<TValue> _root;

        public TernaryTrie()
        {
        }

        public TValue this[String partOfKey]
        {
            get
            {
                var nodes = GetNodes(partOfKey);

                foreach (var node in nodes)
                    return node.Value;

                throw new Exception();
            }
            set
            {
                Insert(partOfKey, value, true);
            }
        }

        public void Remove(String partOfKey)
        {

        }

        public bool Contains(String partOfKey)
        {
            foreach (var n in GetNodes(partOfKey))
                return true;

            return false;
        }

        public IEnumerable<TValue> Find(String partOfKey)
        {
            foreach (var n in GetNodes(partOfKey))
                yield return n.Value;
        }

        public IEnumerator<KeyValuePair<String, TValue>> GetEnumerator()
        {
            foreach (var n in Traversal(_root))
            {
                if (n.IsLeaf)
                    yield return new KeyValuePair<String, TValue>(n.Key, n.Value);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Insert(String partOfKey, TValue value, bool update)
        {
            var index = 0;
            var length = partOfKey.Length - 1;

            if (_root == null)
                _root = new TernaryNode<TValue> { Char = partOfKey[0] };

            var node = _root;

            while (true)
            {
                var @char = partOfKey[index];
                if (@char < node.Char)
                {
                    if (node.Left == null)
                        node.Left = new TernaryNode<TValue> { Char = @char };

                    node = node.Left;
                }
                else if (@char > node.Char)
                {
                    if (node.Right == null)
                        node.Right = new TernaryNode<TValue> { Char = @char };

                    node = node.Right;
                }
                else if (index < length)
                {
                    if (node.Middle == null)
                        node.Middle = new TernaryNode<TValue> { Char = partOfKey[index + 1] };

                    node = node.Middle;
                    index++;
                }
                else
                {
                    if (!node.IsLeaf)
                    {
                        node.IsLeaf = true;
                        node.Key = partOfKey;
                        node.Value = value;

                        return;
                    }

                    if (update)
                    {
                        node.Key = partOfKey;
                        node.Value = value;

                        return;
                    }

                    throw new Exception("Key already exists");
                }
            }
        }

        private IEnumerable<TernaryNode<TValue>> GetNodes(String partOfKey)
        {
            var index = 0;
            var node = _root;

            while (node != null)
            {
                var @char = partOfKey[index];
                if (@char < node.Char)
                    node = node.Left;
                else if (@char > node.Char)
                    node = node.Right;
                else if (index < partOfKey.Length - 1)
                {
                    node = node.Middle;
                    index++;
                }
                else
                    break;
            }

            if (node != null)
            {
                foreach (var n in Traversal(node))
                {
                    if (n.IsLeaf)
                        yield return n;
                }
            }
        }

        private IEnumerable<TernaryNode<TValue>> Traversal(TernaryNode<TValue> parent)
        {
            var stack = new Stack<TernaryNode<TValue>>();
            stack.Push(parent);

            while (stack.Count > 0)
            {
                var node = stack.Pop();

                if (node.Left != null)
                    stack.Push(node.Left);

                if (node.Middle != null)
                    stack.Push(node.Middle);

                if (node.Right != null)
                    stack.Push(node.Right);

                if (node.IsLeaf)
                    yield return node;
            }
        }
    }
}
