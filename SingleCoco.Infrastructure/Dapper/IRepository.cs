using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SingleCoco.Infrastructure.Dapper
{
    public interface IRepository<T> where T : class
    {
        #region Contrib.Extensions

        bool Add(T entity);
        void AddBatch(IEnumerable<T> entitys);
        bool Update(T entity);
        bool Update(params Expression<Func<T, object>>[] fields);
        bool Delete(T entity);
        bool Delete(string Id);
        bool Delete(int Id);
        void Delete(Guid Id);
        T Get(string Id);
        T Get(Guid Id);
        T Get(int Id);
        T Get(T entity);
        T Get(Expression<Func<T, bool>> func);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetList(Expression<Func<T, bool>> where = null, Expression<Func<T, bool>> order = null);
        Tuple<int, IEnumerable<T>> GetPage(Page page, object where = null, params Expression<Func<T, object>>[] order);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sqlOrder"></param>
        /// <param name="sqlWhere"></param>
        /// <returns>int:pageIndex,int:pageSize,int:pageCount,IEnumerable<t>:data</returns>
        Tuple<int, int, int, IEnumerable<T>> GetPage(int pageIndex, int pageSize, string sqlOrder, string sqlWhere = null);
        /// <summary>
        /// 根据查询条件检索数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        long Count(params Expression<Func<T, object>>[] where);
        /// <summary>
        /// 根据查询条件检索数据是否存在,非参数化查询[有风险]
        /// </summary>
        /// <param name="t"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Exists(T t, params Expression<Func<T, object>>[] where);
        /// <summary>
        /// 根据查询条件检索数据是否存在,参数化查询
        /// </summary>
        /// <param name="t"></param>
        /// <param name="obj">ex:new {name:value,n:v}</param>
        /// <param name="fields"></param>
        /// <returns></returns>
        bool Exists(T t, object obj, params Expression<Func<T, object>>[] fields);
        #endregion

    }
}