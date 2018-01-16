using System;

namespace D365WebApiClient.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="slidingExpiration">有效期</param>
        /// <param name="t">缓存对象</param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string key, TimeSpan slidingExpiration, T t);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="absoluteExpiration">绝对失效时间</param>
        /// <param name="t">缓存对象</param>
        /// <typeparam name="T">类型</typeparam>
        void Set<T>(string key, DateTimeOffset absoluteExpiration, T t);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>缓存对象</returns>
        T Get<T>(string key);

        /// <summary>
        /// 是否设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns><see cref="Int32"/></returns>
        bool IsSet(string key);
    }
}