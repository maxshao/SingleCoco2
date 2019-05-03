using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Infrastructure
{
    public class Page
    {

        /// <summary>
        /// 当前页,页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页显示数目
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int Count { get; set; }

    }

    public enum OrderByDes
    {
        desc,
        asc
    }


}
