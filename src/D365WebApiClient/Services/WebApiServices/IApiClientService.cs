using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using D365WebApiClient.Common;
using D365WebApiClient.Values;
using D365WebApiClient.WebApiQueryOptions;

namespace D365WebApiClient.Services.WebApiServices
{
    /// <summary>
    ///  Dynamics365 WebApi服务
    /// </summary>
    public interface IApiClientService
    {
        /// <summary>
        ///     插入/更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="value"></param>
        /// <param name="ifMatch">
        ///     如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加 If-Match 标题到值为 "*" 的请求。
        ///     <para>如果找到实体，您将收到状态 204 的正常响应 (No Content)。 如果未找到实体，您将收到状态 404 的以下响应 (Not Found)。</para>
        /// </param>
        /// <param name="ifNoneMatch">
        ///     如果在插入数据，有某种可能具有相同 id 值的记录已存在于系统中，您可能不希望更新它。 若要防止出现此情况，添加 If-None-Match 标题到值为 "*" 的请求。
        ///     <para>如果未找到实体，您将收到状态 204 的正常响应 (No Content)。 如果找到实体，您将收到状态 412 的以下响应 (Precondition Failed)。</para>
        /// </param>
        /// <returns></returns>
        Task UpsertAsync(string entityName, Guid guid, Value value, bool ifMatch = false,
            bool ifNoneMatch = false);

        /// <summary>
        ///     插入/更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="value"></param>
        /// <param name="ifMatch">
        ///     如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加 If-Match 标题到值为 "*" 的请求。
        ///     <para>如果找到实体，您将收到状态 204 的正常响应 (No Content)。 如果未找到实体，您将收到状态 404 的以下响应 (Not Found)。</para>
        /// </param>
        /// <param name="ifNoneMatch">
        ///     如果在插入数据，有某种可能具有相同 id 值的记录已存在于系统中，您可能不希望更新它。 若要防止出现此情况，添加 If-None-Match 标题到值为 "*" 的请求。
        ///     <para>如果未找到实体，您将收到状态 204 的正常响应 (No Content)。 如果找到实体，您将收到状态 412 的以下响应 (Precondition Failed)。</para>
        /// </param>
        /// <returns></returns>
        Task UpsertAsync(string entityName, string alternateKey, string alternateValue, Value value,
            bool ifMatch = false, bool ifNoneMatch = false);

        /// <summary>
        ///     插入/更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="value"></param>
        /// <param name="ifMatch">
        ///     如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加 If-Match 标题到值为 "*" 的请求。
        ///     <para>如果找到实体，您将收到状态 204 的正常响应 (No Content)。 如果未找到实体，您将收到状态 404 的以下响应 (Not Found)。</para>
        /// </param>
        /// <param name="ifNoneMatch">
        ///     如果在插入数据，有某种可能具有相同 id 值的记录已存在于系统中，您可能不希望更新它。 若要防止出现此情况，添加 If-None-Match 标题到值为 "*" 的请求。
        ///     <para>如果未找到实体，您将收到状态 204 的正常响应 (No Content)。 如果找到实体，您将收到状态 412 的以下响应 (Precondition Failed)。</para>
        /// </param>
        /// <returns></returns>
        Task UpsertAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            Value value,
            bool ifMatch = false, bool ifNoneMatch = false);

