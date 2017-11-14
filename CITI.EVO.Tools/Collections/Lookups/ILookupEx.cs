using System.Linq;

namespace CITI.EVO.Tools.Collections.Lookups
{
    public interface ILookupEx<TKey, TElement> : ILookup<TKey, TElement>
    {
        void Add(TKey key, TElement element);

        bool Remove(TKey key);
        bool Remove(TKey key, TElement element);

        bool Contains(TKey key, TElement element);

        void Clear();
    }
}