using Good.Admin.Common.DataAccess;
using Good.Admin.Common.DI;
using Good.Admin.Common.Helper;
using Good.Admin.Common;
using Good.Admin.Common;
using SqlSugar;
using System.Linq.Expressions;

namespace Good.Admin.Repository
{
    //IBaseRepository<T>
    //SimpleClient<T> where T : class, new()
    //: IBaseRepository<T> where T : class, new()
    public class BaseRepository<T> : ISingletonDependency, IBaseRepository<T> where T : class, new()
    {
        private readonly SqlSugarScope _dbBase;
        private readonly IUnitOfWork _unitOfWork;

        public ISqlSugarClient Db => _db;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbBase = unitOfWork.GetDbClient();
        }

        private ISqlSugarClient _db {
            get {
                /* 如果要开启多库支持，
                 * 1、在appsettings.json 中开启MutiDBEnabled节点为true，必填
                 * 2、设置一个主连接的数据库ID，节点MainDB，对应的连接字符串的Enabled也必须true，必填
                 */
                if (_dbBase != null)
                {
                    return _dbBase;
                }

                if (Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
                {
                    if (typeof(T).GetTypeInfo().GetCustomAttributes(typeof(SugarTable), true).FirstOrDefault((x => x.GetType() == typeof(SugarTable))) is SugarTable sugarTable && !string.IsNullOrEmpty(sugarTable.TableDescription))
                    {
                        _dbBase.ChangeDatabase(sugarTable.TableDescription.ToLower());
                    }
                    else
                    {
                        _dbBase.ChangeDatabase(MainDb.CurrentDbConnId.ToLower());
                    }
                }
                var reslt = _dbBase.CopyNew().Ado.IsValidConnection();

                return _dbBase;
            }
        }
        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体</returns>
        public T QueryById(object pkValue, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().With(SqlWith.NoLock).InSingle(pkValue)
                : _db.Queryable<T>().InSingle(pkValue);
        }

        /// <summary>
        ///     根据主值查询单条数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>数据实体</returns>
        public async Task<T> QueryByIdAsync(object objId, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().In(objId).With(SqlWith.NoLock).SingleAsync()
                : await _db.Queryable<T>().In(objId).SingleAsync();
        }

        /// <summary>
        ///     根据主值列表查询单条数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        public List<T> QueryByIDs(object[] lstIds, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToList()
                : _db.Queryable<T>().In(lstIds).ToList();
        }

