using System;
using System.Collections;
using System.Runtime.Caching;
using System.Web;
using System.Web.Caching;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace CITI.EVO.Rpc.Common
{
	public class CollectionsCache
	{
		public static IEnumerator GetCollection(Guid collectionID)
		{
			var key = Convert.ToString(collectionID);
			return GetObjectCache(key) as IEnumerator;
		}

		public static Guid InsertCollection(IEnumerable collection)
		{
			var enumerator = collection.GetEnumerator();
			return InsertCollection(enumerator);
		}

		public static Guid InsertCollection(IEnumerator collection)
		{
			var collectionID = Guid.NewGuid();

			var key = Convert.ToString(collectionID);
			SetObjectCache(key, TimeSpan.MaxValue, collection);

			return collectionID;
		}

		public static void DeleteCollection(Guid collectionID)
		{
			var key = Convert.ToString(collectionID);
			DeleteObjectCache(key);
		}

		public static Object GetObjectCache(String cacheKey)
		{
			var context = HttpContext.Current;
			if (context == null || context.Cache == null)
			{
				var memCache = MemoryCache.Default;
				return memCache[cacheKey];
			}

			var appCache = context.Cache;
			return appCache[cacheKey];
		}
		public static void DeleteObjectCache(String cacheKey)
		{
			var context = HttpContext.Current;
			if (context == null || context.Cache == null)
			{
				var memCache = MemoryCache.Default;
				memCache.Remove(cacheKey);
			}

			var appCache = context.Cache;
			appCache.Remove(cacheKey);
		}

		public static void SetObjectCache(String cacheKey, TimeSpan timeout, Object @object)
		{
			var context = HttpContext.Current;
			if (context == null || context.Cache == null)
			{
				var memCache = MemoryCache.Default;
				memCache.Set(cacheKey, @object, new CacheItemPolicy(), null);
			}

			var appCache = context.Cache;
			appCache.Insert(cacheKey, @object, null, Cache.NoAbsoluteExpiration, timeout, CacheItemPriority.Default, cache_RemoveCallback);
		}

		private static void cache_RemoveCallback(String key, Object value, CacheItemRemovedReason reason)
		{
			var disposable = value as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}
