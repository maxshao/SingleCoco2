using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Infrastructure.Dapper
{
    public class AppSettings
    {
        public static readonly string DefaultKey = "MSSqlSys";
        public static readonly string DefaultProviderName = "system.data.sqlclient";

        /// <summary>
        /// 连接字符串都在这里
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> DbConStrDic { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        public static void SetAppSettings(IConfigurationRoot Configuration, IList<string> dbNames)
        {
            if (dbNames.Count == 0)
                throw new Exception("数据库初始化无连接字符串!");
            var dic = new Dictionary<string, string>();

            foreach (var dbStr in dbNames)
            {
                dic.Add(Configuration.GetSection($"ConnectionStrings:{dbStr}:ProviderName").Value, Configuration.GetSection($"ConnectionStrings:{dbStr}:ConnectionString").Value);
                DbConStrDic.Add(dbStr, dic);
            }
        }
    }
}
