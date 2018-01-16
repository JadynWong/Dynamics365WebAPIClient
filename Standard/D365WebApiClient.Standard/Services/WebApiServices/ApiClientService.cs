using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using D365WebApiClient.Auth;
using D365WebApiClient.Standard.Common;
using D365WebApiClient.Standard.Configs;
using D365WebApiClient.Standard.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace D365WebApiClient.Standard.Services.WebApiServices
{
    /// <summary>
    /// Dynamics365 WebApi服务
    /// <para>单例使用</para>
    /// </summary>
    public partial class ApiClientService : IApiClientService
    {
        private readonly HttpClient _restClient;

        private readonly Dynamics365Options _dynamics365Options;

        /// <summary>
        /// Dynamics365配置
        /// </summary>
        public Dynamics365Options Dynamics365Options => _dynamics365Options;

        /// <summary>
        /// 构造Dynamics365 WebAPI服务
        /// </summary>
        public ApiClientService(IOptions<Dynamics365Options> options, IDistributedCache distributedCache)
        {
            _dynamics365Options = options.Value;
            if (_dynamics365Options.IsIfd)
            {
                _restClient = BuildOnIfd(distributedCache);
            }
            else
            {
                _restClient = BuildOnPremise();
            }
        }

        /// <summary>
        /// 构造IFD HTTP客户端
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private HttpClient BuildOnIfd(IDistributedCache distributedCache)
        {
            if (distributedCache == null)
                throw new ArgumentNullException(nameof(distributedCache));
            var httpMessageHandler = new OAuthMessageHandler(_dynamics365Options.ADFS_URI,
                _dynamics365Options.Resource,
                _dynamics365Options.ClientId,
                _dynamics365Options.RedirectUri,
                $"{_dynamics365Options.DomainName}\\{_dynamics365Options.UserName}",
                _dynamics365Options.Password,
                distributedCache,
                new HttpClientHandler());
            ;
            return GetNewHttpClient(httpMessageHandler, _dynamics365Options.WebApiAddress);
        }

        /// <summary>
        /// 构造On-Premise HTTP客户端
        /// </summary>
        /// <returns></returns>
        private HttpClient BuildOnPremise()
        {
            var httpMessageHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential(_dynamics365Options.UserName, _dynamics365Options.Password, _dynamics365Options.DomainName)
            };
            return GetNewHttpClient(httpMessageHandler, _dynamics365Options.WebApiAddress);
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