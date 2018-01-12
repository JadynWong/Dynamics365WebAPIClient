using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dynamics365WebApi.WebApiQueryOption;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynamics365WebApi.Service
{
    public partial class WebApiService
    {
        /// <summary>
        /// PUT更新 推荐Upsert No
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string entityName, Guid guid, JObject jObject)
        {
            var url = BuildGuidUrl(entityName, guid);

            var req = BuildRequest(HttpMethod.Put, url, jObject);

            var response = await this.SendAsync(req); //204
        }

        /// <summary>
        /// PUT更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string entityName, string alternateKey, string alternateValue, JObject jObject)
        {
            await UpdateAsync(entityName, new[] { new KeyValuePair<string, string>(alternateKey, alternateValue), },
                 jObject);
        }

        /// <summary>
        /// PUT更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdateAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, JObject jObject)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKeyValues);

            var req = BuildRequest(HttpMethod.Put, url, jObject);

            var response = await this.SendAsync(req); //204
        }

        /// <summary>
        /// Patch部分更新
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entityName"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdatePatchAsync(string entityName, Guid guid, JObject jObject)
        {
            await UpsertAsync(entityName, guid, jObject, true);
        }

        /// <summary>
        /// Patch部分更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdatePatchAsync(string entityName, string alternateKey, string alternateValue,
            JObject jObject)
        {
            await UpsertAsync(entityName, alternateKey, alternateValue, jObject, true);
        }

        /// <summary>
        /// Patch部分更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdatePatchAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            JObject jObject)
        {
            await UpsertAsync(entityName, alternateKeyValues, jObject, true);
        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="jObject"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName, Guid guid, JObject jObject,
            QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName, guid, jObject, queryOptions, true, false, enumAnnotations);
        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="jObject"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName, Guid guid, JObject jObject,
            string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName, guid, jObject, queryOptions, true, false, enumAnnotations);
        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="jObject"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName,
            string alternateKey, string alternateValue,
            JObject jObject, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName,
                alternateKey,
                alternateValue,
                jObject,
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
        /// <param name="jObject"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName,
            string alternateKey, string alternateValue,
             JObject jObject, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName,
                new[] { new KeyValuePair<string, string>(alternateKey, alternateValue) },
                jObject,
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
        /// <param name="jObject"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            JObject jObject, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName, alternateKeyValues, jObject, queryOptions, true, false, enumAnnotations);


        }

        /// <summary>
        /// 更新 并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="jObject"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            JObject jObject, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await UpsertAndReadAsync(entityName, alternateKeyValues, jObject, queryOptions, true,
                false, enumAnnotations);
        }

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdateEntitySinglePropAsync(string entityName, Guid guid, string attribute, JObject jObject)
        {
            //Create unique guidentifier by appending property name 
            var url = BuildGuidUrl(entityName, guid, null, attribute);

            //Now update just the single property.
            var req = BuildRequest(HttpMethod.Put, url, jObject);
            var responseMessage = await this.SendAsync(req); //204
        }

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateValue"></param>
        /// <param name="attribute"></param>
        /// <param name="jObject"></param>
        /// <param name="alternateKey"></param>
        /// <returns></returns>
        public async Task UpdateEntitySinglePropAsync(string entityName,
            string alternateKey, string alternateValue,
            string attribute, JObject jObject)
        {
            await UpdateEntitySinglePropAsync(entityName,
                new[] { new KeyValuePair<string, string>(alternateKey, alternateValue) }, attribute, jObject);
        }

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="attribute"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdateEntitySinglePropAsync(string entityName,
            IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            string attribute, JObject jObject)
        {
            //Create unique guidentifier by appending property name 
            var url = BuildAlternateKeyUrl(entityName, alternateKeyValues, attribute);

            //Now update just the single property.
            var req = BuildRequest(HttpMethod.Put, url, jObject);
            var responseMessage = await this.SendAsync(req); //204
        }
    }
}
