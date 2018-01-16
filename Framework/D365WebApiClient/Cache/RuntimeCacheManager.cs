using System;
using System.Runtime.Caching;

namespace D365WebApiClient.Cache
{
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
            return (T) Cache[key];
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
}