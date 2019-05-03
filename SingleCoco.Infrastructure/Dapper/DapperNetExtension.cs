using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SingleCoco.Infrastructure.Dapper
{
    public static class DapperNetExtension
    {
        /// <summary>
        /// 属性是否包含Dapper.Contrib.Extensions.KeyAttribute
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsKey(this PropertyInfo p)
        {
            return p.GetCustomAttribute(typeof(KeyAttribute)) != null;
        }
    }
}
