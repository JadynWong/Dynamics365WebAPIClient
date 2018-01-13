using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D365WebApiClient.WebApiQueryOption;
using D365WebApiClient.WebApiQueryOption.Options;
using Dynamics365WebApi.Service;
using Dynamics365WebApi.WebApiQueryOption;
using Dynamics365WebApi.WebApiQueryOption.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace D365WebApiClient.Service.WebApiServices
{
    public partial class WebApiService
    {
        /// <summary>
        /// WhoImI
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> WhoImIAsync()
        {
            var url = $"WhoAmI";

            var req = BuildGetRequest(url);
            var response = await this.SendAsync(req);//200
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
        }

        /// <summary>
        /// WebApiVersion
        /// </summary>
        /// <returns></returns>
        public async Task<Version> WebApiVersion()
        {
            var url = $"RetrieveVersion";

            var req = BuildGetRequest(url);

            var response =
                await this.SendAsync(req); //200

            JObject retrievedVersion = JsonConvert.DeserializeObject<JObject>(
                await response.Content.ReadAsStringAsync());
            //Capture the actual version available in this organization
            return Version.Parse((string)retrievedVersion.GetValue("Version"));
        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, Guid guid, 
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(entityName, guid, string.Empty, enumAnnotations);
        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, Guid guid, QueryOptions queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(entityName, guid, queryOptions?.ToString(), enumAnnotations);
        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, Guid guid, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            var url = BuildGuidUrl(entityName, guid, queryOptions);

            var req = BuildGetRequest(url, enumAnnotations);

            var response = await this.SendAsync(req); //200
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string alternateKey, string alternateValue,
             EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(
                entityName,
                new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(alternateKey, alternateValue) },
                String.Empty,
                enumAnnotations);

        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string alternateKey, string alternateValue,
            QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(
                entityName,
                new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(alternateKey, alternateValue) },
                queryOptions?.ToString(),
                enumAnnotations);

        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string alternateKey, string alternateValue,
            string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(
                entityName,
                new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(alternateKey, alternateValue) },
                queryOptions,
                enumAnnotations);

        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues
            , EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(entityName,
                alternateKeyValues,
                string.Empty,
                enumAnnotations);
        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            return await ReadAsync(entityName,
                alternateKeyValues,
                queryOptions?.ToString(),
                enumAnnotations);
        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKeyValues, queryOptions);

            var req = BuildGetRequest(url, enumAnnotations);

            var response = await this.SendAsync(req); //200
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName,
            EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null)
        {
            return await ReadAsync(entityName, string.Empty, enumAnnotations, maxPageSize);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, QueryOptions queryOptions,
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

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string queryOptions,
            EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null)
        {
            var url = BuildUrl(entityName, queryOptions);

            var req = BuildGetRequest(url, enumAnnotations, maxPageSize);

            var response = await this.SendAsync(req); //200
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
        }

        ///// <summary>
        ///// 查询单个属性
        ///// </summary>
        ///// <param name="entityName"></param>
        ///// <param name="attribute"></param>
        ///// <param name="enumAnnotations"></param>
        ///// <param name="maxPageSize"></param>
        ///// <returns></returns>
        //public async Task<JObject> ReadEntitySingleProp(string entityName, string attribute, 
        //    EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null)
        //{
        //    return await ReadEntitySingleProp(entityName, attribute, string.Empty, enumAnnotations, maxPageSize);
        //}

        ///// <summary>
        ///// 查询单个属性
        ///// </summary>
        ///// <param name="entityName"></param>
        ///// <param name="attribute"></param>
        ///// <param name="queryOptions"></param>
        ///// <param name="enumAnnotations"></param>
        ///// <param name="maxPageSize"></param>
        ///// <returns></returns>
        //public async Task<JObject> ReadEntitySingleProp(string entityName, string attribute, QueryOptions queryOptions,
        //    EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null)
        //{
        //    if (maxPageSize.HasValue)
        //    {
        //        if (queryOptions.Any(x => x.GetType() == typeof(QueryCount)))
        //        {
        //            throw new ArgumentException("您不应将 $top 与 $count 一起使用",nameof(maxPageSize));
        //        }
        //    }
        //    return await ReadEntitySingleProp(entityName, attribute, queryOptions?.ToString(), enumAnnotations, maxPageSize);
        //}

        ///// <summary>
        ///// 查询单个属性
        ///// </summary>
        ///// <param name="entityName"></param>
        ///// <param name="attribute"></param>
        ///// <param name="queryOptions"></param>
        ///// <param name="enumAnnotations"></param>
        ///// <param name="maxPageSize"></param>
        ///// <returns></returns>
        //public async Task<JObject> ReadEntitySingleProp(string entityName, string attribute, string queryOptions,
        //    EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null)
        //{
        //    //Now retrieve just the single property.
        //    var url = BuildUrl(entityName, queryOptions, attribute);


        //    var req = BuildGetRequest(url, enumAnnotations, maxPageSize);

        //    var response = await this.SendAsync(req);
        //    JObject prop = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync()); //200
        //    return prop;
        //}

        /// <summary>
        /// 查询指定记录单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <param name="enumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadEntitySingleProp(string entityName, Guid guid, string attribute,
            EnumAnnotations enumAnnotations = EnumAnnotations.None)
        {
            //Now retrieve just the single property.
            var url = BuildGuidUrl(entityName, guid, null, attribute);

            var req = BuildGetRequest(url, enumAnnotations);

            var response = await this.SendAsync(req);

            JObject prop = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync()); //200
            return prop;
        }
    }
}
