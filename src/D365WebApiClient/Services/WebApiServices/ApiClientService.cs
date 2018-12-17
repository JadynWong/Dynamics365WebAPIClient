using D365WebApiClient.Cache;
using D365WebApiClient.Common;
using D365WebApiClient.Exceptions;
using D365WebApiClient.Options;
using D365WebApiClient.Values;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace D365WebApiClient.Services.WebApiServices
{
    /// <inheritdoc />
    public partial class ApiClientService : IApiClientService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 构造Dynamics365 WebAPI服务
        /// </summary>
        /// <param name="dynamics365Options">Dynamics365配置</param>
        /// <param name="cacheManager">缓存管理(on-premise时无须提供)</param>
        /// <param name="httpClient"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            EnumAnnotations? enumAnnotations = default(EnumAnnotations?),
            int? maxPageSize = default(int?),
            bool representation = false)
        {
            return BuildRequest(HttpMethod.Get, url, null, enumAnnotations, maxPageSize, representation);
        }

        /// <summary>
        /// 构造请求
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <param name="enumAnnotations"></param>
        /// <param name="maxPageSize"></param>
        /// <param name="representation"></param>
        /// <returns></returns>
        private static HttpRequestMessage BuildRequest(HttpMethod httpMethod, string url, Value value = default(Value),
            EnumAnnotations? enumAnnotations = default(EnumAnnotations?),
            int? maxPageSize = default(int?),
            bool representation = false)
        {
            var req = new HttpRequestMessage(httpMethod, url);
            if (value != null)
            {
                req.Content = new StringContent(value.ToString(Formatting.None), Encoding.UTF8, "application/json");
            }

            var prefer = new List<string>();
            switch (enumAnnotations)
            {
                case EnumAnnotations.None:
                    break;

                case EnumAnnotations.FormattedValue:
                    prefer.Add(
                        "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"");
                    break;

                case EnumAnnotations.AssociatedNavigationProperty:
                    prefer.Add(
                        "odata.include-annotations=\"Microsoft.Dynamics.CRM.associatednavigationproperty\"");
                    break;

                case EnumAnnotations.LookupLogicalName:
                    prefer.Add(
                        "odata.include-annotations=\"Microsoft.Dynamics.CRM.lookuplogicalname\"");
                    break;

                case EnumAnnotations.MicrosoftDynamicsCRM:
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