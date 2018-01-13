using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using D365WebApiClient.WebApiQueryOption;
using Dynamics365WebApi.Service;
using Dynamics365WebApi.WebApiQueryOption;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace D365WebApiClient.Service.WebApiServices
{
    public partial class WebApiService
    {
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task<string> CreateAsync(string entityName, JObject jObject)
        {
            var url = BuildUrl(entityName);

            var req = BuildRequest(HttpMethod.Post, url, jObject);

            var response = await this.SendAsync(req); //204
            var createdguidUrl = response.Headers.GetValues("OData-EntityId").FirstOrDefault();
            return createdguidUrl;
        }

        /// <summary>
        /// 创建并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="jObject"></param>
        /// <param name="enumAnnotations"></param>
        public async Task<JObject> CreateAndReadAsync(string entityName, JObject jObject, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await CreateAndReadAsync(entityName, jObject, queryOptions?.ToString(), enumAnnotations);
        }

        /// <summary>
        /// 创建并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="jObject"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> CreateAndReadAsync(string entityName, JObject jObject, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            var url = BuildUrl(entityName, queryOptions);

            var req = BuildRequest(HttpMethod.Post, url, jObject, enumAnnotations, null, true);

            var response = await this.SendAsync(req); //201

            //Body should contain the requested new-contact information.
            JObject deserializeObject = JsonConvert.DeserializeObject<JObject>(
                await response.Content.ReadAsStringAsync());
            return deserializeObject;
        }
    }
}
