using SingleCoco.Repository.ISystem;
using SingleCoco.Infrastructure.Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Repository.System
{
    public class SystemRepository<T> : Repository<T>, ISystemRepository<T> where T : class
    {
        public SystemRepository() : base(AppSettings.DefaultProviderName, AppSettings.DbConStrDic[AppSettings.DefaultKey][AppSettings.DefaultProviderName])
        {
        }

    }
}
