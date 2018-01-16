using System;
using D365WebApiClient.Standard.Services.WebApiServices;
using Microsoft.Extensions.DependencyInjection;
using D365WebApiClient.DependencyInjection;
using D365WebApiClient.Standard.Configs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Demo.Standard
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            //注入
            services.AddTransient<ILoggerFactory, LoggerFactory>();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddD365WebApiClientService(x =>
            {
                x.ADFS_URI = "https://sts.crm.com/";
                x.ClientId = "A4EC2F43-9F3E-4504-9C9A-CC7B3F5FAA74";
                x.DomainName = "crm";
                x.UserName = "administrator";
                x.Password = "123+abc";
                x.IsIfd = true;
                x.Organization = "demo";
                x.RedirectUri = "http://127.0.0.1:5000/Api/Token";
                x.Resource = "https://demo.crm.com:446/";
                x.Version = new Version(8, 2);
            });
            services.AddTransient<Test>();
            //构建容器
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            //解析
            var test = serviceProvider.GetService<Test>();
            
            test.TestService();
        }
    }
}
