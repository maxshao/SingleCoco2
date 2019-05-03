using Dapper.Contrib.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Entities.Systerm
{
    [Table("Sys_Players")]
    public class Players
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsEable { get; set; }

        public string Keys { get; set; }
    }


    public class PlayersValidator : AbstractValidator<Players>
    {
        public PlayersValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("请输入用户名称.");
            RuleFor(c => c.Name).NotNull().WithMessage("请输入用户名称.");
        }

    }
}
