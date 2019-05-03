using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SingleCoco.Infrastructure.Dapper
{
    public interface IDbConnectionFactory
    {
        IDbConnection DbConnectionStore();
        IDbConnection DbConnectionStore(string dbType, string connStr);
        IDbConnection GetDefaultConnection();
    }
}