using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SingleCoco.Api.Infrastructure;
using SingleCoco.Dtos.Query.Furniture;
using SingleCoco.Entities.Furniture;
using SingleCoco.Repository.IFurniture;

namespace SingleCoco.Api.Controllers
{

    /// <summary>
    /// 库存管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureProductController : ControllersBase
    {
        private readonly IProductsRepository _productsRepository;
        public FurnitureProductController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }


        [HttpPost("gets")]
        public JsonResult GetProducts(ProductQuery q)
        {
            string sqlWhere = "1=1";

            var products = _productsRepository.GetPage(q.PageIndex, q.PageSize, null, sqlWhere);

            return Json(products);
        }

        [HttpGet("get")]
        public JsonResult GetProduct(int id)
        {
            return Json(_productsRepository.Get(id));
        }


        //[HttpGet("delete")]
        //public JsonResult DeleteProduct(int pId)
        //{
        //    var result = _productsRepository.Delete(pId);
        //    return Json(result.ToString());
        //}


        [HttpPost("save")]
        [ValidateFilter]
        public JsonResult SaveProduct([CustomizeValidator]ProductDto pdto)
        {
            var p = new Products()
            {
                RepositoryId = pdto.RepositoryId,
                Name = pdto.Name,
                Code = pdto.Code,
                Specification = pdto.Specification,
                Type = pdto.Type,
                Brand = pdto.Brand,
                Origin = pdto.Origin,
                Unit = pdto.Unit,
                Inventory = pdto.Inventory,
                UnitPrice = pdto.UnitPrice,
                AgentId = pdto.AgentId,
                Description = pdto.Description
            };
            var result = _productsRepository.Add(p);

            return Json<string>( result, "保存成功！");
        }
    }
}