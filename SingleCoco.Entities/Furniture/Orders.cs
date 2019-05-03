using Dapper.Contrib.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Entities.Furniture
{
    [Table("Furniture_Order")]
    public class Orders
    {

        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int RepositoryId { get; set; }
        public int AgentId { get; set; }
        public string Sales { get; set; }
        public string ContractNumber { get; set; }
        public int AdminId { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Count { get; set; }
        public string Description { get; set; }
    }

    public class OrdersValidator : AbstractValidator<Orders>
    {
        public OrdersValidator()
        {
            RuleFor(c => c.ProductId).NotNull().NotEmpty().WithMessage("请输入仓库名称.");
        }

    }
}
