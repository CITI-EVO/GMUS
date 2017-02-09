using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections
{
    public class CollectionPartitioner<TItem> : IEnumerable<IEnumerable<TItem>>
    {
        private readonly int _partitionSize;
        private readonly IEnumerable<TItem> _collection;

        public CollectionPartitioner(IEnumerable<TItem> collection)
            : this(collection, 64)
        {
        }

        public CollectionPartitioner(IEnumerable<TItem> collection, int partitionSize)
        {
            _partitionSize = partitionSize;
            _collection = collection;
        }

        public IEnumerator<IEnumerable<TItem>> GetEnumerator()
        {
            var list = new List<TItem>(_partitionSize);

            foreach (var item in _collection)
            {
                list.Add(item);

                if (list.Count >= _partitionSize)
                {
                    yield return new List<TItem>(list);
                    list.Clear();
                }
            }

            if (list.Count > 0)
            {
                yield return new List<TItem>(list);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
