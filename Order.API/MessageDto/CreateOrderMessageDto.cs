using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.MessageDto
{
    /// <summary>
    /// 下单事件消息
    /// </summary>
    public class CreateOrderMessageDto
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }
    }
}
