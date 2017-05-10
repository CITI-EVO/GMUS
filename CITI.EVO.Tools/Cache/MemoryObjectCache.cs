using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace CITI.EVO.Tools.Cache
{
    public class MemoryObjectCache : CommonCacheBase
    {
        public override IEnumerable<KeyValuePair<String, Object>> GetObjects()
        {
            var cache = MemoryCache.Default;
            foreach (var pair in cache)
                yield return pair;
        }

        public override TObject GetObject<TObject>(String cacheKey)
        {
            var memCache = MemoryCache.Default;
            var localObj = memCache[cacheKey] as TObject;

            return localObj;
        }

        public override TObject InitObject<TObject>(String cacheKey, TimeSpan timeout, Func<TObject> initializer)
        {
            var memCache = MemoryCache.Default;

            lock (initializer)
            {
                var localObj = memCache[cacheKey] as TObject;
                if (localObj == null)
                {
                    localObj = initializer();

                    var cacheItemPolicy = new CacheItemPolicy
                    {
                        SlidingExpiration = timeout,
                        RemovedCallback = cache_RemovedCallback
                    };

                    memCache.Set(cacheKey, localObj, cacheItemPolicy, null);
                }

                return localObj;
            }
        }

        public override void RemoveObject(String cacheKey)
        {
            var memCache = MemoryCache.Default;
            memCache.Remove(cacheKey);
        }

    }
}