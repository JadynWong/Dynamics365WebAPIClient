using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dynamics365WebApi.Service;
using Newtonsoft.Json.Linq;

namespace DynamicsApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var crmApiService = new WebApiService();
            //var a = crmApiService.WhoImIAsync().Result;
            //Console.WriteLine(a);
            var b = crmApiService.ReadAsync("systemuser", null).Result;
            Console.WriteLine(b);
            Console.ReadKey();
        }


    }
}
