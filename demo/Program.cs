using System;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Standard
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            //注入
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddD365WebApiClientService(option =>
            {
                option.ADFS_URI = "https://sts.crm.com/";
                option.ClientId = "A4EC2F43-9F3E-4504-9C9A-CC7B3F5FAA74";
                option.DomainName = "crm";
                option.UserName = "administrator";
                option.Password = "123+abc";
                option.IsIfd = true;
                option.Organization = "demo";
                option.RedirectUri = "http://127.0.0.1:5000/Api/Token";
                option.Resource = "https://demo.crm.com:446/";
                option.Version = new Version(8, 2);
            });
            services.AddTransient<Test>();
            //构建容器
            var serviceProvider = services.BuildServiceProvider();
            //解析
            var test = serviceProvider.GetService<Test>();

            test.TestService();
        }
    }
}