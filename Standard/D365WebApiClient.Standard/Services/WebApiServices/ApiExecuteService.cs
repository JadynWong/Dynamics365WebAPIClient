using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace D365WebApiClient.Standard.Services.WebApiServices
{
    public partial class ApiClientService
    {
        /// <summary>
        /// 执行固定函数
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> ExecuteAsync(HttpMethod httpMethod, string url, JObject jObject = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            var req = BuildRequest(httpMethod, url, jObject);

            var response = await this.SendAsync(req);

            var value = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return value;
        }

        /// <summary>
        /// 执行自定义请求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage req)
        {
            if (req == null)
                throw new ArgumentNullException(nameof(req));

            var response = await this.SendAsync(req);

            return response;
        }
    }
}
