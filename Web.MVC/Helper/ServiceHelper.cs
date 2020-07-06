using Consul;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ConsulClient _consulClient;
        private ConcurrentBag<string> _orderServiceUrls;
        private ConcurrentBag<string> _productServiceUrls;

        public ServiceHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            });
        }

        public async Task<string> GetOrder(string accessToken)
        {
            if (_productServiceUrls == null)
                return await Task.FromResult("【订单服务】正在初始化服务列表...");

            //每次随机访问一个服务实例
            var Client = new RestClient(_orderServiceUrls.ElementAt(new Random().Next(0, _orderServiceUrls.Count())));
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct(string accessToken)
        {
            if(_productServiceUrls == null)
                return await Task.FromResult("【产品服务】正在初始化服务列表...");

            //每次随机访问一个服务实例
            var Client = new RestClient(_productServiceUrls.ElementAt(new Random().Next(0, _productServiceUrls.Count())));
            var request = new RestRequest("/products", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            var serviceNames = new string[] { "OrderService", "ProductService" };
            Array.ForEach(serviceNames, p =>
            {
                Task.Run(() =>
                {
                    //WaitTime默认为5分钟
                    var queryOptions = new QueryOptions { WaitTime = TimeSpan.FromMinutes(10) };
                    while (true)
                    {
                        GetServices(queryOptions, p);
                    }
                });
            });
        }
        private void GetServices(QueryOptions queryOptions, string serviceName)
        {
            var res = _consulClient.Health.Service(serviceName, null, true, queryOptions).Result;

            Console.WriteLine($"{DateTime.Now}获取{serviceName}：queryOptions.WaitIndex：{queryOptions.WaitIndex}  LastIndex：{res.LastIndex}");

            //版本号不一致 说明服务列表发生了变化
            if (queryOptions.WaitIndex != res.LastIndex)
            {
                queryOptions.WaitIndex = res.LastIndex;

                //服务地址列表
                var serviceUrls = res.Response.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

                if (serviceName == "OrderService")
                    _orderServiceUrls = new ConcurrentBag<string>(serviceUrls);
                else if (serviceName == "ProductService")
                    _productServiceUrls = new ConcurrentBag<string>(serviceUrls);
            }
        }
    }
}
