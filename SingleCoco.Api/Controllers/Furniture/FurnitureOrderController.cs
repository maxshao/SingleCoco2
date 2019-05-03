using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SingleCoco.Api.Infrastructure;
using SingleCoco.Dtos.Query.Furniture;
using SingleCoco.Entities.Furniture;
using SingleCoco.Repository.IFurniture;

namespace SingleCoco.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureOrderController : ControllersBase
    {

        private readonly IProductsRepository _productsRepository;
        private readonly IOrderRepository _orderRepository;
        public FurnitureOrderController(IOrderRepository orderRepository, IProductsRepository productsRepository)
        {
            _orderRepository = orderRepository;
            _productsRepository = productsRepository;
        }


        [HttpPost("gets")]
        [ValidateFilter]
        public JsonResult GetOrders([CustomizeValidator]OrderQuery q)
        {
            var orders = _orderRepository.GetPage(q.PageIndex, q.PageSize, null, null);

            return Json(orders);
        }

        [HttpGet("get")]
        public JsonResult GetOrder(int id)
        {
            return Json(_orderRepository.Get(id));
        }


        //[HttpGet("delete/{id}")]
        //public JsonResult DeleteOrder(int id)
        //{
        //    return Json("删除成功!", _orderRepository.Get(id));
        //}


        [HttpPost("save")]
        [ValidateFilter]
        public JsonResult SaveOrder([CustomizeValidator]OrderDto dto)
        {
            var product = _productsRepository.Get(dto.ProductId);

            var order = new Orders()
            {
                ProductId = dto.ProductId,
                AgentId = product.AgentId,
                //ContractNumber = 
                Count = dto.Count,
                Money = dto.UnitPrice,
                Description = dto.Description,
                RepositoryId = product.RepositoryId,
                Sales = "",
                ContractNumber = dto.ContractNumber,
                AdminId = 1,
                CostPrice = product.UnitPrice,
            };

            bool result = _orderRepository.Add(order);
            return Json<string>("保存成功", result);
        }

    }
}