using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;
using System.Web.Caching;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace CITI.EVO.Tools.Cache
{
    public static class CommonObjectCache
    {
		public static IEnumerable<KeyValuePair<String, Object>> GetObjectCaches()
		{
			var context = HttpContext.Current;
			if (context == null || context.Cache == null)
			{
				var cache = MemoryCache.Default;
				foreach (var pair in cache)
				{
					yield return pair;
				}
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

		public static TObject GetObjectCache<TObject>(String cacheKey) where TObject : class
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

		public static void SetObjectCache<TObject>(String cacheKey, TObject @object) where TObject : class
		{
			SetObjectCache(cacheKey, TimeSpan.FromMinutes(15), @object);
		}
		public static void SetObjectCache<TObject>(String cacheKey, TimeSpan timeout, TObject @object) where TObject : class
		{
			InitObjectCache(cacheKey, timeout, () => @object);
		}

		public static TObject InitObjectCache<TObject>(String cacheKey, Func<TObject> initializer) where TObject : class
		{
			return InitObjectCache(cacheKey, TimeSpan.FromMinutes(15), initializer);
		}

		public static TObject InitObjectCache<TObject>(String cacheKey, TimeSpan timeout, Func<TObject> initializer) where TObject : class
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
						memCache.Set(cacheKey, localObj, new CacheItemPolicy(), null);
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

		private static void cache_RemoveCallback(String key, Object value, CacheItemRemovedReason reason)
		{
			var disposable = value as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		public static void RemoveCache(String cacheKey)
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
