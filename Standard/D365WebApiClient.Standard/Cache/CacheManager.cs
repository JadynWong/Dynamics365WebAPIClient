using System;
using System.Text;

#if NETSTANDARD2_0

using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#elif NET45
using System.Runtime.Caching;
#endif

namespace D365WebApiClient.Cache
{
#if NETSTANDARD2_0

    public class DistributedCacheManager : ICacheManager
    {
        private readonly IDistributedCache _distributedCache;

        private static JsonSerializerSettings SerializerSettings { get; } = SerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            MissingMemberHandling = MissingMemberHandling.Ignore,
            MaxDepth = new int?(32),
            TypeNameHandling = TypeNameHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public DistributedCacheManager(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        #region Utilities

        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item, SerializerSettings);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString, SerializerSettings);
        }

        #endregion Utilities

        public T Get<T>(string key)
        {
            var bytes = _distributedCache.Get(key);
            return Deserialize<T>(bytes);
        }

        public bool IsSet(string key)
        {
            return _distributedCache.Get(key) != null;
        }

        public void Set<T>(string key, TimeSpan slidingExpiration, T t)
        {
            _distributedCache.Set(key, Serialize(t), new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.Now + slidingExpiration });
        }

        public void Set<T>(string key, DateTimeOffset absoluteExpiration, T t)
        {
            _distributedCache.Set(key, Serialize(t), new DistributedCacheEntryOptions() { AbsoluteExpiration = absoluteExpiration });
        }
    }

#elif NET45
    /// <summary>
    /// 内置缓存
    /// </summary>
    public class RuntimeCacheManager : ICacheManager
    {
        /// <summary>
        /// Cache object
        /// </summary>
        protected ObjectCache Cache => MemoryCache.Default;

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="slidingExpiration">有效期</param>
        /// <param name="t">缓存对象</param>
        /// <typeparam name="T"></typeparam>
        public void Set<T>(string key, TimeSpan slidingExpiration, T t)
        {
            var policy = new CacheItemPolicy
            {
                //AbsoluteExpiration = DateTimeOffset.Now,
                SlidingExpiration = slidingExpiration
            };
            Cache.Add(new CacheItem(key, t), policy);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="absoluteExpiration">绝对失效时间</param>
        /// <param name="t">缓存对象</param>
        /// <typeparam name="T">类型</typeparam>
        public void Set<T>(string key, DateTimeOffset absoluteExpiration, T t)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration
            };
            Cache.Add(new CacheItem(key, t), policy);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>缓存对象</returns>
        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 是否设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns><see cref="Int32"/></returns>
        public bool IsSet(string key)
        {
            return Cache.Contains(key);
        }
    }

#endif
}