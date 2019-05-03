using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Dtos.Query.Systerm
{
    public class LogonQuery
    {
        public string Name { get; set; }
        public string Psw { get; set; }
    }


    public class LogonQueryValidator : AbstractValidator<LogonQuery>
    {
        public LogonQueryValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage("请输入名称.");
            RuleFor(c => c.Name).MinimumLength(3).MaximumLength(18).WithMessage("请输入3-18位的用户名");
        }

    }
}
