using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Structures
{
    public class GraphNode<TItem>
    {
        private TItem _item;

        private IDictionary<TItem, int> _costs;
        private IDictionary<TItem, GraphNode<TItem>> _neighbors;

        public GraphNode()
        {
        }
        public GraphNode(TItem item) : this(item, null)
        {
        }
        public GraphNode(TItem item, IEnumerable<GraphNode<TItem>> neighbors)
        {
            _item = item;
            _neighbors = neighbors.ToDictionary(n => n.Item);
        }

        public TItem Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public IDictionary<TItem, GraphNode<TItem>> Neighbors
        {
            get
            {
                _neighbors = (_neighbors ?? new Dictionary<TItem, GraphNode<TItem>>());
                return _neighbors;
            }
        }

        public IDictionary<TItem, int> Costs
        {
            get
            {
                _costs = (_costs ?? new Dictionary<TItem, int>());
                return _costs;
            }
        }
    }
}
