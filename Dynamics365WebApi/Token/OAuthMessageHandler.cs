using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Dynamics365WebApi.Token
{
    public class OAuthMessageHandler : DelegatingHandler
    {
        private AuthenticationHeaderValue _authHeader;

        /// <summary>
        /// Cache object
        /// </summary>
        protected ObjectCache Cache => MemoryCache.Default;

        private static readonly object LockObj = new object();

        private const string TokenKey = "CrmAuth";
        private readonly string _adfsUri;
        private readonly string _resource;
        private readonly string _clientId;
        private readonly string _redirectUri;
        private readonly string _userName;
        private readonly string _password;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="adfsUri">ADFS地址</param>
        /// <param name="resource">CRM地址</param>
        /// <param name="clientId">clientIdd</param>
        /// <param name="redirectUri">redirectUri</param>
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <param name="innerHandler">innerHandler</param>
        public OAuthMessageHandler(string adfsUri, string resource, string clientId, string redirectUri, string userName, string password,
                HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            this._adfsUri = adfsUri;
            this._resource = resource;
            this._clientId = clientId;
            this._redirectUri = redirectUri;
            this._userName = userName;
            this._password = password;

        }

        protected override async Task<HttpResponseMessage> SendAsync(
                 HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var accessToken = GetToken();
            _authHeader = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Authorization = _authHeader;
            var httpResponseMessage = await base.SendAsync(request, cancellationToken);
            return httpResponseMessage;
        }

        private string GetToken()
        {
            if (Cache.Contains(TokenKey))
                return (string)Cache[TokenKey];
            // 保证缓存Token不会过期
            var now = DateTime.Now;
            lock (LockObj)
            {
                if (Cache.Contains(TokenKey))
                    return (string)Cache[TokenKey];
                var authResult = CrmAuth.AcquireToken(_adfsUri, _resource, _clientId, _redirectUri, _userName, _password);
                var cacheTime = authResult.expires_In;
                var accessToken = authResult.access_token;
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    throw new Exception("Token get failed");
                }
                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = now + TimeSpan.FromSeconds(cacheTime)
                };
                Cache.Add(new CacheItem(TokenKey, accessToken), policy);
                return accessToken;
            }



        }

    }
}
