using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SingleCoco.Infrastructure.Dapper
{

    public static class EntityExtension
    {
        /// <summary>
        /// 获取标记为key属性的字段名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetKey<T>(this T t) where T : class
        {
            Type type = typeof(T);
            string keyName = string.Empty;
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (pi.GetCustomAttribute<KeyAttribute>() != null)
                {
                    keyName = pi.Name; break;
                }
            }
            return keyName;
        }
    }
}
