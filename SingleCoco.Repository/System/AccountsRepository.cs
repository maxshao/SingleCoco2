using SingleCoco.Repository.ISystem;
using SingleCoco.Infrastructure.Dapper;
using SingleCoco.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using SingleCoco.Entities.Systerm;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using SingleCoco.Infrastructure.DotNetExtension;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using SingleCoco.Dtos.Query.User;

namespace SingleCoco.Repository.System
{
    public class AccountsRepository : SystemRepository<Accounts>, IAccountsRepository
    {
        public async Task<Accounts> GetCurrentUserAsync(string accountName, string password)
        {
            string sql = "SELECT Id ,Name ,Player ,Password ,CreateAt FROM Sys_Accounts WHERE Name = @account AND Password = @password; ";

            var user = await Db.QueryFirstOrDefaultAsync<Accounts>(sql, new { account = accountName, password = password });
            if (user == default(Accounts)) return null;
            return user;
        }
    }
}