        /// <summary>
        ///     插入/更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="ifMatch">
        ///     如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加 If-Match 标题到值为 "*" 的请求。
        ///     <para>如果找到实体，您将收到状态 204 的正常响应 (No Content)。 如果未找到实体，您将收到状态 404 的以下响应 (Not Found)。</para>
        /// </param>
        /// <param name="ifNoneMatch">
        ///     如果在插入数据，有某种可能具有相同 id 值的记录已存在于系统中，您可能不希望更新它。 若要防止出现此情况，添加 If-None-Match 标题到值为 "*" 的请求。
        ///     <para>如果未找到实体，您将收到状态 204 的正常响应 (No Content)。 如果找到实体，您将收到状态 412 的以下响应 (Precondition Failed)。</para>
        /// </param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpsertAndReadAsync(string entityName, Guid guid, Value value,
            QueryOptions queryOptions, bool ifMatch = false, bool ifNoneMatch = false,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        ///     插入/更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="ifMatch">
        ///     如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加 If-Match 标题到值为 "*" 的请求。
        ///     <para>如果找到实体，您将收到状态 204 的正常响应 (No Content)。 如果未找到实体，您将收到状态 404 的以下响应 (Not Found)。</para>
        /// </param>
        /// <param name="ifNoneMatch">
        ///     如果在插入数据，有某种可能具有相同 id 值的记录已存在于系统中，您可能不希望更新它。 若要防止出现此情况，添加 If-None-Match 标题到值为 "*" 的请求。
        ///     <para>如果未找到实体，您将收到状态 204 的正常响应 (No Content)。 如果找到实体，您将收到状态 412 的以下响应 (Precondition Failed)。</para>
        /// </param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpsertAndReadAsync(string entityName, Guid guid, Value value,
            string queryOptions, bool ifMatch = false, bool ifNoneMatch = false,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        ///     插入/更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="ifMatch">
        ///     如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加 If-Match 标题到值为 "*" 的请求。
        ///     <para>如果找到实体，您将收到状态 204 的正常响应 (No Content)。 如果未找到实体，您将收到状态 404 的以下响应 (Not Found)。</para>
        /// </param>
        /// <param name="ifNoneMatch">
        ///     如果在插入数据，有某种可能具有相同 id 值的记录已存在于系统中，您可能不希望更新它。 若要防止出现此情况，添加 If-None-Match 标题到值为 "*" 的请求。
        ///     <para>如果未找到实体，您将收到状态 204 的正常响应 (No Content)。 如果找到实体，您将收到状态 412 的以下响应 (Precondition Failed)。</para>
        /// </param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpsertAndReadAsync(string entityName,
            string alternateKey, string alternateValue,
            Value value, QueryOptions queryOptions,
            bool ifMatch = false, bool ifNoneMatch = false,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        ///     插入/更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="ifMatch">
        ///     如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加 If-Match 标题到值为 "*" 的请求。
        ///     <para>如果找到实体，您将收到状态 204 的正常响应 (No Content)。 如果未找到实体，您将收到状态 404 的以下响应 (Not Found)。</para>
        /// </param>
        /// <param name="ifNoneMatch">
        ///     如果在插入数据，有某种可能具有相同 id 值的记录已存在于系统中，您可能不希望更新它。 若要防止出现此情况，添加 If-None-Match 标题到值为 "*" 的请求。
        ///     <para>如果未找到实体，您将收到状态 204 的正常响应 (No Content)。 如果找到实体，您将收到状态 412 的以下响应 (Precondition Failed)。</para>
        /// </param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpsertAndReadAsync(string entityName,
            string alternateKey, string alternateValue,
            Value value, string queryOptions,
            bool ifMatch = false, bool ifNoneMatch = false,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        ///     插入/更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="ifMatch">
        ///     如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加 If-Match 标题到值为 "*" 的请求。
        ///     <para>如果找到实体，您将收到状态 204 的正常响应 (No Content)。 如果未找到实体，您将收到状态 404 的以下响应 (Not Found)。</para>
        /// </param>
        /// <param name="ifNoneMatch">
        ///     如果在插入数据，有某种可能具有相同 id 值的记录已存在于系统中，您可能不希望更新它。 若要防止出现此情况，添加 If-None-Match 标题到值为 "*" 的请求。
        ///     <para>如果未找到实体，您将收到状态 204 的正常响应 (No Content)。 如果找到实体，您将收到状态 412 的以下响应 (Precondition Failed)。</para>
        /// </param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpsertAndReadAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            Value value, QueryOptions queryOptions,
            bool ifMatch = false, bool ifNoneMatch = false,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        ///     插入/更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="ifMatch">
        ///     如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加 If-Match 标题到值为 "*" 的请求。 如果更新数据，并且有某种可能会有意删除实体，则您不想再创建实体。 若要防止出现此情况，添加
        ///     If-Match 标题到值为 "*" 的请求。
        ///     <para>如果找到实体，您将收到状态 204 的正常响应 (No Content)。 如果未找到实体，您将收到状态 404 的以下响应 (Not Found)。</para>
        /// </param>
        /// <param name="ifNoneMatch">
        ///     如果在插入数据，有某种可能具有相同 id 值的记录已存在于系统中，您可能不希望更新它。 若要防止出现此情况，添加 If-None-Match 标题到值为 "*" 的请求。
        ///     <para>如果未找到实体，您将收到状态 204 的正常响应 (No Content)。 如果找到实体，您将收到状态 412 的以下响应 (Precondition Failed)。</para>
        /// </param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpsertAndReadAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            Value value, string queryOptions,
            bool ifMatch = false, bool ifNoneMatch = false,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// PUT全量更新 推荐Upsert No
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdateAsync(string entityName, Guid guid, Value value);

