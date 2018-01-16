using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using D365WebApiClient.Standard.Common;
using D365WebApiClient.Standard.WebApiQueryOptions;
using D365WebApiClient.Values;
using Newtonsoft.Json.Linq;

namespace D365WebApiClient.Standard.Services.WebApiServices
{
    public partial class ApiClientService
    {
        /// <summary>
        /// PUT更新 推荐Upsert No
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string entityName, Guid guid, Value value)
        {
            var url = BuildGuidUrl(entityName, guid);

            var req = BuildRequest(HttpMethod.Put, url, value);

            var response = await this.SendAsync(req); //204
        }

        /// <summary>
        /// PUT更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string entityName, string alternateKey, string alternateValue, Value value)
        {
            await UpdateAsync(entityName, new[] { new KeyValuePair<string, string>(alternateKey, alternateValue), },
                 value);
        }

        /// <summary>
        /// PUT更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, Value value)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKeyValues);

            var req = BuildRequest(HttpMethod.Put, url, value);

            var response = await this.SendAsync(req); //204
        }

        /// <summary>
        /// Patch部分更新
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entityName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdatePatchAsync(string entityName, Guid guid, Value value)
        {
            await UpsertAsync(entityName, guid, value, true);
        }

        /// <summary>
        /// Patch部分更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdatePatchAsync(string entityName, string alternateKey, string alternateValue,
            Value value)
        {
            await UpsertAsync(entityName, alternateKey, alternateValue, value, true);
        }

        /// <summary>
        /// Patch部分更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdatePatchAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            Value value)
        {
            await UpsertAsync(entityName, alternateKeyValues, value, true);
        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> UpdateAndReadAsync(string entityName, Guid guid, Value value,
            QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName, guid, value, queryOptions, true, false, enumAnnotations);
        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> UpdateAndReadAsync(string entityName, Guid guid, Value value,
            string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName, guid, value, queryOptions, true, false, enumAnnotations);
        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> UpdateAndReadAsync(string entityName,
            string alternateKey, string alternateValue,
            Value value, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName,
                alternateKey,
                alternateValue,
                value,
                queryOptions,
                true,false,
                enumAnnotations);
        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> UpdateAndReadAsync(string entityName,
            string alternateKey, string alternateValue,
             Value value, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName,
                new[] { new KeyValuePair<string, string>(alternateKey, alternateValue) },
                value,
                queryOptions,
                true,false,
                enumAnnotations);
        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> UpdateAndReadAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            Value value, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName, alternateKeyValues, value, queryOptions, true, false, enumAnnotations);


        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> UpdateAndReadAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            Value value, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName, alternateKeyValues, value, queryOptions, true,
                false, enumAnnotations);
        }

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdateEntitySinglePropAsync(string entityName, Guid guid, string attribute, Value value)
        {
            //Create unique guidentifier by appending property name 
            var url = BuildGuidUrl(entityName, guid, null, attribute);

            //Now update just the single property.
            var req = BuildRequest(HttpMethod.Put, url, value);
            var responseMessage = await this.SendAsync(req); //204
        }

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateValue"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="alternateKey"></param>
        /// <returns></returns>
        public async Task UpdateEntitySinglePropAsync(string entityName,
            string alternateKey, string alternateValue,
            string attribute, Value value)
        {
            await UpdateEntitySinglePropAsync(entityName,
                new[] { new KeyValuePair<string, string>(alternateKey, alternateValue) }, attribute, value);
        }

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdateEntitySinglePropAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            string attribute, Value value)
        {
            //Create unique guidentifier by appending property name 
            var url = BuildAlternateKeyUrl(entityName, alternateKeyValues, attribute);

            //Now update just the single property.
            var req = BuildRequest(HttpMethod.Put, url, value);
            var responseMessage = await this.SendAsync(req); //204
        }
    }
}
