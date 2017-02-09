using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections
{
    public interface IComparableGroup<TKey, TValue> : IDictionary<TKey, TValue>, IComparable<IDictionary<TKey, TValue>>
    {
    }
}