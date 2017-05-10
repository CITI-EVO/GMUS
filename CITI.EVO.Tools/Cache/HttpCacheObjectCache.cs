using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace CITI.EVO.Tools.Cache
{
    public class HttpCacheObjectCache : CommonCacheBase
    {
        public override IEnumerable<KeyValuePair<String, Object>> GetObjects()
        {
            var context = HttpContext.Current;
            var cache = context.Cache;

            foreach (DictionaryEntry entry in cache)
            {
                var key = Convert.ToString(entry.Key);
                var value = entry.Value;

                yield return new KeyValuePair<String, Object>(key, value);
            }
        }

        public override TObject GetObject<TObject>(String cacheKey)
        {
            var context = HttpContext.Current;
            var cache = context.Cache;

            var appCache = cache[cacheKey] as TObject;
            return appCache;
        }
        
        public override TObject InitObject<TObject>(String cacheKey, TimeSpan timeout, Func<TObject> initializer)
        {
            var context = HttpContext.Current;

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

            var appCache = context.Cache;
            appCache.Remove(cacheKey);
        }
    }
}