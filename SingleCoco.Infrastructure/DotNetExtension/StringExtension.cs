using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Infrastructure.DotNetExtension
{
    public static class StringExtension
    {

        /// <summary>
        /// 删除字符串中最后匹配的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveLastIndexOf(this string str, string value)
        {
            // 判断是否是初始字符
            if (str.LastIndexOf(value) != 0)
                return str.Remove(str.LastIndexOf(value), value.Length);
            else
                return str;
        }

    }
}
