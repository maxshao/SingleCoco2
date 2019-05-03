using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Entities
{
    public class Utility
    {
        /// <summary>
        /// 数据库连接配置
        /// </summary>
        public enum ConnectionStrings
        {
            /// <summary>
            /// 系统
            /// </summary>
            MSSqlSys,
            ///// <summary>
            ///// 日志
            ///// </summary>
            //Logs,
            ///// <summary>
            ///// 订单
            ///// </summary>
            //Orders,
            ///// <summary>
            ///// 客户
            ///// </summary>
            //Customers,
        }


    }



    public class TableMessage
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 当前页显示数目
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 总数
        /// </summary>
        public int PageCount { get; set; }
    }




    public class JQDataTableMessage<T> where T : class
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }

        public IEnumerable<T> data { get; set; }
    }





}
