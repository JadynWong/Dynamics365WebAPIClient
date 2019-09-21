using D365WebApiClient.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace Demo.Standard
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, errors) => true;
            IServiceCollection services = new ServiceCollection();

            //注入
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddD365WebApiClientService(option =>
            {
                option.ADFSUri = "https://demoifd.testcrm.com/";
                option.ClientId = "*********-****-****-****-************";
                option.ClientSecret = "*************************************";
                option.DomainName = "testdoamin";
                option.UserName = "username";
                option.Password = "pa$$word";
                option.Dynamics365Type = Dynamics365Type.IFD_ADFS_V4;
                option.Organization = "d365test";
                //option.RedirectUri = "http://localhost";
                option.Resource = "https://d365test.testcrm.com/api/data/v9.0";
                option.Version = new Version(9, 0);

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