        /// <summary>
        /// PUT全量更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdateAsync(string entityName, string alternateKey, string alternateValue, Value value);

        /// <summary>
        /// PUT全量更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdateAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, Value value);

        /// <summary>
        /// Patch增量更新
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entityName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdatePatchAsync(string entityName, Guid guid, Value value);

        /// <summary>
        /// Patch增量更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdatePatchAsync(string entityName, string alternateKey, string alternateValue,
            Value value);

        /// <summary>
        /// Patch增量更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdatePatchAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            Value value);

        /// <summary>
        /// 更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpdateAndReadAsync(string entityName, Guid guid, Value value,
            QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpdateAndReadAsync(string entityName, Guid guid, Value value,
            string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpdateAndReadAsync(string entityName,
            string alternateKey, string alternateValue,
            Value value, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpdateAndReadAsync(string entityName,
            string alternateKey, string alternateValue,
            Value value, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpdateAndReadAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            Value value, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 更新 并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> UpdateAndReadAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            Value value, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdateEntitySinglePropAsync(string entityName, Guid guid, string attribute, Value value);

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateValue"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="alternateKey"></param>
        /// <returns></returns>
        Task UpdateEntitySinglePropAsync(string entityName,
            string alternateKey, string alternateValue,
            string attribute, Value value);

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdateEntitySinglePropAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            string attribute, Value value);

        /// <summary>
        /// WhoImI
        /// </summary>
        /// <returns></returns>
        Task<Value> WhoImIAsync();

        /// <summary>
        /// WebApiVersion
        /// </summary>
        /// <returns></returns>
        Task<Version> WebApiVersion();

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, Guid guid,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, Guid guid, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, Guid guid, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, string alternateKey, string alternateValue,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, string alternateKey, string alternateValue,
            QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, string alternateKey, string alternateValue,
            string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues
            , EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName,
            EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <returns></returns>
        Task<Value> ReadAsync(string entityName, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null);

        /// <summary>
        /// 查询指定记录单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> ReadEntitySingleProp(string entityName, Guid guid, string attribute,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task DeleteAsync(string entityName, Guid guid);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <returns></returns>
        Task DeleteAsync(string entityName, string alternateKey, string alternateValue);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <returns></returns>
        Task DeleteAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues);

        /// <summary>
        /// 删除单个属性
        /// </summary>
        /// <para>这无法用于单一值导航属性解除两个实体的关联</para>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        Task DeleteEntitySinglePropAsync(string entityName, Guid guid, string attribute);

        /// <summary>
        /// 删除单个属性
        /// </summary>
        /// <para>这无法用于单一值导航属性解除两个实体的关联</para>
        /// <param name="entityName"></param>
        /// <param name="alternateValue"></param>
        /// <param name="attribute"></param>
        /// <param name="alternateKey"></param>
        /// <returns></returns>
        Task DeleteEntitySinglePropAsync(string entityName, string alternateKey, string alternateValue, string attribute);

        /// <summary>
        /// 删除单个属性
        /// </summary>
        /// <para>这无法用于单一值导航属性解除两个实体的关联</para>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        Task DeleteEntitySinglePropAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, string attribute);

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<string> CreateAsync(string entityName, Value value);

        /// <summary>
        /// 创建并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        Task<Value> CreateAndReadAsync(string entityName, Value value, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 创建并查询 仅v8.2 以上
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        Task<Value> CreateAndReadAsync(string entityName, Value value, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None);

        /// <summary>
        /// 执行固定函数
        /// </summary>
        /// <returns></returns>
        Task<Value> ExecuteAsync(HttpMethod httpMethod, string url, Value value = null);

        /// <summary>
        /// 执行自定义请求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage req);
    }
}
