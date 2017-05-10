using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Runtime.Caching;
using System.Web.Caching;

namespace CITI.EVO.Tools.Cache
{
    public abstract class CommonCacheBase : ICommonObjectCache
    {
        private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(15D);

        public abstract IEnumerable<KeyValuePair<String, Object>> GetObjects();

        public abstract TObject GetObject<TObject>(String cacheKey) where TObject : class;

        public void SetObject<TObject>(String cacheKey, TObject @object) where TObject : class
        {
            var timeout = GetDefaultTimeout();
            SetObject(cacheKey, timeout, @object);
        }
        public void SetObject<TObject>(String cacheKey, TimeSpan timeout, TObject @object) where TObject : class
        {
            InitObject(cacheKey, timeout, () => @object);
        }

        public TObject InitObject<TObject>(String cacheKey, Func<TObject> initializer) where TObject : class
        {
            var timeout = GetDefaultTimeout();
            return InitObject(cacheKey, timeout, initializer);
        }

        public abstract TObject InitObject<TObject>(String cacheKey, TimeSpan timeout, Func<TObject> initializer) where TObject : class;

        public abstract void RemoveObject(String cacheKey);

        protected TimeSpan GetDefaultTimeout()
        {
            var type = GetType();

            var key = $"{type.Name}.DefaultTimeout";
            var value = ConfigurationManager.AppSettings[key];

            if (!String.IsNullOrEmpty(value))
            {
                double d;
                if (double.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out d))
                    return TimeSpan.FromMinutes(d);
            }

            return _defaultTimeout;
        }

        protected void cache_RemovedCallback(CacheEntryRemovedArguments arguments)
        {
            var disposable = arguments.CacheItem.Value as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }

        protected void cache_RemoveCallback(String key, Object value, CacheItemRemovedReason reason)
        {
            var disposable = value as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
    }
}