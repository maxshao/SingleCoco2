using SingleCoco.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Dtos.Query.Furniture
{
    public class OrderDto : TableMessage
    {
        public int ProductId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Count { get; set; }
        /// <summary>
        /// 售卖单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContractNumber { get; set; }
    }
}
