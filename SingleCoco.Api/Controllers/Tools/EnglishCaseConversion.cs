using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SingleCoco.Api.Controllers
{
    [Route("api/[controller]")]
    public class EnglishCaseConversion : ControllersBase
    {
        /// <summary>
        /// 英文字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("UpperCase/{input}")]
        public JsonResult ToUpperCase(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return Json(input.ToUpper());
            else
                return null;
        }

        /// <summary>
        /// 英文字母小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("LowerCase/{input}")]
        public string ToLowerCase(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return input.ToLower();
            else
                return null;
        }
    }
}
