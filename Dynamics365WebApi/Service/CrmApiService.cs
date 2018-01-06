using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dynamics365WebApi.Common;
using Dynamics365WebApi.Token;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynamics365WebApi.Service
{

    public class CrmApiService
    {
        private static readonly HttpClient RestClient;

        public static readonly CrmConfig CrmConfig;

        static CrmApiService()
        {

            CrmConfig = new CrmConfig();
            if (CrmConfig.IsIfd)
            {
                var httpMessageHandler = new OAuthMessageHandler(CrmConfig.AdfsUri, CrmConfig.Resource, CrmConfig.ClientId, CrmConfig.RedirectUri,
                    $"{CrmConfig.DomainName}\\{CrmConfig.UserName}", CrmConfig.Password, new HttpClientHandler()); ;
                RestClient = GetNewHttpClient(httpMessageHandler, CrmConfig.ApiUrl);
            }
            else
            {

                CrmConfig = new CrmConfig();
                var httpMessageHandler = new HttpClientHandler()
                {
                    Credentials = new NetworkCredential(CrmConfig.UserName, CrmConfig.Password, CrmConfig.DomainName)
                };
                RestClient = GetNewHttpClient(httpMessageHandler, CrmConfig.ApiUrl);
            }


        }


        private static HttpClient GetNewHttpClient(HttpMessageHandler httpMessageHandler, string webApiBaseAddress)
        {
            var httpClient = new HttpClient(httpMessageHandler)
            {
                BaseAddress = new Uri(webApiBaseAddress),
                Timeout = new TimeSpan(0, 2, 0),
                DefaultRequestHeaders =
                {
                    {"OData-MaxVersion", "4.0"},
                    {"OData-Version", "4.0"},
                    {"Accept","application/json"}
                }
            };
            return httpClient;
        }

        /// <summary>
        /// WhoImI
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> WhoImIAsync()
        {
            var url = $"WhoAmI";

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await this.SendAsync(req);
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;//200
        }

        /// <summary>
        /// WebApiVersion
        /// </summary>
        /// <returns></returns>
        public async Task<Version> WebApiVersion()
        {
            var url = $"RetrieveVersion";

            HttpRequestMessage retrieveVersionRequest =
                new HttpRequestMessage(HttpMethod.Get, url);

            var retrieveVersionResponse =
                await this.SendAsync(retrieveVersionRequest);//200
            JObject retrievedVersion = JsonConvert.DeserializeObject<JObject>(
                await retrieveVersionResponse.Content.ReadAsStringAsync());
            //Capture the actual version available in this organization
            return Version.Parse((string)retrievedVersion.GetValue("Version"));
        }

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task<string> CreateAsync(string entityName, JObject jObject)
        {
            var url = BuildUrl(entityName);

            var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req);//204
            var createdguidUrl = response.Headers.GetValues("OData-EntityId").FirstOrDefault();
            return createdguidUrl;
        }

        /// <summary>
        /// 创建并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="jObject"></param>
        /// <param name="crmApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> CreateAndReadAsync(string entityName, string queryOptions, JObject jObject, CrmApiEnumAnnotations crmApiEnumAnnotations = CrmApiEnumAnnotations.None)
        {
            var url = BuildUrl(entityName, queryOptions);

            var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Headers = { { "Prefer", "return=representation" } },
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            IncludeAnnotations(req, crmApiEnumAnnotations);
            var response = await this.SendAsync(req);//201
            //Body should contain the requested new-contact information.
            JObject deserializeObject = JsonConvert.DeserializeObject<JObject>(
                await response.Content.ReadAsStringAsync());
            return deserializeObject;
        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="crmApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string guid, string queryOptions, CrmApiEnumAnnotations crmApiEnumAnnotations = CrmApiEnumAnnotations.None)
        {
            var url = BuildGuidUrl(entityName, guid, queryOptions);

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            IncludeAnnotations(req, crmApiEnumAnnotations);
            var response = await this.SendAsync(req);//200
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
        }

        /// <summary>
        /// 查询指定记录
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="crmApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string alternateKey, string alternateValue, string queryOptions, CrmApiEnumAnnotations crmApiEnumAnnotations = CrmApiEnumAnnotations.None)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKey, alternateValue, queryOptions);

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await this.SendAsync(req);//200
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="crmApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string queryOptions, CrmApiEnumAnnotations crmApiEnumAnnotations = CrmApiEnumAnnotations.None)
        {
            var url = BuildUrl(entityName, queryOptions);

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            IncludeAnnotations(req, crmApiEnumAnnotations);
            var response = await this.SendAsync(req);//200
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
        }

        /// <summary>
        /// 查询单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <param name="crmApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadEntitySingleProp(string entityName, string guid, string attribute, CrmApiEnumAnnotations crmApiEnumAnnotations = CrmApiEnumAnnotations.None)
        {
            var url = BuildGuidUrl(entityName, guid, null, attribute);
            //Now retrieve just the single property.
            JObject prop;
            var req = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            IncludeAnnotations(req, crmApiEnumAnnotations);
            HttpResponseMessage responseMessage =
                await this.SendAsync(req);
            prop = JsonConvert.DeserializeObject<JObject>(
                await responseMessage.Content.ReadAsStringAsync());//200
            return prop;
        }

        /// <summary>
        /// PUT更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdatePutAsync(string entityName, string guid, JObject jObject)
        {
            var url = BuildGuidUrl(entityName, guid);

            var req = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req);//204
        }

        /// <summary>
        /// PUT更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdatePutAsync(string entityName, string alternateKey, string alternateValue, JObject jObject)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKey, alternateValue);

            var req = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req);//204
        }

        /// <summary>
        /// Patch部分更新
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="entityName"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdatePatchAsync(string entityName, string guid, JObject jObject)
        {
            var url = BuildGuidUrl(entityName, guid);

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req); //204
        }

        /// <summary>
        /// Patch部分更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task UpdatePatchAsync(string entityName, string alternateKey, string alternateValue, JObject jObject)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKey, alternateValue);

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req); //204
        }

        /// <summary>
        /// 更新并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="jObject"></param>
        /// <param name="crmApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName, string guid, string queryOptions, JObject jObject, CrmApiEnumAnnotations crmApiEnumAnnotations = CrmApiEnumAnnotations.None)
        {
            var url = BuildGuidUrl(entityName, guid, queryOptions);

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Headers = { { "Prefer", "return=representation" } },
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            IncludeAnnotations(req, crmApiEnumAnnotations);
            var response = await this.SendAsync(req); //200
            //Body should contain the requested new-contact information.
            JObject deserializeObject = JsonConvert.DeserializeObject<JObject>(
                await response.Content.ReadAsStringAsync());
            return deserializeObject;
        }

        /// <summary>
        /// 更新并查询 仅v8.2
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName, string alternateKey, string alternateValue, string queryOptions, JObject jObject, CrmApiEnumAnnotations crmApiEnumAnnotations = CrmApiEnumAnnotations.None)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKey, alternateValue, queryOptions);

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Headers = { { "Prefer", "return=representation" } },
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            IncludeAnnotations(req, crmApiEnumAnnotations);
            var response = await this.SendAsync(req);//200
            //Body should contain the requested new-contact information.
            JObject deserializeObject = JsonConvert.DeserializeObject<JObject>(
                await response.Content.ReadAsStringAsync());
            return deserializeObject;
        }

        /// <summary>
        /// 更新单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdateEntitySingleProp(string entityName, string guid, string attribute, JObject value)
        {
            //Create unique guidentifier by appending property name 
            var url = BuildGuidUrl(entityName, guid, null, attribute);

            //Now update just the single property.
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(value.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var responseMessage = await this.SendAsync(httpRequestMessage);//204
        }

        /// <summary>
        /// 插入/更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpsertAsync(string entityName, string guid, JObject value)
        {
            var url = BuildGuidUrl(entityName, guid);

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Headers =
                {
                    { "If-Match", "*" },
                    {"If-None-Match","*" }
                },
                Content = new StringContent(value.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req);//204
        }

        /// <summary>
        /// 插入/更新
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpsertAsync(string entityName, string alternateKey, string alternateValue, JObject value)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKey, alternateValue);

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Headers =
                {
                    {"If-Match", "*" },
                    {"If-None-Match","*" }
                },
                Content = new StringContent(value.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req);//204
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string entityName, string guid)
        {
            var url = BuildGuidUrl(entityName, guid);

            var req = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await this.SendAsync(req);//204
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string entityName, string alternateKey, string alternateValue)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKey, alternateValue);

            var req = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await this.SendAsync(req);//204
        }

        /// <summary>
        /// 删除单个属性
        /// </summary>
        /// <para>这无法用于单一值导航属性解除两个实体的关联</para>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public async Task DeleteEntitySingleProp(string entityName, string guid, string attribute)
        {
            //Create unique guidentifier by appending property name 
            var url = BuildGuidUrl(entityName, guid, attribute);

            //Now update just the single property.
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url); ;
            HttpResponseMessage responseMessage =
                await this.SendAsync(httpRequestMessage);//204
        }

        /// <summary>
        /// 发送记录
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            var response = await RestClient.SendAsync(requestMessage);
            HandError(response);
            return response;
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="response"></param>
        private static void HandError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new CrmHttpResponseException(response.Content);
            }
        }

        private static string BuildUrl(string entityName, string queryOptions = null, string attribute = null)
        {
            string url;
            if (string.IsNullOrWhiteSpace(queryOptions))
            {
                url = $"{Utils.ToPlural(entityName)}";
            }
            else
            {
                url = $"{Utils.ToPlural(entityName)}?{queryOptions}";
            }

            if (!string.IsNullOrWhiteSpace(attribute))
            {
                url += $"/{attribute}";
            }
            return url;
        }

        private static string BuildGuidUrl(string entityName, string guid, string queryOptions = null, string attribute = null)
        {
            string url;

            if (string.IsNullOrWhiteSpace(queryOptions))
            {
                url = $"{Utils.ToPlural(entityName)}({guid})";
            }
            else
            {
                url = $"{Utils.ToPlural(entityName)}({guid})?{queryOptions}";
            }

            if (!string.IsNullOrWhiteSpace(attribute))
            {
                url += $"/{attribute}";
            }
            return url;
        }

        private static string BuildAlternateKeyUrl(string entityName, string alternateKey, string alternateValue, string queryOptions = null)
        {
            string url;
            if (string.IsNullOrWhiteSpace(queryOptions))
            {
                url = $"{Utils.ToPlural(entityName)}({alternateKey}='{alternateValue}')";
            }
            else
            {
                url = $"{Utils.ToPlural(entityName)}({alternateKey}='{alternateValue}')?{queryOptions}";
            }
            return url;
        }

        private static void IncludeAnnotations(HttpRequestMessage requestMessage, CrmApiEnumAnnotations crmApiEnumAnnotations)
        {
            if (crmApiEnumAnnotations == CrmApiEnumAnnotations.None) return;
            switch (crmApiEnumAnnotations)
            {
                case CrmApiEnumAnnotations.FormattedValue:
                    requestMessage.Headers.Add("Prefer", "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"");
                    break;
                case CrmApiEnumAnnotations.Associatednavigationproperty:
                    requestMessage.Headers.Add("Prefer", "odata.include-annotations=\"Microsoft.Dynamics.CRM.associatednavigationproperty\"");
                    break;
                case CrmApiEnumAnnotations.Lookuplogicalname:
                    requestMessage.Headers.Add("Prefer", "odata.include-annotations=\"Microsoft.Dynamics.CRM.lookuplogicalname\"");
                    break;
                case CrmApiEnumAnnotations.All:
                    requestMessage.Headers.Add("Prefer", "odata.include-annotations=\"*\"");
                    break;
                case CrmApiEnumAnnotations.None:
                    break;
                default:
                    break;
            }
        }
    }
}
