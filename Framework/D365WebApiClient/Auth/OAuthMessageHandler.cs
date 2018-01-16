using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using D365WebApiClient.Cache;

namespace D365WebApiClient.Auth
{
    public class OAuthMessageHandler : DelegatingHandler
    {
        private AuthenticationHeaderValue _authHeader;

        private static readonly object LockObj = new object();

        private const string TokenKey = "CrmAuth_{0}_{1}_{2}";
        private readonly string _adfsUri;
        private readonly string _resource;
        private readonly string _clientId;
        private readonly string _redirectUri;
        private readonly string _userName;
        private readonly string _password;
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="ADFS_Uri">ADFS地址</param>
        /// <param name="resource">CRM地址</param>
        /// <param name="clientId">clientIdd</param>
        /// <param name="redirectUri">redirectUri</param>
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <param name="cacheManager">缓存</param>
        /// <param name="innerHandler">innerHandler</param>
        public OAuthMessageHandler(string ADFS_Uri,
            string resource,
            string clientId,
            string redirectUri,
            string userName,
            string password,
            ICacheManager cacheManager,
            HttpMessageHandler innerHandler) : base(innerHandler)
        {
            this._adfsUri = ADFS_Uri;
            this._resource = resource;
            this._clientId = clientId;
            this._redirectUri = redirectUri;
            this._userName = userName;
            this._password = password;
            this._cacheManager = cacheManager;
        }

        /// <summary>
        /// 执行Http请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var accessToken = GetToken();
            _authHeader = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Authorization = _authHeader;
            var httpResponseMessage = await base.SendAsync(request, cancellationToken);
            return httpResponseMessage;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual string GetToken()
        {
            string accessToken;
            var tokenKey = string.Format(TokenKey, this._resource, this._clientId, this._userName);
            if (_cacheManager.IsSet(tokenKey))
            {
                accessToken = _cacheManager.Get<string>(tokenKey);
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    return accessToken;
                }
            }

            // 保证缓存Token不会过期
            var now = DateTimeOffset.Now;
            lock (LockObj)
            {
                if (_cacheManager.IsSet(tokenKey))
                {
                    accessToken = _cacheManager.Get<string>(tokenKey);
                    if (!string.IsNullOrWhiteSpace(accessToken))
                    {
                        return accessToken;
                    }
                }

                var authResult =
                    Dynamics365Auth.AcquireToken(_adfsUri, _resource, _clientId, _redirectUri, _userName, _password);
                if (authResult == null)
                {
                    throw new Exception("Auth get failed");
                }

                var expiresIn = authResult.expires_in;
                accessToken = authResult.access_token;
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    throw new Exception("Token get failed");
                }

                var cacheTime = now + TimeSpan.FromSeconds(expiresIn);

                _cacheManager.Set<string>(tokenKey, cacheTime, accessToken);
                return accessToken;
            }
        }
    }
}