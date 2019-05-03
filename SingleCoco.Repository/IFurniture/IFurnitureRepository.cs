using SingleCoco.Infrastructure.Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Repository.IFurniture
{

    public interface IFurnitureRepository<T> : IRepository<T> where T : class
    {
    }
}
