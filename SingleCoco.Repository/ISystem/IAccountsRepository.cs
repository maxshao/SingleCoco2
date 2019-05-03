using SingleCoco.Infrastructure.Dapper;
using SingleCoco.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using SingleCoco.Entities.Systerm;
using System.Threading.Tasks;
using SingleCoco.Dtos.Query.User;

namespace SingleCoco.Repository.ISystem
{
    public interface IAccountsRepository : ISystemRepository<Accounts>
    {

        Task<Accounts> GetCurrentUserAsync(string accountName, string password);

    }
}
