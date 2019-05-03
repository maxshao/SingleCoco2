using Dapper.Contrib.Extensions;
using FluentValidation;
using System;


namespace SingleCoco.Entities.Systerm
{
    [Table("Sys_Actions")]
    public class Actions
    {
        [Key]
        public int Id { get; set; }
        public string GId { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }
    }
}
