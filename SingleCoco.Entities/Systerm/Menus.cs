using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Entities.Systerm
{

    [Table("Sys_Menus")]
    public class Menus
    {
        [Key]
        public int Id { get; set; }
        public string GId { get; set; }

        public string Name { get; set; }

        public string Href { get; set; }
        /// <summary>
        /// 上级Id
        /// </summary>
        public string Pid { get; set; }

        public string Icon { get; set; }

    }
}
