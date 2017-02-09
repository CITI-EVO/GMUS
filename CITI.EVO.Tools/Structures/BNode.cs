using System.Collections.Generic;

namespace CITI.EVO.Tools.Structures
{
    public class BNode<TItem>
    {
        private readonly int _degree;
        private readonly int _maxDeg;

        private IList<TItem> _items;
        private IList<BNode<TItem>> _nodes;

        public BNode(int degree)
        {
            _degree = degree;
            _maxDeg = degree * 2;
        }

        public bool IsLeaf
        {
            get { return _nodes.Count == 0; }
        }

        public IList<TItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public IList<BNode<TItem>> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        public bool HasReachedMaxSize
        {
            get { return Items.Count == _maxDeg - 1; }
        }

        public bool HasReachedMinSize
        {
            get { return Items.Count == _degree - 1; }
        }

        public IEnumerable<TItem> GetItems(int index, int count)
        {
            while (count-- > 0)
                yield return _items[index++];
        }

        public void InsertItem(TItem key)
        {
            _items.Add(key);
        }
        public void InsertItem(int index, TItem item)
        {
            _items.Insert(index, item);
        }
        public void InsertItems(IEnumerable<TItem> items)
        {
            foreach (var item in items)
                _items.Add(item);
        }

        public void RemoveItem(int index)
        {
            _items.RemoveAt(index);
        }
        public void RemoveItems(int index, int count)
        {
            while (count-- > 0)
                _items.RemoveAt(index);
        }

        public IEnumerable<BNode<TItem>> GetNodes(int index, int count)
        {
            while (count-- > 0)
                yield return _nodes[index++];
        }

        public void InsertNode(BNode<TItem> node)
        {
            _nodes.Add(node);
        }
        public void InsertNode(int index, BNode<TItem> node)
        {
            _nodes.Insert(index, node);
        }
        public void InsertNodes(IEnumerable<BNode<TItem>> nodes)
        {
            foreach (var node in nodes)
                _nodes.Add(node);
        }

        public void RemoveNode(int index)
        {
            _nodes.RemoveAt(index);
        }
        public void RemoveNodes(int index, int count)
        {
            while (count-- > 0)
                _nodes.RemoveAt(index);
        }
    }
}