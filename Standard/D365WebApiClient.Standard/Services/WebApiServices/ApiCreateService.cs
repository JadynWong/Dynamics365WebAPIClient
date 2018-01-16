using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using D365WebApiClient.Standard.Common;
using D365WebApiClient.Standard.WebApiQueryOptions;
using D365WebApiClient.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace D365WebApiClient.Standard.Services.WebApiServices
{
    public partial class ApiClientService
    {
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string> CreateAsync(string entityName, Value value)
        {
            var url = BuildUrl(entityName);

            var req = BuildRequest(HttpMethod.Post, url, value);

            var response = await this.SendAsync(req); //204
            var createdguidUrl = response.Headers.GetValues("OData-EntityId").FirstOrDefault();
            return createdguidUrl;
        }

        /// <summary>
        /// 创建并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        public async Task<Value> CreateAndReadAsync(string entityName, Value value, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await CreateAndReadAsync(entityName, value, queryOptions?.ToString(), enumAnnotations);
        }

        /// <summary>
        /// 创建并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<Value> CreateAndReadAsync(string entityName, Value value, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            var url = BuildUrl(entityName, queryOptions);

            var req = BuildRequest(HttpMethod.Post, url, value, enumAnnotations, null, true);

            var response = await this.SendAsync(req); //201

            //Body should contain the requested new-contact information.
            Value deserializeObject = Value.Read(await response.Content.ReadAsStringAsync());
            return deserializeObject;
        }
    }
}
