using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dynamics365WebApi.Auth;
using Dynamics365WebApi.Cache;
using Dynamics365WebApi.Common;
using Dynamics365WebApi.Configs;
using Dynamics365WebApi.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynamics365WebApi.Service
{
    /// <summary>
    /// Dynamics365 WebApi服务
    /// <para>单例使用</para>
    /// </summary>
    public class WebApiService
    {
        private readonly HttpClient _restClient;

        private readonly Dynamics365Config _dynamics365Config;
        
        /// <summary>
        /// Dynamics365配置
        /// </summary>
        public Dynamics365Config Dynamics365Config => _dynamics365Config;

        /// <summary>
        /// 构造Dynamics365 WebAPI服务
        /// </summary>
        public WebApiService()
        {
            _dynamics365Config = new Dynamics365Config();
            if (_dynamics365Config.IsIfd)
            {
                var cacheManager = new RuntimeCacheManager();
                _restClient = BuildOnIfd(cacheManager);
            }
            else
            {
                _restClient = BuildOnPremise();
            }
        }

        /// <summary>
        /// 构造Dynamics365 WebAPI服务
        /// </summary>
        /// <param name="dynamics365Config">Dynamics365配置</param>
        /// <param name="cacheManager">缓存管理(on-premise时无须提供)</param>
        /// <exception cref="ArgumentNullException"></exception>
        public WebApiService(Dynamics365Config dynamics365Config, ICacheManager cacheManager)
        {
            if (dynamics365Config == null)
                throw new ArgumentNullException(nameof(dynamics365Config));

            _dynamics365Config = dynamics365Config;
            if (_dynamics365Config.IsIfd)
            {
                if (cacheManager == null)
                    throw new ArgumentNullException(nameof(cacheManager));
                _restClient = BuildOnIfd(cacheManager);
            }
            else
            {
                _restClient = BuildOnPremise();
            }
        }

        /// <summary>
        /// 构造IFD HTTP客户端
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private HttpClient BuildOnIfd(ICacheManager cacheManager)
        {
            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));
            var httpMessageHandler = new OAuthMessageHandler(_dynamics365Config.ADFS_URI,
                _dynamics365Config.Resource,
                _dynamics365Config.ClientId,
                _dynamics365Config.RedirectUri,
                $"{_dynamics365Config.DomainName}\\{_dynamics365Config.UserName}",
                _dynamics365Config.Password,
                cacheManager,
                new HttpClientHandler());
            ;
            return GetNewHttpClient(httpMessageHandler, _dynamics365Config.WebApiAddress);
        }

        /// <summary>
        /// 构造On-Premise HTTP客户端
        /// </summary>
        /// <returns></returns>
        private HttpClient BuildOnPremise()
        {
            var httpMessageHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential(_dynamics365Config.UserName, _dynamics365Config.Password, _dynamics365Config.DomainName)
            };
            return GetNewHttpClient(httpMessageHandler, _dynamics365Config.WebApiAddress);
        }

        /// <summary>
        /// 获取HTTP 客户端
        /// </summary>
        /// <param name="httpMessageHandler"></param>
        /// <param name="webApiBaseAddress"></param>
        /// <returns></returns>
        private static HttpClient GetNewHttpClient(HttpMessageHandler httpMessageHandler, string webApiBaseAddress)
        {
            var httpClient = new HttpClient(httpMessageHandler)
            {
                BaseAddress = new Uri(webApiBaseAddress),
                Timeout = new TimeSpan(0, 2, 0),
                DefaultRequestHeaders =
                {
                    {
                        "OData-MaxVersion",
                        "4.0"
                    },
                    {
                        "OData-Version",
                        "4.0"
                    },
                    {
                        "Accept",
                        "application/json"
                    }
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
            return jObject; //200
        }

        /// <summary>
        /// WebApiVersion
        /// </summary>
        /// <returns></returns>
        public async Task<Version> WebApiVersion()
        {
            var url = $"RetrieveVersion";

            var req =
                new HttpRequestMessage(HttpMethod.Get, url);

            var response =
                await this.SendAsync(req); //200
            JObject retrievedVersion = JsonConvert.DeserializeObject<JObject>(
                await response.Content.ReadAsStringAsync());
            //Capture the actual version available in this organization
            return Version.Parse((string) retrievedVersion.GetValue("Version"));
        }
        
        /// <summary>
        /// WebApiVersion
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> Execute(string url)
        {
            if(string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await this.SendAsync(req);
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject; //200
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
        /// <param name="webApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> CreateAndReadAsync(string entityName, string queryOptions, JObject jObject,
            WebApiEnumAnnotations webApiEnumAnnotations = WebApiEnumAnnotations.None)
        {
            var url = BuildUrl(entityName, queryOptions);

            var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Headers =
                {
                    {
                        "Prefer",
                        "return=representation"
                    }
                },
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            IncludeAnnotations(req, webApiEnumAnnotations);
            var response = await this.SendAsync(req); //201
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
        /// <param name="webApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string guid, string queryOptions,
            WebApiEnumAnnotations webApiEnumAnnotations = WebApiEnumAnnotations.None)
        {
            var url = BuildGuidUrl(entityName, guid, queryOptions);

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            IncludeAnnotations(req, webApiEnumAnnotations);
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
        /// <param name="queryOptions"></param>
        /// <param name="webApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string alternateKey, string alternateValue,
            string queryOptions, WebApiEnumAnnotations webApiEnumAnnotations = WebApiEnumAnnotations.None)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKey, alternateValue, queryOptions);

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await this.SendAsync(req); //200
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="webApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadAsync(string entityName, string queryOptions,
            WebApiEnumAnnotations webApiEnumAnnotations = WebApiEnumAnnotations.None)
        {
            var url = BuildUrl(entityName, queryOptions);

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            IncludeAnnotations(req, webApiEnumAnnotations);
            var response = await this.SendAsync(req); //200
            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
        }

        /// <summary>
        /// 查询单个属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <param name="webApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> ReadEntitySingleProp(string entityName, string guid, string attribute,
            WebApiEnumAnnotations webApiEnumAnnotations = WebApiEnumAnnotations.None)
        {
            var url = BuildGuidUrl(entityName, guid, null, attribute);
            //Now retrieve just the single property.
            JObject prop;
            var req = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            IncludeAnnotations(req, webApiEnumAnnotations);
            HttpResponseMessage responseMessage =
                await this.SendAsync(req);
            prop = JsonConvert.DeserializeObject<JObject>(
                await responseMessage.Content.ReadAsStringAsync()); //200
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
        public async Task UpdatePutAsync(string entityName, string alternateKey, string alternateValue, JObject jObject)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKey, alternateValue);

            var req = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req); //204
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
        public async Task UpdatePatchAsync(string entityName, string alternateKey, string alternateValue,
            JObject jObject)
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
        /// <param name="webApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName, string guid, string queryOptions,
            JObject jObject, WebApiEnumAnnotations webApiEnumAnnotations = WebApiEnumAnnotations.None)
        {
            var url = BuildGuidUrl(entityName, guid, queryOptions);

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Headers =
                {
                    {
                        "Prefer",
                        "return=representation"
                    }
                },
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            IncludeAnnotations(req, webApiEnumAnnotations);
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
        /// <param name="webApiEnumAnnotations"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateAndReadAsync(string entityName, string alternateKey, string alternateValue,
            string queryOptions, JObject jObject,
            WebApiEnumAnnotations webApiEnumAnnotations = WebApiEnumAnnotations.None)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKey, alternateValue, queryOptions);

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Headers =
                {
                    {
                        "Prefer",
                        "return=representation"
                    }
                },
                Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            IncludeAnnotations(req, webApiEnumAnnotations);
            var response = await this.SendAsync(req); //200
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
            var responseMessage = await this.SendAsync(httpRequestMessage); //204
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
                    {
                        "If-Match",
                        "*"
                    },
                    {
                        "If-None-Match",
                        "*"
                    }
                },
                Content = new StringContent(value.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req); //204
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
                    {
                        "If-Match",
                        "*"
                    },
                    {
                        "If-None-Match",
                        "*"
                    }
                },
                Content = new StringContent(value.ToString(Formatting.None), Encoding.UTF8, "application/json")
            };
            var response = await this.SendAsync(req); //204
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
            var response = await this.SendAsync(req); //204
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
            var response = await this.SendAsync(req); //204
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
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
            ;
            HttpResponseMessage responseMessage =
                await this.SendAsync(httpRequestMessage); //204
        }

        /// <summary>
        /// 发送记录
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            var response = await _restClient.SendAsync(requestMessage);
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
                throw new WebApiHttpResponseException(response.Content);
            }
        }

        /// <summary>
        /// 构造Url
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="queryOptions"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 构造Guid Url
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="queryOptions"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private static string BuildGuidUrl(string entityName, string guid, string queryOptions = null,
            string attribute = null)
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

        
        /// <summary>
        /// 构造备用键Url
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <param name="queryOptions"></param>
        /// <returns></returns>
        private static string BuildAlternateKeyUrl(string entityName, string alternateKey, string alternateValue,
            string queryOptions = null)
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

        /// <summary>
        /// 注释处理
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="webApiEnumAnnotations"></param>
        private static void IncludeAnnotations(HttpRequestMessage requestMessage,
            WebApiEnumAnnotations webApiEnumAnnotations)
        {
            if (webApiEnumAnnotations == WebApiEnumAnnotations.None) return;
            switch (webApiEnumAnnotations)
            {
                case WebApiEnumAnnotations.FormattedValue:
                    requestMessage.Headers.Add("Prefer",
                        "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"");
                    break;
                case WebApiEnumAnnotations.Associatednavigationproperty:
                    requestMessage.Headers.Add("Prefer",
                        "odata.include-annotations=\"Microsoft.Dynamics.CRM.associatednavigationproperty\"");
                    break;
                case WebApiEnumAnnotations.Lookuplogicalname:
                    requestMessage.Headers.Add("Prefer",
                        "odata.include-annotations=\"Microsoft.Dynamics.CRM.lookuplogicalname\"");
                    break;
                case WebApiEnumAnnotations.All:
                    requestMessage.Headers.Add("Prefer", "odata.include-annotations=\"*\"");
                    break;
                case WebApiEnumAnnotations.None:
                    break;
                default:
                    break;
            }
        }
    }
}