using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Cache
{
    public interface ICommonObjectCache
    {
        IEnumerable<KeyValuePair<String, Object>> GetObjects();

        TObject GetObject<TObject>(String cacheKey) where TObject : class;

        TObject InitObject<TObject>(String cacheKey, TimeSpan timeout, Func<TObject> initializer) where TObject : class;

        void RemoveObject(String cacheKey);
    }
}