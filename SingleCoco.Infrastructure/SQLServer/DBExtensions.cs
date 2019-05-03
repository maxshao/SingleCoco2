using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SingleCoco.Infrastructure
{
    internal static class DBExtensions
    {

        /// <summary>
        /// 获取设置默认连接池数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static int GetConnectionPoolSize<T>() where T : class, new()
        {
            string database = GetDatabaseName<T>();
            string[] poolSizeArray = ConfigurationManager.AppSettings["ConnectionsPoolSize"].Split('|');
            if (poolSizeArray != null)
            {
                foreach (string sizeItem in poolSizeArray)
                {
                    string[] sizeItemArray = sizeItem.Split(':');
                    if (database == sizeItemArray[0])
                        return int.Parse(sizeItemArray[1]);
                }
            }
            return 50;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetConnectionString<T>() where T : class, new()
        {
            string tableName = GetTableName<T>();
            string[] databaseArray = ConfigurationManager.AppSettings["DatabaseArray"].Split('|');
            if (databaseArray != null)
            {
                foreach (string database in databaseArray)
                {
                    string tableNameList = ConfigurationManager.AppSettings[database];
                    string[] tables = ConfigurationManager.AppSettings[database].Split('|');
                    if (tables != null && tables.Length > 0)
                        if (tables.Contains(tableName))
                            return ConfigurationManager.ConnectionStrings[database].ConnectionString;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetTableName<T>() where T : class, new()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetDatabaseName<T>() where T : class, new()
        {
            string tableName = GetTableName<T>();
            string[] databaseArray = ConfigurationManager.AppSettings["DatabaseArray"].Split('|');
            if (databaseArray != null)
            {
                foreach (string database in databaseArray)
                {
                    string tableNameList = ConfigurationManager.AppSettings[database];
                    string[] tables = ConfigurationManager.AppSettings[database].Split('|');
                    if (tables != null && tables.Length > 0)
                        if (tables.Contains(tableName))
                            return database;
                }
            }
            return string.Empty;
        }




    }
}