        /// <summary>
        ///     根据主值列表查询单条数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        public async Task<List<T>> QueryByIDsAsync(object[] lstIds, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToListAsync()
                : await _db.Queryable<T>().In(lstIds).ToListAsync();
        }

        /// <summary>
        ///     根据主值列表查询单条数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        public List<T> QueryByIDs(int[] lstIds, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToList()
                : _db.Queryable<T>().In(lstIds).ToList();
        }

        /// <summary>
        ///     根据主值列表查询单条数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        public async Task<List<T>> QueryByIDsAsync(int[] lstIds, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().In(lstIds).With(SqlWith.NoLock).ToListAsync()
                : await _db.Queryable<T>().In(lstIds).ToListAsync();
        }

        /// <summary>
        ///     查询表单所有数据(无分页,请慎用)
        /// </summary>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public List<T> Query(bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().With(SqlWith.NoLock).ToList()
                : _db.Queryable<T>().ToList();
        }

        /// <summary>
        ///     查询表单所有数据(无分页,请慎用)
        /// </summary>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync(bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().With(SqlWith.NoLock).ToListAsync()
                : await _db.Queryable<T>().ToListAsync();
        }

        /// <summary>
        ///  根据条件查询数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="orderBy">排序字段，如name asc,age desc</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        public List<T> QueryListByClause(string strWhere, string orderBy = "", bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).With(SqlWith.NoLock).ToList()
                : _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList();
        }

        /// <summary>
        /// 根据条件查询-分页
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="orderBy">排序字段，如name asc,age desc</param>
        /// <param name="pagenumber">页码</param>
        /// <param name="pagesize">每页数量</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public PageResult<T> QueryPageListByClause(string strWhere, string orderBy = "", int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false)
        {
            int total = 0;
            var result_data = blUseNoLock
                ? _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).With(SqlWith.NoLock).ToPageList(pageIndex, pagesize, ref total)
                : _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                     .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageList(pageIndex, pagesize, ref total);
            return new PageResult<T>(pageIndex, total, pagesize, result_data);
        }

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="orderBy">排序字段，如name asc,age desc</param>
        /// <returns>泛型实体集合</returns>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        public async Task<List<T>> QueryListByClauseAsync(string strWhere, string orderBy = "",
            bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).With(SqlWith.NoLock).ToListAsync()
                : await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }

        /// <summary>
        /// 根据条件查询-分页
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="orderBy">排序字段，如name asc,age desc</param>
        /// <param name="pagenumber">页码</param>
        /// <param name="pagesize">每页数量</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<PageResult<T>> QueryPageListByClauseAsync(
             Expressionable<T> whereLambda = null,
            string orderBy = "",
            int pageIndex = 1,
            int pagesize = 20,
            bool blUseNoLock = false)
        {
            RefAsync<int> total = 0;

            var result_data = blUseNoLock
                ? await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(whereLambda != null, whereLambda.ToExpression()).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pagesize, total)
                : await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                     .WhereIF(whereLambda != null, whereLambda.ToExpression()).ToPageListAsync(pageIndex, pagesize, total);
            return new PageResult<T>(pageIndex, total.Value, pagesize, result_data);
        }

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序字段，如name asc,age desc</param>
        /// <returns>泛型实体集合</returns>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        public List<T> QueryListByClause(Expression<Func<T, bool>> predicate, string orderBy = "",
            bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToList()
                : _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).ToList();
        }

        /// <summary>
        ///     根据条件查询数据-分页
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序字段，如name asc,age desc</param>
        /// <returns>泛型实体集合</returns>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        public PageResult<T> QueryPageListByClause(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false)
        {
            int total = 0;
            var result_data = blUseNoLock
                ? _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToPageList(pageIndex, pagesize, ref total)
                : _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).ToPageList(pageIndex, pagesize, ref total);
            return new PageResult<T>(pageIndex, total, pagesize, result_data);
        }

        /// <summary>
        /// 根据条件查询单表数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序字段，如name asc,age desc</param>
        /// <returns>泛型实体集合</returns>
        /// <param name="useNoLock">是否使用WITH(NOLOCK)</param>
        public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, string orderBy = "",
            bool useNoLock = false)
        {
            return useNoLock
                ? await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToListAsync()
                : await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).ToListAsync();
        }
        /// <summary>
        /// 根据条件获取指定列得数据
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="selectExpression">查询列</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="useNoLock"></param>
        /// <returns></returns>
        public async Task<List<TResult>> QueryListByClauseAsync<T, TResult>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selectExpression, string orderBy = "",
        bool useNoLock = false)
        {
            return useNoLock
                ? await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).Select(selectExpression).ToListAsync()
                : await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).Select(selectExpression).ToListAsync();
        }
        /// <summary>
        /// 根据条件获取指定列得数据
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="selectExpression">查询列</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="useNoLock"></param>
        /// <returns></returns>
        public List<TResult> QueryListByClause<T, TResult>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selectExpression, string orderBy = "",
        bool useNoLock = false)
        {
            return useNoLock
                ? _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).Select(selectExpression).ToList()
                : _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).Select(selectExpression).ToList();
        }
        /// <summary>
        ///     根据条件查询数据-分页
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序字段，如name asc,age desc</param>
        /// <returns>泛型实体集合</returns>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        public async Task<PageResult<T>> QueryPageListByClauseAsync(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false)
        {

            RefAsync<int> total = 0;
            var result_data = blUseNoLock
                ? await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pagesize, total)
                : await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy)
                    .WhereIF(predicate != null, predicate).ToPageListAsync(pageIndex, pagesize, total);
            return new PageResult<T>(pageIndex, total.Value, pagesize, result_data);
        }

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        public List<T> QueryListByClause(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToList()
                : _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).ToList();
        }

        /// <summary>
        ///     根据条件查询数据-分页
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        public PageResult<T> QueryPageListByClause(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false)
        {
            int total = 0;
            var result_data = blUseNoLock
                ? _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToPageList(pageIndex, pagesize, ref total)
                : _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).ToPageList(pageIndex, pagesize, ref total);
            return new PageResult<T>(pageIndex, total, pagesize, result_data);
        }


        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToListAsync()
                : await _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).ToListAsync();
        }

        /// <summary>
        ///     根据条件查询数据_分页
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        public async Task<PageResult<T>> QueryPageListByClauseAsync(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> total = 0;
            var result_data = blUseNoLock
                ? await _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pagesize, total)
                : await _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).ToPageListAsync(pageIndex, pagesize, total);
            return new PageResult<T>(pageIndex, total, pagesize, result_data);
        }

        /// <summary>
        ///     根据条件查询一定数量数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="take">获取数量</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).Take(take).With(SqlWith.NoLock).ToList()
                : _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).Take(take).ToList();
        }

        /// <summary>
        ///     根据条件查询一定数量数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="take">获取数量</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).Take(take).With(SqlWith.NoLock).ToListAsync()
                : await _db.Queryable<T>().OrderByIF(orderByPredicate != null, orderByPredicate, orderByType)
                    .WhereIF(predicate != null, predicate).Take(take).ToListAsync();
        }

        /// <summary>
        ///     根据条件查询一定数量数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="take">获取数量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take, string strOrderByFileds = "",
            bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                    .Where(predicate).Take(take).With(SqlWith.NoLock).ToList()
                : _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                    .Where(predicate).Take(take).ToList();
        }

        /// <summary>
        ///     根据条件查询一定数量数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="take">获取数量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take,
            string strOrderByFileds = "", bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                    .Where(predicate).Take(take).With(SqlWith.NoLock).ToListAsync()
                : await _db.Queryable<T>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                    .Where(predicate).Take(take).ToListAsync();
        }

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public T QueryByClause(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().With(SqlWith.NoLock).First(predicate)
                : _db.Queryable<T>().First(predicate);
        }

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>     
        public async Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
        {
            var rr = _db.CopyNew().Ado.IsValidConnection();

            return blUseNoLock
                ? await _db.Queryable<T>().With(SqlWith.NoLock).FirstAsync(predicate)
                : await _db.Queryable<T>().FirstAsync(predicate);
        }

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public T QueryByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate,
            OrderByType orderByType, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().OrderBy(orderByPredicate, orderByType).With(SqlWith.NoLock).First(predicate)
                : _db.Queryable<T>().OrderBy(orderByPredicate, orderByType).First(predicate);
        }

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().OrderBy(orderByPredicate, orderByType).With(SqlWith.NoLock)
                    .FirstAsync(predicate)
                : await _db.Queryable<T>().OrderBy(orderByPredicate, orderByType).FirstAsync(predicate);
        }

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public int Insert(T entity)
        {
            return _db.Insertable(entity).ExecuteReturnIdentity();
        }

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<int> InsertAsync(T entity)
        {
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <param name="insertColumns">插入的列</param>
        /// <returns></returns>
        public int Insert(T entity, Expression<Func<T, object>> insertColumns = null)
        {
            var insert = _db.Insertable(entity);
            if (insertColumns == null)
                return insert.ExecuteReturnIdentity();
            return insert.InsertColumns(insertColumns).ExecuteReturnIdentity();
        }

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <param name="insertColumns">插入的列</param>
        /// <returns></returns>
        public async Task<int> InsertAsync(T entity, Expression<Func<T, object>> insertColumns = null)
        {
            var insert = _db.Insertable(entity);
            if (insertColumns == null)
                return await insert.ExecuteReturnIdentityAsync();
            return await insert.InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">需插入的字段</param>
        /// <returns></returns>
        public bool InsertGuid(T entity, Expression<Func<T, object>> insertColumns = null)
        {
            var insert = _db.Insertable(entity);
            if (insertColumns == null)
                return insert.ExecuteCommand() > 0;
            return insert.InsertColumns(insertColumns).ExecuteCommand() > 0;
        }

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">需插入的字段</param>
        /// <returns></returns>
        public async Task<bool> InsertGuidAsync(T entity, Expression<Func<T, object>> insertColumns = null)
        {
            var insert = _db.Insertable(entity);
            if (insertColumns == null)
                return await insert.ExecuteCommandAsync() > 0;
            return await insert.InsertColumns(insertColumns).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        ///     批量写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public int Insert(List<T> entity)
        {
            return _db.Insertable(entity.ToArray()).ExecuteReturnIdentity();
        }

        /// <summary>
        ///     批量写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<int> InsertAsync(List<T> entity)
        {
            return await _db.Insertable(entity.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        ///     批量写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<int> InsertCommandAsync(List<T> entity)
        {
            return await _db.Insertable(entity.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        ///     批量更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(List<T> entity)
        {
            return _db.Updateable(entity).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     批量更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(List<T> entity)
        {
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            return _db.Updateable(entity).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        /// 更新实体数据忽略空数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateIgnoreNullAsync(T entity)
        {
            return await _db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     根据手写条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public bool Update(T entity, string strWhere)
        {
            return _db.Updateable(entity).Where(strWhere).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     根据手写条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T entity, string strWhere)
        {
            return await _db.Updateable(entity).Where(strWhere).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     根据手写sql语句更新数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool Update(string strSql, SugarParameter[] parameters = null)
        {
            return _db.Ado.ExecuteCommand(strSql, parameters) > 0;
        }

        /// <summary>
        ///     根据手写sql语句更新数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.ExecuteCommandAsync(strSql, parameters) > 0;
        }

        /// <summary>
        ///     更新某个字段
        /// </summary>
        /// <param name="columns">lamdba表达式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
        /// <param name="where">lamdba判断</param>
        /// <returns></returns>
        public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where)
        {
            var i = _db.Updateable<T>().SetColumns(columns).Where(where).ExecuteCommand();
            return i > 0;
        }

        /// <summary>
        ///     更新某个字段
        /// </summary>
        /// <param name="columns">lamdba表达式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
        /// <param name="where">lamdba判断</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where)
        {
            return await _db.Updateable<T>().SetColumns(columns).Where(where).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     根据条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns"></param>
        /// <param name="lstIgnoreColumns"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T entity, List<string> lstColumns = null,
            List<string> lstIgnoreColumns = null, string strWhere = "")
        {
            var up = _db.Updateable(entity);
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
            }

            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = up.UpdateColumns(lstColumns.ToArray());
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = up.Where(strWhere);
            }
            return await up.ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     根据条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns"></param>
        /// <param name="lstIgnoreColumns"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public bool Update(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null,
            string strWhere = "")
        {
            var up = _db.Updateable(entity);
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
                up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
            if (lstColumns != null && lstColumns.Count > 0) up = up.UpdateColumns(lstColumns.ToArray());
            if (!string.IsNullOrEmpty(strWhere)) up = up.Where(strWhere);
            return up.ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            return _db.Deleteable(entity).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public bool Delete(IEnumerable<T> entity)
        {
            return _db.Deleteable<T>(entity).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(IEnumerable<T> entity)
        {
            return await _db.Deleteable<T>(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> where)
        {
            return _db.Deleteable(where).ExecuteCommandHasChange();
        }
        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return await _db.Deleteable(where).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(object id)
        {
            return _db.Deleteable<T>(id).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(object id)
        {
            return await _db.Deleteable<T>(id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(int[] ids)
        {
            return _db.Deleteable<T>().In(ids).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(int[] ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(long[] ids)
        {
            return _db.Deleteable<T>().In(ids).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(long[] ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
        }


        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(Guid[] ids)
        {
            return _db.Deleteable<T>().In(ids).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(Guid[] ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
        }


        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(string[] ids)
        {
            return _db.Deleteable<T>().In(ids).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(string[] ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
        }


        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(List<int> ids)
        {
            return _db.Deleteable<T>().In(ids).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(List<int> ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(List<string> ids)
        {
            return _db.Deleteable<T>().In(ids).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(List<string> ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(List<Guid> ids)
        {
            return _db.Deleteable<T>().In(ids).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(List<Guid> ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(List<long> ids)
        {
            return _db.Deleteable<T>().In(ids).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(List<long> ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
        }


        /// <summary>
        ///     判断数据是否存在
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().Where(predicate).With(SqlWith.NoLock).Any()
                : _db.Queryable<T>().Where(predicate).Any();
        }

        /// <summary>
        ///     判断数据是否存在
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().Where(predicate).With(SqlWith.NoLock).AnyAsync()
                : await _db.Queryable<T>().Where(predicate).AnyAsync();
        }

        /// <summary>
        ///     获取数据总数
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().With(SqlWith.NoLock).Count(predicate)
                : _db.Queryable<T>().Count(predicate);
        }

        /// <summary>
        ///     获取数据总数
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().With(SqlWith.NoLock).CountAsync(predicate)
                : await _db.Queryable<T>().CountAsync(predicate);
        }

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public int GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field, bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().Where(predicate).With(SqlWith.NoLock).Sum(field)
                : _db.Queryable<T>().Where(predicate).Sum(field);
        }

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<int> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field,
            bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().Where(predicate).With(SqlWith.NoLock).SumAsync(field)
                : await _db.Queryable<T>().Where(predicate).SumAsync(field);
        }

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public decimal GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
            bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().Where(predicate).With(SqlWith.NoLock).Sum(field)
                : _db.Queryable<T>().Where(predicate).Sum(field);
        }

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<decimal> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
            bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().Where(predicate).With(SqlWith.NoLock).SumAsync(field)
                : await _db.Queryable<T>().Where(predicate).SumAsync(field);
        }

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public float GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field,
            bool blUseNoLock = false)
        {
            return blUseNoLock
                ? _db.Queryable<T>().Where(predicate).With(SqlWith.NoLock).Sum(field)
                : _db.Queryable<T>().Where(predicate).Sum(field);
        }

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<float> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field,
            bool blUseNoLock = false)
        {
            return blUseNoLock
                ? await _db.Queryable<T>().Where(predicate).With(SqlWith.NoLock).SumAsync(field)
                : await _db.Queryable<T>().Where(predicate).SumAsync(field);
        }
        /// <summary>
        ///     查询-2表查询
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
           Expressionable<T1, T2> whereLambda = null,
            bool blUseNoLock = false) where T1 : class, new() where T2 : class, new()
        {
            if (whereLambda == null)
                return blUseNoLock
                    ? _db.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock).ToList()
                    : _db.Queryable(joinExpression).Select(selectExpression).ToList();
            return blUseNoLock
                ? _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression).With(SqlWith.NoLock)
                    .ToList()
                : _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression).ToList();
        }

        /// <summary>
        ///  2表连接查询
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        public async Task<List<TResult>> QueryMuchAsync<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
           Expressionable<T1, T2> whereLambda = null,
            bool blUseNoLock = false) where T1 : class, new() where T2 : class, new()
        {
            if (whereLambda == null)
                return blUseNoLock
                    ? await _db.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock)
                        .ToListAsync()
                    : await _db.Queryable(joinExpression).Select(selectExpression).ToListAsync();
            return blUseNoLock
                ? await _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression)
                    .With(SqlWith.NoLock).ToListAsync()
                : await _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression)
                .ToListAsync();
        }
        /// <summary>
        /// 两表连接-分页
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="joinExpression">连接条件</param>
        /// <param name="selectExpression">返回值</param>
        /// <param name="whereLambda">where 条件</param>
        /// <param name="blUseNoLock"></param>
        /// <param name="pagenumber">页码</param>
        /// <param name="pagesize">单页行数</param>
        /// <returns></returns>
        public async Task<PageResult<TResult>> QueryMuchPageAsync<T1, T2, TResult>(
           Expression<Func<T1, T2, object[]>> joinExpression,
           Expression<Func<T1, T2, TResult>> selectExpression,
           Expressionable<T1, T2> whereLambda = null,
           int pagenumber = 1,
           int pagesize = 20,
           bool blUseNoLock = false
           ) where T1 : class, new() where T2 : class, new()
        {
            RefAsync<int> total = 0;


            List<TResult> result_data = new List<TResult>();

            if (whereLambda == null)
            {
                if (blUseNoLock)
                {
                    result_data = await _db.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock)
                      .ToPageListAsync(pagenumber, pagesize, total);
                }
                else
                {
                    result_data = await _db.Queryable(joinExpression).Select(selectExpression).ToPageListAsync(pagenumber, pagesize, total);
                }
            }
            else
            {
                if (blUseNoLock)
                {
                    result_data = await _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression)
                          .With(SqlWith.NoLock).ToPageListAsync(pagenumber, pagesize, total);
                }
                else
                {
                    result_data = await _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression).ToPageListAsync(pagenumber, pagesize, total);
                }
            }
            return new PageResult<TResult>(pagenumber, total.Value, pagesize, result_data);
        }
        /// <summary>
        ///     查询-二表查询
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        public TResult QueryMuchFirst<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
         Expressionable<T1, T2> whereLambda = null,
            bool blUseNoLock = false) where T1 : class, new() where T2 : class, new()
        {
            if (whereLambda == null)
                return blUseNoLock
                    ? _db.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock).First()
                    : _db.Queryable(joinExpression).Select(selectExpression).First();
            return blUseNoLock
                ? _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression).With(SqlWith.NoLock)
                    .First()
                : _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression).First();
        }

        /// <summary>
        ///     查询-二表查询
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        public async Task<TResult> QueryMuchFirstAsync<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
           Expressionable<T1, T2> whereLambda = null,
            bool blUseNoLock = false) where T1 : class, new() where T2 : class, new()
        {
            if (whereLambda == null)
                return blUseNoLock
                    ? await _db.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock)
                        .FirstAsync()
                    : await _db.Queryable(joinExpression).Select(selectExpression).FirstAsync();
            return blUseNoLock
                ? await _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression)
                    .With(SqlWith.NoLock).FirstAsync()
                : await _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression).FirstAsync();
        }

        /// <summary>
        ///     查询-三表查询
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T1, T2, T3, TResult>(
            Expression<Func<T1, T2, T3, object[]>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expressionable<T1, T2, T3> whereLambda = null,
            bool blUseNoLock = false) where T1 : class, new() where T2 : class, new() where T3 : class, new()
        {
            if (whereLambda == null)
                return blUseNoLock
                    ? _db.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock)
                        .ToList()
                    : _db.Queryable(joinExpression).Select(selectExpression).ToList();
            return blUseNoLock
                ? _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression).With(SqlWith.NoLock)
                    .ToList()
                : _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression)
                    .ToList();
        }

        /// <summary>
        ///     查询-三表查询
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        public async Task<List<TResult>> QueryMuchAsync<T1, T2, T3, TResult>(
            Expression<Func<T1, T2, T3, object[]>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
    Expressionable<T1, T2, T3> whereLambda = null,
            bool blUseNoLock = false) where T1 : class, new() where T2 : class, new() where T3 : class, new()
        {
            if (whereLambda == null)
                return blUseNoLock
                    ? await _db.Queryable(joinExpression).Select(selectExpression).With(SqlWith.NoLock)
                        .ToListAsync()
                    : await _db.Queryable(joinExpression).Select(selectExpression).ToListAsync();
            return blUseNoLock
                ? await _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression)
                    .With(SqlWith.NoLock)
                    .ToListAsync()
                : await _db.Queryable(joinExpression).Where(whereLambda.ToExpression()).Select(selectExpression)
                    .ToListAsync();
        }

        /// <summary>
        ///     执行sql语句并返回List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> SqlQuery(string sql, List<SugarParameter> parameters)
        {
            var list = _db.Ado.SqlQuery<T>(sql, parameters);
            return list;
        }

        /// <summary>
        ///     执行sql语句并返回List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<T>> SqlQueryable(string sql)
        {
            var list = await _db.SqlQueryable<T>(sql).ToListAsync();
            return list;
        }
    }


}
