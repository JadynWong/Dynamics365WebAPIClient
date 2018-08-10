#if NETSTANDARD2_0

using System;
using D365WebApiClient.Cache;
using D365WebApiClient.Standard.Configs;
using D365WebApiClient.Standard.Services.WebApiServices;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class D365WebApiClientServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services to the specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.
        /// </summary>
        /// <param name="services">
        /// The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection"/> to add
        /// services to.
        /// </param>
        /// <param name="setupAction">An <see cref="T:System.Action`1"/> to configure the provided</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection"/> so that
        /// additional calls can be chained.
        /// </returns>
        public static IServiceCollection AddD365WebApiClientService(this IServiceCollection services, Action<Dynamics365Options> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));
            var options = new Dynamics365Options();
            setupAction(options);
            services.TryAddSingleton(options);
            services.TryAddScoped<ICacheManager, DistributedCacheManager>();
            services.TryAddScoped<IApiClientService, ApiClientService>();
            return services;
        }
    }
}

#endif