#if NETSTANDARD2_0

using D365WebApiClient.Cache;
using D365WebApiClient.OAuth;
using D365WebApiClient.Options;
using D365WebApiClient.Services.WebApiServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class D365WebApiClientServiceCollectionExtensions
    {

        public static IServiceCollection AddD365WebApiClientService(this IServiceCollection services, Action<Dynamics365Option> setupAction)
        {
            return services.AddD365WebApiClientService<DistributedCacheManager>(setupAction);
        }


        /// <summary>
        /// Adds services to the specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TCacheManager">缓存管理</typeparam>
        /// <param name="services">
        /// The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection"/> to add
        /// services to.
        /// </param>
        /// <param name="setupAction">An <see cref="T:System.Action`1"/> to configure the provided</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection"/> so that
        /// additional calls can be chained.
        /// </returns>
        public static IServiceCollection AddD365WebApiClientService<TCacheManager>(this IServiceCollection services, Action<Dynamics365Option> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var options = new Dynamics365Option();
            setupAction(options);
            services.TryAddSingleton(options);
            services.TryAddTransient<OAuthMessageHandler>();
            services.AddHttpClient<IApiClientService, ApiClientService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(options.WebApiAddress);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
            {
                var httpClientHandler = new HttpClientHandler()
                {
                    AllowAutoRedirect = true,
                    UseDefaultCredentials = true
                };
                var dynamic365Option = serviceProvider.GetRequiredService<Dynamics365Option>();
                if (!dynamic365Option.IsIfd)
                {
                    httpClientHandler.Credentials = new NetworkCredential(dynamic365Option.UserName, dynamic365Option.Password, dynamic365Option.DomainName);
                }
                return httpClientHandler;

            }).AddHttpMessageHandler<OAuthMessageHandler>();
            if (options.IsIfd)
            {
                services.AddHttpClient<OAuthService>(httpClient =>
                {
                    httpClient.BaseAddress = new Uri(options.ADFS_URI);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                }).ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
                {
                    var httpClientHandler = new HttpClientHandler()
                    {
                        AllowAutoRedirect = false,
                        UseDefaultCredentials = true
                    };
                    return httpClientHandler;

                });
            }
            services.TryAddScoped(typeof(ICacheManager), typeof(TCacheManager));
            services.TryAddScoped<IAsyncLocker, AsyncLocker>();
            services.TryAddScoped<IApiClientService, ApiClientService>();
            return services;
        }
    }
}

#endif