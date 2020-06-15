using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    /// <summary>
    /// 通过gateway调用服务
    /// </summary>
    public class GatewayServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder()
        {
            var Client = new RestClient("http://localhost:9070");
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            var Client = new RestClient("http://localhost:9070");
            var request = new RestRequest("/products", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            throw new NotImplementedException();
        }
    }
}
