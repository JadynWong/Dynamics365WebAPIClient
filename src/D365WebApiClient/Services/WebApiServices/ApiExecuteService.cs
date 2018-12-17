using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using D365WebApiClient.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace D365WebApiClient.Services.WebApiServices
{
    public partial class ApiClientService
    {
        /// <inheritdoc />
        /// <summary>
        /// 执行固定函数
        /// </summary>
        /// <returns></returns>
        public virtual async Task<Value> ExecuteAsync(HttpMethod httpMethod, string url, Value value = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            var req = BuildRequest(httpMethod, url, value);

            var response = await this.ExecuteAsync(req);

            var result = Value.Read(await response.Content.ReadAsStringAsync());
            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// 执行自定义请求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage req)
        {
            if (req == null)
                throw new ArgumentNullException(nameof(req));

            var response = await _httpClient.SendAsync(req).ConfigureAwait(false);
            HandError(response);
            return response;
        }
    }
}
