using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using SingleCoco.Infrastructure.DotNetExtension;

namespace SingleCoco.Infrastructure.Dapper
{

    public static class LinqToWhere
    {
        /// <summary>
        /// 返回Expression查询的SQL ex: Name='' AND Password=''
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static string WhereToSQL<T>(this Expression<Func<T, object>> t, params Expression<Func<T, object>>[] fields) where T : class
        {
            List<PropertyInfo> propIn = fields.Select(p => p.ToPropertyInfo()).ToList();
            string parms = string.Empty, name = string.Empty;
            foreach (PropertyInfo property in propIn)
            {
                name = property.Name;
                var filed = propIn.Where(p => p.Name == property.Name).SingleOrDefault();
                if (filed != null)
                {
                    var value = t.GetType().GetProperty(property.Name).GetValue(t);
                    parms = string.Concat(property.Name, "=", value);
                }
            }
            return parms;
        }

        /// <summary>
        /// Object to SQL  ex: new {key = value,k = v}
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string WhereToSQL(this object o)
        {
            string parms = string.Empty;
            foreach (PropertyInfo property in o.GetType().GetProperties())
            {
                parms += string.Concat(property.Name, " = ", property.GetValue(o), " AND ");
            }
            return parms.RemoveLastIndexOf("AND");
        }


        public static string OrderToSQL<T>(this Expression<Func<T, object>>[] t) where T : class
        {
            List<PropertyInfo> propIn = t.Select(p => p.ToPropertyInfo()).ToList();
            string parms = string.Empty, name = string.Empty;
            foreach (PropertyInfo property in propIn)
            {
                name = property.Name;
                var filed = propIn.Where(p => p.Name == property.Name).SingleOrDefault();
                if (filed != null)
                {
                    var value = t.GetType().GetProperty(property.Name).GetValue(t);
                    parms = string.Concat(property.Name, "=", value);
                }
            }

            return parms;
        }




        public static string OrderToSQL<T>(this Expression<Func<T, object>> t, Expression<Func<T, object>> fields) where T : class
        {
            List<PropertyInfo> propIn = null;
            string parms = string.Empty, name = string.Empty;
            foreach (PropertyInfo property in propIn)
            {
                name = property.Name;
                var filed = propIn.Where(p => p.Name == property.Name).SingleOrDefault();
                if (filed != null)
                {
                    var value = t.GetType().GetProperty(property.Name).GetValue(t);
                    parms = string.Concat(property.Name, "=", value);
                }
            }

            return parms;
        }




        /// <summary>
        /// 把匿名委托表达式转成属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun">返回T某个属性的表达式 (如果是其他类型会引发错误)</param>
        /// <returns></returns>
        public static PropertyInfo ToPropertyInfo<T, TKey>(this Expression<Func<T, TKey>> fun)
        {
            PropertyInfo property = null;
            switch (fun.Body.NodeType)
            {
                case ExpressionType.Convert:
                    property = (PropertyInfo)((MemberExpression)((UnaryExpression)fun.Body).Operand).Member;
                    break;
                case ExpressionType.MemberAccess:
                    property = (PropertyInfo)((MemberExpression)fun.Body).Member;
                    break;
            }
            return property;
        }
    }



}
