using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dynamics365WebApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Dynamics365WebApi.Token
{
    public class CrmAuth
    {
        ///<summary>
        /// Token
        /// </summary>
        public string access_token { get; set; }

        ///<summary>
        /// 类型
        /// </summary>
        public string token_type { get; set; }

        ///<summary>
        /// 有效期 秒
        /// </summary>
        public int expires_in { get; set; }

        ///<summary>
        /// refresh_Token 用于刷新Token
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 请求Token
        /// </summary>
        /// <param name="adfsUri">ADFS地址</param>
        /// <param name="resource">CRM地址</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="redirectUri">跳转地址</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static CrmAuth AcquireToken(string adfsUri, string resource, string clientId, string redirectUri, string userName, string password)
        {
            VerifyParams(adfsUri, resource, clientId, redirectUri, userName, password);

            var url = BuildCodeUrl(adfsUri, resource, clientId, redirectUri);

            var tokenUrl = BuildTokenUrl(adfsUri);

            var list = BuildCodeParams(userName, password);

            //包含上下文的 用WebRequest也可以
            using (var handler = new WebRequestHandler { AllowAutoRedirect = false })
            {
                using (var httpClient = new HttpClient(handler))
                {
                    //第1次请求
                    using (var response = httpClient.PostAsync(url, new FormUrlEncodedContent(list)).Result)
                    {
                        // 第2次请求 httpClient 还要用之前的 包含了第一次返回的Cookies
                        using (var response2 = httpClient.GetAsync(response.Headers.Location).Result)
                        {
                            // 获取返回的Code
                            var query = response2.Headers.Location.Query;
                            var col = Utils.GetQueryString(query);
                            var code = col["code"];
                            if (string.IsNullOrWhiteSpace(code))
                            {
                                throw new Exception(query);
                            }
                            // 第3次请求 请求Token
                            var tokenParams = BuildTokenParams(clientId, code, resource, redirectUri);
                            using (var response3 = httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(tokenParams)).Result)
                            {
                                var json = response3.Content.ReadAsStringAsync().Result;
                                var auth = JsonConvert.DeserializeObject<CrmAuth>(json);
                                if (auth == null)
                                    throw new Exception(json);
                                return auth;
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="adfsUri"></param>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public static CrmAuth CrmRefreshToken(string adfsUri, string refresh_token)
        {
            var tokenUrl = BuildTokenUrl(adfsUri);
            using (var httpClient = new HttpClient())
            {
                // 第3次请求 请求Token
                var tokenParams = BuildRefreshTokenParams(refresh_token);
                using (var response3 = httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(tokenParams)).Result)
                {
                    var json = response3.Content.ReadAsStringAsync().Result;
                    var auth = JsonConvert.DeserializeObject<CrmAuth>(json);
                    if (auth == null)
                        throw new Exception(json);
                    return auth;
                }
            }
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="adfsUri"></param>
        /// <param name="resource"></param>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        private static void VerifyParams(string adfsUri, string resource, string clientId, string redirectUri, string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(adfsUri))
                throw new ArgumentNullException(nameof(adfsUri));
            if (string.IsNullOrWhiteSpace(nameof(resource)))
                throw new ArgumentNullException(resource);
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(redirectUri))
                throw new ArgumentNullException(nameof(redirectUri));
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));
        }

        /// <summary>
        /// 构建ADFS CODE地址
        /// </summary>
        /// <param name="adfsUri"></param>
        /// <param name="resource"></param>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        private static string BuildCodeUrl(string adfsUri, string resource, string clientId, string redirectUri)
        {

            if (!adfsUri.EndsWith("/"))
                adfsUri = $"{adfsUri}/";
            var url = $"{adfsUri}adfs/oauth2/authorize";

            var response_type = "code";

            var sb = new StringBuilder();
            sb.Append(url);
            sb.Append("?");
            sb.Append($"response_type={response_type}");
            sb.Append("&");
            sb.Append($"client_id={clientId}");
            sb.Append("&");
            sb.Append($"redirect_uri={redirectUri}");
            sb.Append("&");
            sb.Append($"resource={resource}");
            url = sb.ToString();
            return url;
        }

        /// <summary>
        /// 构建ADFS TOKEN地址
        /// </summary>
        /// <param name="adfsUri"></param>
        /// <returns></returns>
        private static string BuildTokenUrl(string adfsUri)
        {
            return $"{adfsUri}adfs/oauth2/token";
        }

        /// <summary>
        /// 构建请求Code参数
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static List<KeyValuePair<string, string>> BuildCodeParams(string userName, string password)
        {
            var list = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("UserName",userName),
                new KeyValuePair<string, string>("Password",password),
                new KeyValuePair<string, string>("Kmsi","true"),
                new KeyValuePair<string, string>("AuthMethod","FormsAuthentication")
            };
            return list;
        }

        /// <summary>
        /// 构建请求Token参数
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="code"></param>
        /// <param name="resource"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        private static List<KeyValuePair<string, string>> BuildTokenParams(string clientId, string code, string resource, string redirectUri)
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("resource", resource),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
            };
            return list;
        }

        /// <summary>
        /// 构建请求刷新Token参数
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        private static List<KeyValuePair<string, string>> BuildRefreshTokenParams(string refresh_token)
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refresh_token),
            };
            return list;
        }

    }
}
