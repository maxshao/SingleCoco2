using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SingleCoco.Infrastructure.Controllers.Secret
{
    public class SecretAppService : ISecretAppService
    {
        #region Initialize

        /// <summary>
        /// 领域接口
        /// </summary>
        private readonly ISecretDomain _secret;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="secret"></param>
        public SecretAppService(ISecretDomain secret)
        {
            _secret = secret;
        }

        #endregion

        #region API Implements

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<UserDto> GetCurrentUserAsync(string account, string password)
        {
            var user = await _secret.GetUserForLoginAsync(account, password);

            //Todo：AutoMapper 做实体转换

            return null;
        }

        #endregion
    }
}
