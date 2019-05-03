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
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureConfigController : ControllersBase
    {

        private readonly IWarehouseRepository _warehouseRepository;


        public FurnitureConfigController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }


        #region 仓库管理

        [HttpPost("getwarehouses")]
        public JsonResult GetRepositorys(WarehouseQuery q)
        {
            var rst = _warehouseRepository.GetPage(q.PageIndex, q.PageSize, null);
            return Json(rst);
        }

        [HttpGet("getwarehouse")]
        public JsonResult GetRepository(int id)
        {
            return Json(_warehouseRepository.Get(id));
        }

        [HttpGet("deletewarehouse")]
        public JsonResult DeleteRepository(int id)
        {
            return Json<string>( _warehouseRepository.Delete(id), "删除成功！");
        }

        [HttpPost("savewarehouse")]
        [ValidateFilter]
        public JsonResult SaveRepository([CustomizeValidator]WarehouseDto m)
        {
            var q = new Repositorys()
            {
                Id = m.Id,
                Name = m.Name,
                Address = m.Address
            };
            return Json<string>( _warehouseRepository.Add(q), "保存成功!");
        }

        #endregion


        #region 系统配置













        #endregion



    }
}