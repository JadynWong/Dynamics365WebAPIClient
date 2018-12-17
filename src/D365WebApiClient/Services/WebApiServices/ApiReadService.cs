using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D365WebApiClient.Common;
using D365WebApiClient.WebApiQueryOptions;
using D365WebApiClient.WebApiQueryOptions.Options;
using D365WebApiClient.Values;

namespace D365WebApiClient.Services.WebApiServices
{
    public partial class ApiClientService
    {
        /// <inheritdoc />
        /// <summary>
        /// WhoImI
        /// </summary>
        /// <returns></returns>
        public async Task<Value> WhoImIAsync()
        {
            var url = $"WhoAmI";

            var req = BuildGetRequest(url);
            var response = await this.ExecuteAsync(req);//200
            var value = Value.Read(await response.Content.ReadAsStringAsync());
            return value;
        }

        /// <inheritdoc />
        /// <summary>
        /// WebApiVersion
        /// </summary>
        /// <returns></returns>
        public async Task<Version> WebApiVersion()
        {
            var url = $"RetrieveVersion";

            var req = BuildGetRequest(url);

            var response =
                await this.ExecuteAsync(req); //200

            Value retrievedVersion = Value.Read(
                await response.Content.ReadAsStringAsync());
            //Capture the actual version available in this organization
            return Version.Parse((string)retrievedVersion.GetValue("Version"));
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, Guid guid, 
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(entityName, guid, string.Empty, enumAnnotations);
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, Guid guid, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(entityName, guid, queryOptions?.ToString(), enumAnnotations);
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, Guid guid, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            var url = BuildGuidUrl(entityName, guid, queryOptions);

            var req = BuildGetRequest(url, enumAnnotations);

            var response = await this.ExecuteAsync(req); //200
            var value = Value.Read(await response.Content.ReadAsStringAsync());
            return value;
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, string alternateKey, string alternateValue,
             EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(
                entityName,
                new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(alternateKey, alternateValue) },
                String.Empty,
                enumAnnotations);

        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, string alternateKey, string alternateValue,
            QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(
                entityName,
                new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(alternateKey, alternateValue) },
                queryOptions?.ToString(),
                enumAnnotations);

        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, string alternateKey, string alternateValue,
            string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(
                entityName,
                new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(alternateKey, alternateValue) },
                queryOptions,
                enumAnnotations);

        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues
            , EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(entityName,
                alternateKeyValues,
                string.Empty,
                enumAnnotations);
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(entityName,
                alternateKeyValues,
                queryOptions?.ToString(),
                enumAnnotations);
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKeyValues, queryOptions);

            var req = BuildGetRequest(url, enumAnnotations);

            var response = await this.ExecuteAsync(req); //200
            var value = Value.Read(await response.Content.ReadAsStringAsync());
            return value;
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName,
            EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null)
        {
            return await ReadAsync(entityName, string.Empty, enumAnnotations, maxPageSize);
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null)
        {
            if (maxPageSize.HasValue)
            {
                if (queryOptions.Any(x => x.GetType() == typeof(QueryCount)))
                {
                    throw new ArgumentException("您不应将 $top 与 $count 一起使用",nameof(maxPageSize));
                }
            }
            return await ReadAsync(entityName, queryOptions?.ToString(), enumAnnotations, maxPageSize);
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <returns></returns>
        public async Task<Value> ReadAsync(string entityName, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null)
        {
            var url = BuildUrl(entityName, queryOptions);

            var req = BuildGetRequest(url, enumAnnotations, maxPageSize);

            var response = await this.ExecuteAsync(req); //200
            var value = Value.Read(await response.Content.ReadAsStringAsync());
            return value;
        }

        /// <inheritdoc />
        /// <summary>
        /// 查询指定记录单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> ReadEntitySingleProp(string entityName, Guid guid, string attribute,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            //Now retrieve just the single property.
            var url = BuildGuidUrl(entityName, guid, null, attribute);

            var req = BuildGetRequest(url, enumAnnotations);

            var response = await this.ExecuteAsync(req);

            Value prop = Value.Read(await response.Content.ReadAsStringAsync()); //200
            return prop;
        }
    }
}
