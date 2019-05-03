using Dapper;
using Dapper.Contrib.Extensions;
using SingleCoco.Infrastructure.DotNetExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SingleCoco.Infrastructure.Dapper
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected IDbConnection Db { get; private set; }

        string _dbType;
        string _connStr;
        public Repository() { }
        public Repository(string dbType, string connStr)
        {
            _dbType = dbType; _connStr = connStr;
            Db = new DbConnectionFactory().DbConnectionStore(dbType, connStr);
            if (Db.State == ConnectionState.Closed) Db.Open();
        }


        private IDbConnection GetDb()
        {
            return new DbConnectionFactory().DbConnectionStore(_dbType, _connStr);
        }

        #region Contrib.Extensions

        public bool Add(T entity)
        {
            using (var db = GetDb())
            {
                db.Open();
                using (var trans = db.BeginTransaction())
                {
                    bool result = db.Insert(entity, trans) > 0;
                    if (!result) trans.Rollback();
                    else trans.Commit();
                    return result;
                }
            }
        }

        public void AddBatch(IEnumerable<T> entitys)
        {
            Db.Insert(entitys);
        }
        /// <summary>
        /// 根据查询条件检索数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public long Count(params Expression<Func<T, object>>[] where)
        {
            string sql = "SELECT COUNT({0}) FROM {1} WHERE {2}";

            List<PropertyInfo> propIn = where.Select(p => p.ToPropertyInfo()).ToList();


            //sql = string.Format(sql, where.GetKey(), typeof(T).Name, where.WhereToSQL());
            return (long)Db.ExecuteScalar(sql);
        }

        public bool Delete(T entity)
        {
            return Db.Delete(entity);
        }

        public bool Delete(string Id)
        {
            var entity = Get(Id);
            if (entity != null)
                return Delete(entity);
            else return false;
        }

        public bool Delete(int Id)
        {
            var entity = Get(Id);
            if (entity != null)
                Delete(entity);
            return false;
        }

        public void Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public T Get(string Id)
        {
            return Db.Get<T>(Id);
        }

        public T Get(Guid Id)
        {
            return Db.Get<T>(Id);
        }

        public T Get(int Id)
        {
            using (Db)
            {
                return Db.Get<T>(Id);
            }
        }

        public T Get(T entity)
        {
            return Db.Get<T>(entity);
        }

        public T Get(Expression<Func<T, bool>> func)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            return Db.GetAll<T>();
        }

        public IEnumerable<T> GetList(Expression<Func<T, bool>> where = null, Expression<Func<T, bool>> order = null)
        {
            string tableName = string.Empty;
            string fields = string.Empty;
            int pageSize = 10;
            int current = 1;
            //#1.trans where
            string sqlWhere = string.Empty;

            //#2.trans order
            string sqlOrder = string.Empty;



            string sql = $"select {fields} from {tableName} order by {sqlOrder} offset {pageSize * current} row fetch next {pageSize} rows only";

            return null;
        }

        /// <summary>
        /// 当前单表分页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Tuple<int, IEnumerable<T>> GetPage(Page page, object where = null, params Expression<Func<T, object>>[] order)
        {
            if (page.PageIndex == 1) page.PageIndex = 0;
            else if (page.PageIndex > 1) page.PageIndex -= 1;
            //#1.trans where
            string sqlWhere = string.Empty;
            if (where == null) sqlWhere = "1=1";
            else sqlWhere = where.WhereToSQL();

            //#2.trans order
            string sqlOrder = order.OrderToSQL();

            //#3.struct sql query
            string sql = $"select * from {typeof(T).Name} where {sqlWhere} order by {sqlOrder} offset {page.PageSize * page.PageIndex} row fetch next {page.PageSize} rows only";

            //#4.return result

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">必须>=1</param>
        /// <param name="sqlWhere"></param>
        /// <returns>int:总条数,IEnumerable<T>数据</returns>
        public Tuple<int, int, int, IEnumerable<T>> GetPage(int pageIndex, int pageSize, string sqlOrder, string sqlWhere)
        {
            if (sqlWhere == null)
                sqlWhere = "1=1";
            if (sqlOrder == null)
                sqlOrder = $"{GetTableKey()} DESC";

            string sql = sql = $"select * from {GetTableName()} where {sqlWhere} order by {sqlOrder} offset {pageSize * (pageIndex - 1)} row fetch next {pageSize} rows only;";

            string sqlCount = $"select COUNT(*) from  {GetTableName()} where {sqlWhere}";
            if (Db.State == ConnectionState.Closed)
                Db.Open();
            IEnumerable<T> dt = Db.Query<T>(sql);
            int count = Db.ExecuteScalar<int>(sqlCount);
            return new Tuple<int, int, int, IEnumerable<T>>(pageIndex, pageSize, count, dt);
        }


        public bool Update(T entity)
        {
            using (var trans = Db.BeginTransaction())
            {
                bool result = Db.Update(entity, trans);
                if (result) trans.Commit();
                else trans.Rollback();
                return result;
            }
        }

        /// <summary>
        /// 更新需要更新的属性
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public bool Update(params Expression<Func<T, object>>[] fields)
        {
            // 获取ID
            var id = typeof(T).GetProperties().Where(p => p.IsKey()).Single().GetValue(fields, null);
            T o = Db.Get<T>(id);

            // 获取需要变动的属性名称
            List<PropertyInfo> propIn = fields.Select(p => p.ToPropertyInfo()).ToList();
            // 遍历更改属性
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                var filed = propIn.Where(p => p.Name == property.Name).SingleOrDefault();
                if (filed != null)
                {
                    var value = typeof(T).GetProperty(property.Name).GetValue(fields);
                    PropertyInfo f_info = o.GetType().GetProperty(property.Name);
                    if (f_info == null) throw new Exception("null");
                    var f_value = f_info.GetValue(o, null);
                    f_info.SetValue(o, value, null);
                }
            }
            return Update(o);
        }


        /// <summary>
        /// 判断指定数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public bool Exists(T t, params Expression<Func<T, object>>[] fields)
        {
            string sql = "SELECT COUNT(0) FROM {1} WHERE {2}";
            string parms = string.Empty, name = string.Empty;
            List<PropertyInfo> propIn = fields.Select(p => p.ToPropertyInfo()).ToList();
            foreach (PropertyInfo property in propIn)
            {
                name = property.Name;
                var filed = propIn.Where(p => p.Name == property.Name).SingleOrDefault();
                if (filed != null)
                {
                    var value = typeof(T).GetProperty(property.Name).GetValue(t);
                    parms += string.Concat(" ", property.Name, "='", value, "' AND ");
                }
            }
            parms = parms.RemoveLastIndexOf("AND");
            sql = string.Format(sql, name, GetTableName(), parms);
            return (int)Db.ExecuteScalar(sql) > 0;
        }


        /// <summary>
        /// 使用Dapper的查询
        /// </summary>
        /// <param name="t"></param>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public bool Exists(T t, object obj, params Expression<Func<T, object>>[] fields)
        {
            string sql = "SELECT COUNT(0) FROM {1} WHERE {2}";
            string parms = string.Empty, name = string.Empty;
            List<PropertyInfo> propIn = fields.Select(p => p.ToPropertyInfo()).ToList();
            foreach (PropertyInfo property in propIn)
            {
                name = property.Name;
                var filed = propIn.Where(p => p.Name == property.Name).SingleOrDefault();
                if (filed != null)
                {
                    var value = typeof(T).GetProperty(property.Name).GetValue(t);
                    parms += string.Concat(" ", property.Name, "=@", property.Name, " AND ");
                }
            }
            parms = parms.RemoveLastIndexOf("AND");
            sql = string.Format(sql, name, GetTableName(), parms);
            return (int)Db.ExecuteScalar(sql, obj) > 0;
        }

        #endregion




        #region Internal

        internal string GetTableName()
        {
            return ((TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), false)[0]).Name;
        }

        internal string GetTableKey()
        {
            foreach (var item in typeof(T).GetProperties())
            {
                if (item.GetCustomAttributes(typeof(KeyAttribute), false) != null)
                    return item.Name;
            }
            return null;
        }

        #endregion



    }

}
