using D365WebApiClient.Common;
using D365WebApiClient.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace D365WebApiClient.OAuth
{
    public class Adfs3OAuthService : IOAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly Dynamics365Option _dynamics365Option;

        public Adfs3OAuthService(HttpClient httpClient, Dynamics365Option dynamics365Option)
        {
            _httpClient = httpClient;
            _dynamics365Option = dynamics365Option;
        }

        #region Methods

        /// <summary>
        /// 请求Token
        /// </summary>
        /// <returns></returns>
        public virtual async Task<OAuthResult> AcquireTokenAsync()
        {
            var userName = $"{_dynamics365Option.DomainName}\\{_dynamics365Option.UserName}";
            VerifyParams(_dynamics365Option.ADFSUri, _dynamics365Option.Resource, _dynamics365Option.ClientId, _dynamics365Option.RedirectUri, userName, _dynamics365Option.Password);

            var url = BuildCodeUrl(_dynamics365Option.ADFSUri, _dynamics365Option.Resource, _dynamics365Option.ClientId, _dynamics365Option.RedirectUri);

            var tokenUrl = BuildTokenUrl(_dynamics365Option.ADFSUri);

            var list = BuildCodeParams(userName, _dynamics365Option.Password);


            //第1次请求
            using (var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(list)))
            {
                // 第2次请求 httpClient 还要用之前的 包含了第一次返回的Cookies
                using (var response2 = await _httpClient.GetAsync(response.Headers.Location))
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
                    var tokenParams = BuildTokenParams(_dynamics365Option.ClientId, code, _dynamics365Option.Resource, _dynamics365Option.RedirectUri);
                    using (var response3 =
                       await _httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(tokenParams)))
                    {
                        var json = await response3.Content.ReadAsStringAsync();
                        var auth = JsonConvert.DeserializeObject<OAuthResult>(json);
                        if (auth == null)
                        {
                            throw new Exception(json);
                        }

                        return auth;
                    }
                }
            }

        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public virtual async Task<OAuthResult> CrmRefreshTokenAsync(string refresh_token)
        {
            var tokenUrl = BuildTokenUrl(_dynamics365Option.ADFSUri);
            // 第3次请求 请求Token
            var tokenParams = BuildRefreshTokenParams(refresh_token);
            using (var response3 = await _httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(tokenParams)))
            {
                var json = await response3.Content.ReadAsStringAsync();
                var auth = JsonConvert.DeserializeObject<OAuthResult>(json);
                if (auth == null)
                {
                    throw new Exception(json);
                }

                return auth;
            }
        }

        #endregion Methods

        #region Helper

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="ADFS_Uri">ADFS地址</param>
        /// <param name="resource">Dynamics地址</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="redirectUri">跳转地址</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        private static void VerifyParams(string ADFS_Uri, string resource, string clientId, string redirectUri,
            string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(ADFS_Uri))
            {
                throw new ArgumentNullException(nameof(ADFS_Uri));
            }

            if (string.IsNullOrWhiteSpace(nameof(resource)))
            {
                throw new ArgumentNullException(resource);
            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                throw new ArgumentNullException(nameof(redirectUri));
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
        }

        /// <summary>
        /// 构建ADFS CODE地址
        /// </summary>
        /// <param name="ADFS_Uri"></param>
        /// <param name="resource"></param>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        private static string BuildCodeUrl(string ADFS_Uri, string resource, string clientId, string redirectUri)
        {
            if (!ADFS_Uri.EndsWith("/"))
            {
                ADFS_Uri = $"{ADFS_Uri}/";
            }

            var url = $"{ADFS_Uri}adfs/oauth2/authorize";

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
        /// <param name="ADFS_Uri"></param>
        /// <returns></returns>
        private static string BuildTokenUrl(string ADFS_Uri)
        {
            return $"{ADFS_Uri}adfs/oauth2/token";
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
                new KeyValuePair<string, string>("UserName", userName),
                new KeyValuePair<string, string>("Password", password),
                new KeyValuePair<string, string>("Kmsi", "true"),
                new KeyValuePair<string, string>("AuthMethod", "FormsAuthentication")
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
        private static List<KeyValuePair<string, string>> BuildTokenParams(string clientId, string code,
            string resource, string redirectUri)
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

        #endregion Helper
    }
}
