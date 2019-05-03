using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Dtos.Query.Furniture
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string Type { get; set; }
        public string Brand { get; set; }
        /// <summary>
        /// 产地
        /// </summary>
        public string Origin { get; set; }
        public string Unit { get; set; }
        public decimal Inventory { get; set; }
        public decimal UnitPrice { get; set; }
        public int AgentId { get; set; }
        public string Description { get; set; }
    }
}
