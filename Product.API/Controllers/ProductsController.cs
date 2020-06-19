using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Product.API.MessageDto;
using Product.API.Models;

namespace Product.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICapPublisher _capBus;
        private readonly ProductContext _context;

        public ProductsController(ILogger<ProductsController> logger, IConfiguration configuration, ICapPublisher capPublisher, ProductContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _capBus = capPublisher;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string result = $"【产品服务】{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}——" +
                $"{Request.HttpContext.Connection.LocalIpAddress}:{_configuration["ConsulSetting:ServicePort"]}";
            return Ok(result);
        }

        /// <summary>
        /// 减库存 订阅下单事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        [CapSubscribe("order.services.createorder")]
        public async Task ReduceStock(CreateOrderMessageDto message)
        {
            //业务代码
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == message.ProductID);
            product.Stock -= message.Count;

            await _context.SaveChangesAsync();
        }

    }
}