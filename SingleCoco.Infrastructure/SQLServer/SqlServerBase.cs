using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SingleCoco.Infrastructure
{
    public class SqlServerBase
    {

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection<T>() where T : class, new()
        {
            return SqlConnectionPool.GetConnection<T>();
        }

    }
}
