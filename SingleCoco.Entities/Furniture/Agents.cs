using Dapper.Contrib.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Entities.Furniture
{
    [Table("Furniture_Agent")]
    public class Agents
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
    }


    public class AgentsValidator : AbstractValidator<Agents>
    {
        public AgentsValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage("请输入代理商名称.");
        }

    }

}
