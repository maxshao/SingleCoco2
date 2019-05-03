using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace SingleCoco.Infrastructure.Dapper
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        /// <summary>
        /// connection string
        /// </summary>
        private string _connStr;
        /// <summary>
        ///  database type
        /// </summary>
        private string _dbType;
        public DbConnectionFactory()
        {
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection DbConnectionStore()
        {
            IDbConnection conn = null;
            switch (_dbType)
            {
                case "system.data.sqlclient":
                    conn = new System.Data.SqlClient.SqlConnection(_connStr);
                    break;
                //case "mysql":
                //    conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                //    break;
                default:
                    conn = new System.Data.SqlClient.SqlConnection(_connStr);
                    break;
            }
            return conn;
        }
        /// <summary>
        /// 返回指定数据连接
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public IDbConnection DbConnectionStore(string dbType, string connStr)
        {
            _dbType = dbType;
            _connStr = connStr;
            return DbConnectionStore();
        }
        /// <summary>
        /// 获取默认连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDefaultConnection()
        {
            return DbConnectionStore(AppSettings.DefaultProviderName, AppSettings.DbConStrDic[AppSettings.DefaultKey][AppSettings.DefaultProviderName]);
        }
    }
}
