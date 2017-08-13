using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CITI.EVO.Tools.Structures
{
    public class RWayTrie<TValue> : IEnumerable<KeyValuePair<String, TValue>>
    {
        private Encoding _encoding;
        private RWayNode<TValue> _root;

        public RWayTrie() : this(Encoding.Default)
        {
        }
        public RWayTrie(Encoding encoding)
        {
            _encoding = encoding;
        }

        public TValue this[String key]
        {
            get
            {
                var nodes = GetNodes(key);

                foreach (var node in nodes)
                    return node.Value;

                throw new Exception();
            }
            set
            {
                Insert(key, value, true);
            }
        }

        public void Add(String key, TValue value)
        {
            Insert(key, value, false);
        }

        public void Remove(String partOfKey)
        {
            var parent = (RWayNode<TValue>)null;
            var index = -1;
            var node = _root;

            if (node == null)
                return;

            var bytes = _encoding.GetBytes(partOfKey);
            for (int i = 0; i < bytes.Length; i++)
            {
                index = bytes[i];

                var child = node[index];
                if (child == null)
                    return;

                parent = node;
                node = child;
            }

            if (parent != null)
                parent[index] = null;
        }

        public bool Contains(String partOfKey)
        {
            var node = _root;
            if (node == null)
                return false;

            var nodes = GetNodes(partOfKey);
            foreach (var child in nodes)
                return true;

            return false;
        }

        public IEnumerable<KeyValuePair<String, TValue>> Find(String partOfKey)
        {
            var node = _root;
            if (node == null)
                yield break;

            var nodes = GetNodes(partOfKey);
            foreach (var child in nodes)
                yield return new KeyValuePair<string, TValue>(child.Key, child.Value);
        }

        private void Insert(String key, TValue value, bool update)
        {
            if (_root == null)
                _root = new RWayNode<TValue>();

            var node = _root;

            var bytes = _encoding.GetBytes(key);
            for (int i = 0; i < bytes.Length; i++)
            {
                var @byte = bytes[i];

                var child = node[@byte];
                if (child == null)
                {
                    child = new RWayNode<TValue> { Byte = @byte };

                    node[@byte] = child;
                }

                node = child;
            }

            if (!update && node.IsLeaf)
                throw new Exception();

            node.Key = key;
            node.IsLeaf = true;
            node.Value = value;
        }

        private IEnumerable<RWayNode<TValue>> GetNodes(String partOfKey)
        {
            var node = _root;
            if (node == null)
                yield break;

            var bytes = _encoding.GetBytes(partOfKey);
            for (int i = 0; i < bytes.Length; i++)
            {
                var @byte = bytes[i];
                node = node[@byte];

                if (node == null)
                    yield break;
            }

            foreach (var n in Traversal(node))
            {
                if (n.IsLeaf)
                    yield return n;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<String, TValue>> GetEnumerator()
        {
            foreach (var node in Traversal(_root))
            {
                if (node.IsLeaf)
                    yield return new KeyValuePair<String, TValue>(node.Key, node.Value);
            }
        }

        private IEnumerable<RWayNode<TValue>> Traversal(RWayNode<TValue> node)
        {
            var stack = new Stack<RWayNode<TValue>>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();
                yield return n;

                foreach (var child in node)
                    stack.Push(child);
            }
        }
    }
}
