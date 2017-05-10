using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;
using System.Web.Caching;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace CITI.EVO.Tools.Cache
{
    public class DefaultObjectCache : CommonCacheBase
    {
        public override IEnumerable<KeyValuePair<String, Object>> GetObjects()
        {
            var context = HttpContext.Current;
            if (context == null || context.Cache == null)
            {
                var cache = MemoryCache.Default;
                foreach (var pair in cache)
                    yield return pair;
            }
            else
            {
                var cache = context.Cache;
                foreach (DictionaryEntry entry in cache)
                {
                    var key = Convert.ToString(entry.Key);
                    var value = entry.Value;

                    yield return new KeyValuePair<String, Object>(key, value);
                }
            }
        }

        public override TObject GetObject<TObject>(String cacheKey)
        {
            var context = HttpContext.Current;
            if (context == null || context.Cache == null)
            {
                var memCache = MemoryCache.Default;
                var localObj = memCache[cacheKey] as TObject;

                return localObj;
            }

            var cache = context.Cache;

            var appCache = cache[cacheKey] as TObject;
            return appCache;
        }

        public override TObject InitObject<TObject>(String cacheKey, TimeSpan timeout, Func<TObject> initializer)
        {
            var context = HttpContext.Current;
            if (context == null || context.Cache == null)
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

            var appCache = context.Cache;
            lock (initializer)
            {
                var appObj = appCache[cacheKey] as TObject;
                if (appObj == null)
                {
                    appObj = initializer();
                    appCache.Insert(cacheKey, appObj, null, System.Web.Caching.Cache.NoAbsoluteExpiration, timeout, CacheItemPriority.Default, cache_RemoveCallback);
                }

                return appObj;
            }
        }

        public override void RemoveObject(String cacheKey)
        {
            var context = HttpContext.Current;
            if (context == null || context.Cache == null)
            {
                var memCache = MemoryCache.Default;
                memCache.Remove(cacheKey);

                return;
            }

            var appCache = context.Cache;
            appCache.Remove(cacheKey);
        }
    }
}