using Dapper.Contrib.Extensions;
using FluentValidation;
using System;

namespace SingleCoco.Entities.Systerm
{
    [Table("Sys_Accounts")]
    public class Accounts
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Player { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;
    }

    public class AccountsValidator : AbstractValidator<Accounts>
    {
        public AccountsValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("请输入账户名称.");
            RuleFor(c => c.Name).NotNull().WithMessage("请输入账户名称.");
            RuleFor(c => c.Password).NotEmpty().WithMessage("请输入账户密码.");
        }

    }
}