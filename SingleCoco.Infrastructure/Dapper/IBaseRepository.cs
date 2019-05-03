using SingleCoco.Infrastructure.Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Infrastructure.Dapper
{
    public interface IBaseRepository<T> : IRepository<T> where T : class
    {
    }
}
