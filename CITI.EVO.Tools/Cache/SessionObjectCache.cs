using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace CITI.EVO.Tools.Cache
{
    public class SessionObjectCache : CommonCacheBase
    {
        public override IEnumerable<KeyValuePair<String, Object>> GetObjects()
        {
            var context = HttpContext.Current;
            var cache = context.Session;

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
            var cache = context.Session;

            var appCache = cache[cacheKey] as TObject;
            return appCache;
        }

        public override TObject InitObject<TObject>(String cacheKey, TimeSpan timeout, Func<TObject> initializer)
        {
            var context = HttpContext.Current;

            var appCache = context.Session;
            lock (initializer)
            {
                var appObj = appCache[cacheKey] as TObject;
                if (appObj == null)
                {
                    appObj = initializer();
                    appCache[cacheKey] = appObj;
                }

                return appObj;
            }
        }

        public override void RemoveObject(String cacheKey)
        {
            var context = HttpContext.Current;

            var appCache = context.Session;
            appCache.Remove(cacheKey);
        }
    }
}