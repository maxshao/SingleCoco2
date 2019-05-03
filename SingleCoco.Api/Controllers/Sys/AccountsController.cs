using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SingleCoco.Api.Infrastructure;
using SingleCoco.Dtos.Query.Systerm;
using SingleCoco.Entities.Systerm;
using SingleCoco.Repository.ISystem;

namespace SingleCoco.Api.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Policy = "Permission")]
    [ApiController]
    public class AccountsController : ControllersBase
    {
        private readonly IAccountsRepository _accountsRepository;
        /// <summary>
        /// 日志记录服务
        /// </summary>
        private readonly ILogger _logger;
        public AccountsController(IAccountsRepository accountsRepository, ILogger<AccountsController> logger)
        {
            _accountsRepository = accountsRepository;
            _logger = logger;
        }

        [HttpPost("gets")]
        public JsonResult GetAccounts(AccountQuery q)
        {
            //string sqlWhere = $"Name like '%{q.Name}%'";
            string sqlWhere = q.Name == null ? "" : $"Name like '%{ q.Name}%'";
            var accounts = _accountsRepository.GetPage(q.PageIndex, q.PageSize, null, sqlWhere);
            return JsonToDataTables(accounts);
        }

        [HttpGet("get")]
        //[HttpGet("get/{id}")] // 不能使用这类,这样会导致角色对应的url不匹配
        public JsonResult GetAccount(int id)
        {
            return Json(_accountsRepository.Get(id));
        }

        /// <summary>
        /// 保存账户
        /// </summary>
        /// <returns></returns>
        [HttpPost("save")]
        [ValidateFilter]
        public JsonResult SaveAccount([CustomizeValidator]Accounts acc)
        {
            bool result = _accountsRepository.Add(acc);
            return Json<string>("保存成功", result);
        }

    }
}