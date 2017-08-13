using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Structures
{
    public class Graph<TItem> : IEnumerable<TItem>
    {
        private readonly IDictionary<TItem, GraphNode<TItem>> _nodes;

        public Graph() : this(null)
        {
        }
        public Graph(IEnumerable<GraphNode<TItem>> nodes)
        {
            if (_nodes == null)
                _nodes = new Dictionary<TItem, GraphNode<TItem>>();
            else
                _nodes = nodes.ToDictionary(n => n.Item);
        }

        public int Count
        {
            get { return _nodes.Count; }
        }

        public IEnumerable<GraphNode<TItem>> Nodes
        {
            get { return _nodes.Values; }
        }

        public void AddNode(TItem item)
        {
            AddNode(new GraphNode<TItem>(item));
        }
        public void AddNode(GraphNode<TItem> node)
        {
            _nodes.Add(node.Item, node);
        }

        public void AddDirectedEdge(TItem from, TItem to, int cost)
        {
            var fromNode = Find(from);
            var toNode = Find(to);

            AddDirectedEdge(fromNode, toNode, cost);
        }
        public void AddDirectedEdge(GraphNode<TItem> from, GraphNode<TItem> to, int cost)
        {
            from.Neighbors.Add(to.Item, to);
            from.Costs.Add(to.Item, cost);
        }

        public void AddUndirectedEdge(TItem from, TItem to, int cost)
        {
            var fromNode = Find(from);
            var toNode = Find(to);

            AddUndirectedEdge(fromNode, toNode, cost);
        }
        public void AddUndirectedEdge(GraphNode<TItem> from, GraphNode<TItem> to, int cost)
        {
            from.Neighbors.Add(to.Item, to);
            from.Costs.Add(to.Item, cost);

            to.Neighbors.Add(from.Item, from);
            to.Costs.Add(from.Item, cost);
        }

        public bool Contains(TItem item)
        {
            return Find(item) != null;
        }

        public bool Remove(TItem item)
        {
            // first remove the node from the nodeset
            var nodeToRemove = Find(item);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            _nodes.Remove(nodeToRemove.Item);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (var gnode in _nodes.Values)
            {
                gnode.Neighbors.Remove(nodeToRemove.Item);
                gnode.Costs.Remove(nodeToRemove.Item);
            }

            return true;
        }

        public GraphNode<TItem> Find(TItem item)
        {
            GraphNode<TItem> node;
            if (_nodes.TryGetValue(item, out node))
                return node;

            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            foreach (var node in _nodes)
                yield return node.Value.Item;
        }
    }
}