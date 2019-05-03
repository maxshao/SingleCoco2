using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SingleCoco.Infrastructure
{
    public static class SqlConnectionPool
    {

        /// <summary>
        /// 单例锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 存储所有的连接字典
        /// </summary>
        private static Dictionary<string, SqlConnection> Connections = null;

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SqlConnection GetConnection<T>() where T : class, new()
        {
            string databaseName = DBExtensions.GetDatabaseName<T>();
            if (string.IsNullOrEmpty(databaseName))
                return null;
            // 初始实例化
            if (Connections == null)
            {
                lock (locker)
                {
                    Connections = new Dictionary<string, SqlConnection>();
                }
            }
            // 获取到可用 key
            string connKey = FindFreeSqlConnection(databaseName);
            if (connKey != null)
                return Connections[connKey];
            else// 初始化加载数据库连接
            {
                string strconn = DBExtensions.GetConnectionString<T>();
                int poolSize = DBExtensions.GetConnectionPoolSize<T>();
                lock (locker)
                {
                    for (int i = 0; i < poolSize; ++i)
                    {
                        SqlConnection conn = new SqlConnection(strconn);
                        conn.Open();
                        Connections.Add(databaseName + "_" + i.ToString(), conn);
                        conn.Close();
                    }
                }
                return Connections[FindFreeSqlConnection(databaseName)];
            }
        }

        /// <summary>
        /// 查找可用的SQL连接
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns>数据库连接对象Key</returns>
        private static string FindFreeSqlConnection(string databaseName)
        {
            IEnumerable<string> connKeys = Connections.Keys.Where(item => item.StartsWith(databaseName));
            if (connKeys != null && connKeys.Count() > 0)
            {
                foreach (string key in connKeys)
                {
                    if (Connections[key].State == ConnectionState.Closed)
                        return key;
                }
            }
            return null;
        }

        /// <summary>
        /// 清空与指定连接关联的连接池
        /// </summary>
        /// <param name="sql"></param>
        public static void ClearConnection(this SqlConnection sql)
        {
            SqlConnection.ClearPool(sql);
        }


    }


}
