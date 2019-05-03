using SingleCoco.Infrastructure.Dapper;
using SingleCoco.Repository.IFurniture;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Repository.Furniture
{
    public class FurnitureRepository<T> : Repository<T>, IFurnitureRepository<T> where T : class
    {
        public FurnitureRepository() : base(AppSettings.DefaultProviderName, AppSettings.DbConStrDic[AppSettings.DefaultKey][AppSettings.DefaultProviderName])
        {

        }
    }
}
