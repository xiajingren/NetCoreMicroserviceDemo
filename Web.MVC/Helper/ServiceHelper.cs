using Consul;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        private readonly IConfiguration _configuration;

        public ServiceHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetOrder()
        {
            //string[] serviceUrls = { "http://localhost:9060", "http://localhost:9061", "http://localhost:9062" };//订单服务的地址，可以放在配置文件或者数据库等等...

            var consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            });

            //var agentServices = consulClient.Catalog.Services().Result.Response;
            var agentServices = consulClient.Agent.Services().Result.Response;//从Consul获取服务列表
            string[] serviceUrls = agentServices.Where(p => p.Value.Service == "OrderService")
                .Select(p => $"http://{p.Value.Address + ":" + p.Value.Port}").ToArray();//筛选订单服务的地址

            if (!serviceUrls.Any())
            {
                return await Task.FromResult("【订单服务】服务列表为空");
            }

            //每次随机访问一个服务实例
            var Client = new RestClient(serviceUrls[new Random().Next(0, serviceUrls.Length)]);
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            //string[] serviceUrls = { "http://localhost:9050", "http://localhost:9051", "http://localhost:9052" };//产品服务的地址，可以放在配置文件或者数据库等等...

            var consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            });

            //var agentServices = consulClient.Catalog.Services().Result.Response;
            var agentServices = consulClient.Agent.Services().Result.Response;//从Consul获取服务列表
            string[] serviceUrls = agentServices.Where(p => p.Value.Service == "ProductService")
                .Select(p => $"http://{p.Value.Address + ":" + p.Value.Port}").ToArray();//筛选订单服务的地址

            if (!serviceUrls.Any())
            {
                return await Task.FromResult("【产品服务】服务列表为空");
            }

            //每次随机访问一个服务实例
            var Client = new RestClient(serviceUrls[new Random().Next(0, serviceUrls.Length)]);
            var request = new RestRequest("/products", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
