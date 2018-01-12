using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public partial class WebApiService
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
        private static string BuildGuidUrl(string entityName, Guid guid, string queryOptions = null,
            string attribute = null)
        {
            string url;

            if (string.IsNullOrWhiteSpace(queryOptions))
            {
                url = $"{Utils.ToPlural(entityName)}({guid:D})";
            }
            else
            {
                url = $"{Utils.ToPlural(entityName)}({guid:D})?{queryOptions}";
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
        /// <param name="alternateKeyValues"></param>
        /// <param name="queryOptions"></param>
        /// <returns></returns>
        private static string BuildAlternateKeyUrl(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues,
            string queryOptions = null)
        {
            string url;
            if (string.IsNullOrWhiteSpace(queryOptions))
            {
                url = $"{Utils.ToPlural(entityName)}({BuildAlternateKeyValues(alternateKeyValues)})";
            }
            else
            {
                url = $"{Utils.ToPlural(entityName)}({BuildAlternateKeyValues(alternateKeyValues)})?{queryOptions}";
            }

            return url;
        }

        /// <summary>
        /// 生成备用键
        /// </summary>
        /// <param name="alternateKeyValues"></param>
        /// <returns></returns>
        private static string BuildAlternateKeyValues(IEnumerable<KeyValuePair<string, string>> alternateKeyValues)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (alternateKeyValues == null)
            {
                throw new ArgumentNullException(nameof(alternateKeyValues));
            }
            foreach (var alternateKeyValue in alternateKeyValues)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(",");
                }

                stringBuilder.AppendFormat("{0}='{1}'", alternateKeyValue.Key, alternateKeyValue.Value);
            }

            if (stringBuilder.Length == 0)
            {
                throw new ArgumentException("不能为空", nameof(alternateKeyValues));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 构造请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <param name="representation"></param>
        /// <returns></returns>
        private static HttpRequestMessage BuildGetRequest(string url,
            EnumAnnotations? enumAnnotations = null,
            int? maxPageSize = null,
            bool representation = false)
        {
            return BuildRequest(HttpMethod.Get, url, null, enumAnnotations, maxPageSize, representation);
        }

        /// <summary>
        /// 构造请求
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="url"></param>
        /// <param name="jObject"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <param name="representation"></param>
        /// <returns></returns>
        private static HttpRequestMessage BuildRequest(HttpMethod httpMethod, string url, JObject jObject = null,
            EnumAnnotations? enumAnnotations = null,
            int? maxPageSize = null,
            bool representation = false)
        {
            var req = new HttpRequestMessage(httpMethod, url);
            if (jObject != null)
                req.Content = new StringContent(jObject.ToString(Formatting.None), Encoding.UTF8, "application/json");
            var prefer = new List<string>();
            switch (enumAnnotations)
            {
                case EnumAnnotations.None:
                    break;
                case EnumAnnotations.FormattedValue:
                    prefer.Add(
                        "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"");
                    break;
                case EnumAnnotations.Associatednavigationproperty:
                    prefer.Add(
                        "odata.include-annotations=\"Microsoft.Dynamics.CRM.associatednavigationproperty\"");
                    break;
                case EnumAnnotations.Lookuplogicalname:
                    prefer.Add(
                        "odata.include-annotations=\"Microsoft.Dynamics.CRM.lookuplogicalname\"");
                    break;
                case EnumAnnotations.MicrosoftDynamicsCrmAll:
                    prefer.Add(
                        "odata.include-annotations=\"Microsoft.Dynamics.CRM.*\"");
                    break;
                case EnumAnnotations.All:
                    prefer.Add("odata.include-annotations=\"*\"");
                    break;
                case null:
                    break;
                default:
                    break;
            }

            if (maxPageSize.HasValue)
            {
                prefer.Add($"odata.maxpagesize={maxPageSize.Value}");
            }

            if (representation)
            {
                prefer.Add("return=representation");
            }
            if (prefer.Count > 0)
            {
                req.Headers.Add("Prefer", prefer);
            }

            return req;
        }
    }
}