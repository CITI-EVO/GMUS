using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace CITI.EVO.Tools.Cache
{
    public static class CommonObjectCache
    {
        private static readonly Object _syncLock = new Object();

        private static CommonCacheBase _defaultObjectCache;
        private static CommonCacheBase _httpCacheObjectCache;
        private static CommonCacheBase _memoryObjectCache;
        private static CommonCacheBase _requestObjectCache;
        private static CommonCacheBase _sessionObjectCache;

        public static IEnumerable<KeyValuePair<String, Object>> GetObjects()
        {
            return GetObjects(CommonCacheStore.Default);
        }
        public static IEnumerable<KeyValuePair<String, Object>> GetObjects(CommonCacheStore store)
        {
            var cache = GetCacheProvider(store);
            return cache.GetObjects();
        }

        public static TObject GetObject<TObject>(String cacheKey) where TObject : class
        {
            return GetObject<TObject>(cacheKey, CommonCacheStore.Default);
        }
        public static TObject GetObject<TObject>(String cacheKey, CommonCacheStore store) where TObject : class
        {
            var cache = GetCacheProvider(store);
            return cache.GetObject<TObject>(cacheKey);
        }

        public static void SetObject<TObject>(String cacheKey, TObject @object) where TObject : class
        {
            SetObject(cacheKey, CommonCacheStore.Default, @object);
        }
        public static void SetObject<TObject>(String cacheKey, CommonCacheStore store, TObject @object) where TObject : class
        {
            SetObject(cacheKey, store, TimeSpan.FromMinutes(15), @object);
        }
        public static void SetObject<TObject>(String cacheKey, TimeSpan timeout, TObject @object) where TObject : class
        {
            SetObject(cacheKey, CommonCacheStore.Default, timeout, @object);
        }
        public static void SetObject<TObject>(String cacheKey, CommonCacheStore store, TimeSpan timeout, TObject @object) where TObject : class
        {
            InitObject(cacheKey, store, timeout, () => @object);
        }

        public static TObject InitObject<TObject>(String cacheKey, Func<TObject> initializer) where TObject : class
        {
            return InitObject(cacheKey, CommonCacheStore.Default, TimeSpan.FromMinutes(15), initializer);
        }
        public static TObject InitObject<TObject>(String cacheKey, CommonCacheStore store, Func<TObject> initializer) where TObject : class
        {
            return InitObject(cacheKey, store, TimeSpan.FromMinutes(15), initializer);
        }

        public static TObject InitObject<TObject>(String cacheKey, TimeSpan timeout, Func<TObject> initializer) where TObject : class
        {
            return InitObject(cacheKey, CommonCacheStore.Default, timeout, initializer);
        }
        public static TObject InitObject<TObject>(String cacheKey, CommonCacheStore store, TimeSpan timeout, Func<TObject> initializer) where TObject : class
        {
            var cache = GetCacheProvider(store);
            return cache.InitObject(cacheKey, timeout, initializer);
        }

        public static void RemoveObject(String cacheKey)
        {
            RemoveObject(cacheKey, CommonCacheStore.Default);
        }
        public static void RemoveObject(String cacheKey, CommonCacheStore store)
        {
            var cache = GetCacheProvider(store);
            cache.RemoveObject(cacheKey);
        }

        private static CommonCacheBase GetCacheProvider(CommonCacheStore store)
        {
            lock (_syncLock)
            {
                switch (store)
                {
                    case CommonCacheStore.HttpCache:
                        {
                            if (_httpCacheObjectCache == null)
                                _httpCacheObjectCache = new HttpCacheObjectCache();

                            return _httpCacheObjectCache;
                        }
                    case CommonCacheStore.Memory:
                        {
                            if (_memoryObjectCache == null)
                                _memoryObjectCache = new MemoryObjectCache();

                            return _memoryObjectCache;
                        }
                    case CommonCacheStore.Request:
                        {
                            if (_requestObjectCache == null)
                                _requestObjectCache = new RequestObjectCache();

                            return _requestObjectCache;
                        }
                    case CommonCacheStore.Session:
                        {
                            if (_sessionObjectCache == null)
                                _sessionObjectCache = new SessionObjectCache();

                            return _sessionObjectCache;
                        }
                    default:
                        {
                            if (_defaultObjectCache == null)
                                _defaultObjectCache = new DefaultObjectCache();

                            return _defaultObjectCache;
                        }
                }
            }
        }
    }
}
