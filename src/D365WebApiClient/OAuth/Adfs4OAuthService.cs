using D365WebApiClient.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace D365WebApiClient.OAuth
{
    public class Adfs4OAuthService : IOAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly Dynamics365Option _dynamics365Option;

        public Adfs4OAuthService(HttpClient httpClient, Dynamics365Option dynamics365Option)
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
            VerifyParams(_dynamics365Option);
            var tokenUrl = BuildTokenUrl(_dynamics365Option.ADFSUri);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            var content = BuildTokenParams(_dynamics365Option);
            using (var res = await _httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(content)))
            {
                var json = await res.Content.ReadAsStringAsync();
                res.EnsureSuccessStatusCode();
                var auth = JsonConvert.DeserializeObject<OAuthResult>(json);
                if (auth == null)
                {
                    throw new Exception(json);
                }

                return auth;
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
        private static void VerifyParams(Dynamics365Option dynamics365Option)
        {
            if (dynamics365Option == null)
            {
                throw new ArgumentNullException(nameof(dynamics365Option));
            }

            if (string.IsNullOrWhiteSpace(dynamics365Option.ADFSUri))
            {
                throw new ArgumentNullException(nameof(dynamics365Option.ADFSUri));
            }

            if (string.IsNullOrWhiteSpace(nameof(dynamics365Option.Resource)))
            {
                throw new ArgumentNullException(nameof(dynamics365Option.Resource));
            }

            if (string.IsNullOrWhiteSpace(dynamics365Option.ClientId))
            {
                throw new ArgumentNullException(nameof(dynamics365Option.ClientId));
            }

            if (string.IsNullOrWhiteSpace(dynamics365Option.ClientSecret))
            {
                throw new ArgumentNullException(nameof(dynamics365Option.ClientSecret));
            }

            if (string.IsNullOrWhiteSpace(dynamics365Option.DomainName))
            {
                throw new ArgumentNullException(nameof(dynamics365Option.DomainName));
            }

            if (string.IsNullOrWhiteSpace(dynamics365Option.UserName))
            {
                throw new ArgumentNullException(nameof(dynamics365Option.UserName));
            }

            if (string.IsNullOrWhiteSpace(dynamics365Option.Password))
            {
                throw new ArgumentNullException(nameof(dynamics365Option.Password));
            }
        }

        /// <summary>
        /// 构建ADFS TOKEN地址
        /// </summary>
        /// <param name="ADFS_Uri"></param>
        /// <returns></returns>
        private static string BuildTokenUrl(string ADFS_Uri)
        {
            if (!ADFS_Uri.EndsWith("/"))
            {
                ADFS_Uri = $"{ADFS_Uri}/";
            }
            return $"{ADFS_Uri}adfs/oauth2/token";
        }

        /// <summary>
        /// 构建请求Token参数
        /// </summary>
        /// <param name="dynamics365Option"></param>
        /// <returns></returns>
        private static List<KeyValuePair<string, string>> BuildTokenParams(Dynamics365Option dynamics365Option)
        {
            var content = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string,string>("client_id",dynamics365Option.ClientId),
                new KeyValuePair<string,string>("client_secret",dynamics365Option.ClientSecret),
                new KeyValuePair<string,string>("resource",dynamics365Option.Resource),
                new KeyValuePair<string,string>("username",$"{dynamics365Option.DomainName}\\{dynamics365Option.UserName}"),
                new KeyValuePair<string,string>("password",dynamics365Option.Password),
                new KeyValuePair<string,string>("grant_type","password"),
            };
            return content;
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
