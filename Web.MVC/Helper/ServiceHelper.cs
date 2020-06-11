using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder()
        {
            string[] serviceUrls = { "http://localhost:9060", "http://localhost:9061", "http://localhost:9062" };//订单服务的地址，可以放在配置文件或者数据库等等...

            //每次随机访问一个服务实例
            var Client = new RestClient(serviceUrls[new Random().Next(0, 3)]);
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            string[] serviceUrls = { "http://localhost:9050", "http://localhost:9051", "http://localhost:9052" };//产品服务的地址，可以放在配置文件或者数据库等等...

            //每次随机访问一个服务实例
            var Client = new RestClient(serviceUrls[new Random().Next(0, 3)]);
            var request = new RestRequest("/products", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
