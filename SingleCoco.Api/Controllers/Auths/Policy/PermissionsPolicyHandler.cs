using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using SingleCoco.Entities.Systerm;
using SingleCoco.Infrastructure.Controllers;
using SingleCoco.Repository.ISystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SingleCoco.Api.Controllers
{
    public class PermissionsPolicyHandler : AuthorizationHandler<PolicyRequirement>
    {

        /// <summary>
        /// 授权方式（cookie, bearer, oauth, openid）
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// jwt 服务
        /// </summary>
        private readonly IJwtRepository _jwtService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="jwtApp"></param>
        public PermissionsPolicyHandler(IAuthenticationSchemeProvider schemes, IJwtRepository jwtService)
        {
            Schemes = schemes;
            _jwtService = jwtService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {

            //Todo：获取角色、Url 对应关系
            List<Menus> list = new List<Menus> {
                new Menus
                {
                    GId = Guid.Empty.ToString(),
                    Href = "/api/Accounts/Get"
                },
                new Menus
                {
                    GId=Guid.Empty.ToString(),
                    Href="/api/v1.0/secret/deactivate"
                },
                new Menus
                {
                    GId=Guid.Empty.ToString(),
                    Href="/api/v1.0/secret/refresh"
                }
            };

            var httpContext = (context.Resource as AuthorizationFilterContext).HttpContext;

            //获取授权方式
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                //验证签发的用户信息
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (result.Succeeded)
                {
                    //判断是否为已停用的 Token
                    if (!await _jwtService.IsCurrentActiveTokenAsync())
                    {
                        context.Fail();
                        return;
                    }

                    httpContext.User = result.Principal;

                    //判断角色与 Url 是否对应
                    var url = httpContext.Request.Path.Value.ToLower();
                    var role = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
                    var menu = list.Where(x => x.GId.Equals(role) && x.Href.ToLower().Equals(url)).FirstOrDefault();

                    if (menu == null)
                    {
                        context.Fail();
                        return;
                    }

                    //判断是否过期
                    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.UtcNow)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                    return;
                }
            }
            context.Fail();
        }
    }

}
