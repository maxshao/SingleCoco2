
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SingleCoco.Dtos.Query.Jwt;
using SingleCoco.Dtos.Query.User;
using SingleCoco.Infrastructure.Controllers.Auths;
using SingleCoco.Repository.ISystem;
using System;
using System.Threading.Tasks;

namespace SingleCoco.Api.Controllers
{
    [ApiController]
    //[ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AuthsController : ControllerBase
    {
        #region Initialize

        /// <summary>
        /// Jwt 服务
        /// </summary>
        private readonly IJwtRepository _jwtApp;

        /// <summary>
        /// 日志记录服务
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// 用户服务
        /// </summary>
        private readonly IAccountsRepository _accountRepository;

        /// <summary>
        /// 配置信息
        /// </summary>
        public IConfiguration _configuration { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="jwtApp"></param>
        /// <param name="accountRepository"></param>
        public AuthsController(ILogger<AuthsController> logger, IConfiguration configuration, IJwtRepository jwtApp, IAccountsRepository accountRepository)
        {
            _configuration = configuration;
            _jwtApp = jwtApp;
            _accountRepository = accountRepository;
            _logger = logger;
        }

        #endregion

        #region APIs

        /// <summary>
        /// 停用 Jwt 授权数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("deactivate")]
        public async Task<IActionResult> CancelAccessToken()
        {
            await _jwtApp.DeactivateCurrentAsync();
            return Ok();
        }

        /// <summary>
        /// 获取 Jwt 授权数据
        /// </summary>
        /// <param name="dto">授权用户信息</param>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody]AuthsDto dto)
        {
            // 获取账户信息
            var users = await _accountRepository.GetCurrentUserAsync(dto.Account, dto.Password);
            UserDto user = null;
            if (users != null)
                user = new UserDto()
                {
                    Id = users.Id.ToString(),
                    UserName = users.Name,
                    Email = "a@a.com",
                    Role = Guid.Empty,
                    Phone = "13912345678",
                };

            if (user == null)
                return Ok(new JwtResponseDto
                {
                    Access = "无权访问",
                    Type = "Bearer",
                    Profile = new Profile
                    {
                        Name = dto.Account,
                        Auths = 0,
                        Expires = 0
                    }
                });

            var jwt = _jwtApp.Create(user);

            return Ok(new JwtResponseDto
            {
                Access = jwt.Token,
                Type = "Bearer",
                Profile = new Profile
                {
                    Name = user.UserName,
                    Auths = jwt.Auths,
                    Expires = jwt.Expires
                }
            });
        }

        /// <summary>
        /// 刷新 Jwt 授权数据
        /// </summary>
        /// <param name="dto">刷新授权用户信息</param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessTokenAsync([FromBody]AuthsDto dto)
        {
            //Todo：获取用户信息
            //var user = new UserDto
            //{
            //    Id = "111id",
            //    UserName = "yuiter",
            //    Role = Guid.Empty,
            //    Email = "yuiter@yuiter.com",
            //    Phone = "13912345678",
            //};
            var users = await _accountRepository.GetCurrentUserAsync(dto.Account, dto.Password);
            UserDto user = null;
            if (users != null)
                user = new UserDto()
                {
                    Id = users.Id.ToString(),
                    UserName = users.Name,
                    Email = "a@a.com",
                    Role = Guid.Empty,
                    Phone = "13912345678",
                };

            if (user == null)
                return Ok(new JwtResponseDto
                {
                    Access = "无权访问",
                    Type = "Bearer",
                    Profile = new Profile
                    {
                        Name = dto.Account,
                        Auths = 0,
                        Expires = 0
                    }
                });

            var jwt = await _jwtApp.RefreshAsync(dto.Token, user);

            return Ok(new JwtResponseDto
            {
                Access = jwt.Token,
                Type = "Bearer",
                Profile = new Profile
                {
                    Name = user.UserName,
                    Auths = jwt.Success ? jwt.Auths : 0,
                    Expires = jwt.Success ? jwt.Expires : 0
                }
            });
        }

        #endregion
    }
}