using Dapper.Contrib.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Entities.Furniture
{
    [Table("Furniture_Repository")]
    public class Repositorys
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }

    public class RepositorysValidator : AbstractValidator<Repositorys>
    {
        public RepositorysValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage("请输入仓库名称.");
        }

    }
}
