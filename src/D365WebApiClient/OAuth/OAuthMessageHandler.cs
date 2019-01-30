using D365WebApiClient.Cache;
using D365WebApiClient.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace D365WebApiClient.OAuth
{
    public class OAuthMessageHandler : DelegatingHandler
    {
        private AuthenticationHeaderValue _authHeader;

        private const string TokenKey = "CrmAuth_{0}_{1}_{2}";

        private readonly Dynamics365Option _dynamics365Options;
        private readonly ICacheManager _cacheManager;
        private readonly IOAuthService _oAuthService;
        private readonly IAsyncLocker _asyncLocker;

        /// <inheritdoc />
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <param name="dynamics365Options"></param>
        /// <param name="oAuthService"></param>
        /// <param name="asyncLocker"></param>
        public OAuthMessageHandler(
            Dynamics365Option dynamics365Options,
            ICacheManager cacheManager,
            IOAuthService oAuthService,
            IAsyncLocker asyncLocker)
        {
            _dynamics365Options = dynamics365Options;
            _cacheManager = cacheManager;
            _oAuthService = oAuthService;
            _asyncLocker = asyncLocker;
        }

        /// <inheritdoc />
        /// <summary>
        /// 执行Http请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {

            if (!request.Headers.Contains("Authentication"))
            {
                var accessToken = await GetTokenAsync();
                _authHeader = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Authorization = _authHeader;
            }



            var httpResponseMessage = await base.SendAsync(request, cancellationToken);
            return httpResponseMessage;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual async Task<string> GetTokenAsync()
        {
            string accessToken;
            var tokenKey = string.Format(TokenKey, _dynamics365Options.Resource, _dynamics365Options.ClientId, $"{_dynamics365Options.DomainName}\\{_dynamics365Options.UserName}");
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
            return await _asyncLocker.RunWithLockAsync(tokenKey, async () =>
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
                     await _oAuthService.AcquireTokenAsync();
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
             });
        }
    }
}