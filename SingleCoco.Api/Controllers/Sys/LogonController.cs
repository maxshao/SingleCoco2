using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SingleCoco.Api.Infrastructure;
using SingleCoco.Dtos.Query.Systerm;
using SingleCoco.Entities.Systerm;
using SingleCoco.Repository.ISystem;

namespace SingleCoco.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogonController : ControllersBase
    {


        private readonly IAccountsRepository _accountsRepository;
        public LogonController(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }


        [HttpPost("in")]
        [ValidateFilter]
        public JsonResult Login([CustomizeValidator]LogonQuery q)
        {
            var x = new Accounts()
            {
                Name = q.Name,
                Password = q.Psw
            };
            var rt = _accountsRepository.Exists(x, new { x.Name, x.Password }, t => t.Name, t => t.Password);
            return Json<string>(rt, "");
        }

        [HttpPost("out")]
        public JsonResult LogOut()
        {
            return Json<string>(true, "ok");
        }


    }
}