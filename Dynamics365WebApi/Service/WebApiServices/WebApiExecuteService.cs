using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynamics365WebApi.Service
{
    public partial class WebApiService
    {
        /// <summary>
        /// 执行固定函数
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> ExecuteAsync(HttpMethod httpMethod, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            var req = BuildRequest(httpMethod, url);

            var response = await this.SendAsync(req);

            var jObject = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return jObject;
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
