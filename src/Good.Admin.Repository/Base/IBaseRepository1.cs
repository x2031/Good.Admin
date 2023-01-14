using Good.Admin.Util;
using SqlSugar;
using System.Linq.Expressions;

namespace Good.Admin.Repository
{
    public interface IBaseRepository1<T> where T : class, new()
    {
        ISqlSugarClient Db { get; }

        bool Delete(Expression<Func<T, bool>> where);
        bool Delete(IEnumerable<T> entity);
        bool Delete(T entity);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> where);
        Task<bool> DeleteAsync(IEnumerable<T> entity);
        Task<bool> DeleteAsync(T entity);
        bool DeleteById(object id);
        Task<bool> DeleteByIdAsync(object id);
        bool DeleteByIds(Guid[] ids);
        bool DeleteByIds(int[] ids);
        bool DeleteByIds(List<Guid> ids);
        bool DeleteByIds(List<int> ids);
        bool DeleteByIds(List<long> ids);
        bool DeleteByIds(List<string> ids);
        bool DeleteByIds(long[] ids);
        bool DeleteByIds(string[] ids);
        Task<bool> DeleteByIdsAsync(Guid[] ids);
        Task<bool> DeleteByIdsAsync(int[] ids);
        Task<bool> DeleteByIdsAsync(List<Guid> ids);
        Task<bool> DeleteByIdsAsync(List<int> ids);
        Task<bool> DeleteByIdsAsync(List<long> ids);
        Task<bool> DeleteByIdsAsync(List<string> ids);
        Task<bool> DeleteByIdsAsync(long[] ids);
        Task<bool> DeleteByIdsAsync(string[] ids);
        bool Exists(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);
        int GetCount(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);
        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);
        decimal GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field, bool blUseNoLock = false);
        float GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field, bool blUseNoLock = false);
        int GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field, bool blUseNoLock = false);
        Task<decimal> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field, bool blUseNoLock = false);
        Task<float> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field, bool blUseNoLock = false);
        Task<int> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field, bool blUseNoLock = false);
        int Insert(List<T> entity);
        int Insert(T entity);
        int Insert(T entity, Expression<Func<T, object>> insertColumns = null);
        Task<int> InsertAsync(List<T> entity);
        Task<int> InsertAsync(T entity);
        Task<int> InsertAsync(T entity, Expression<Func<T, object>> insertColumns = null);
        Task<int> InsertCommandAsync(List<T> entity);
        bool InsertGuid(T entity, Expression<Func<T, object>> insertColumns = null);
        Task<bool> InsertGuidAsync(T entity, Expression<Func<T, object>> insertColumns = null);
        List<T> Query(bool blUseNoLock = false);
        Task<List<T>> QueryAsync(bool blUseNoLock = false);
        T QueryByClause(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);
        T QueryByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);
        Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);
        Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);
        T QueryById(object pkValue, bool blUseNoLock = false);
        Task<T> QueryByIdAsync(object objId, bool blUseNoLock = false);
        List<T> QueryByIDs(int[] lstIds, bool blUseNoLock = false);
        List<T> QueryByIDs(object[] lstIds, bool blUseNoLock = false);
        Task<List<T>> QueryByIDsAsync(int[] lstIds, bool blUseNoLock = false);
        Task<List<T>> QueryByIDsAsync(object[] lstIds, bool blUseNoLock = false);
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, string orderBy = "", bool blUseNoLock = false);
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take, string strOrderByFileds = "", bool blUseNoLock = false);
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);
        List<T> QueryListByClause(string strWhere, string orderBy = "", bool blUseNoLock = false);
        List<TResult> QueryListByClause<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectExpression, string orderBy = "", bool useNoLock = false);
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, string orderBy = "", bool useNoLock = false);
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take, string strOrderByFileds = "", bool blUseNoLock = false);
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);
        Task<List<T>> QueryListByClauseAsync(string strWhere, string orderBy = "", bool blUseNoLock = false);
        Task<List<TResult>> QueryListByClauseAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selectExpression, string orderBy = "", bool useNoLock = false);
        List<TResult> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression, Expressionable<T1, T2, T3> whereLambda = null, bool blUseNoLock = false)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new();
        List<TResult> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expressionable<T1, T2> whereLambda = null, bool blUseNoLock = false)
            where T1 : class, new()
            where T2 : class, new();
        Task<List<TResult>> QueryMuchAsync<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, object[]>> joinExpression, Expression<Func<T1, T2, T3, TResult>> selectExpression, Expressionable<T1, T2, T3> whereLambda = null, bool blUseNoLock = false)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new();
        Task<List<TResult>> QueryMuchAsync<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expressionable<T1, T2> whereLambda = null, bool blUseNoLock = false)
            where T1 : class, new()
            where T2 : class, new();
        TResult QueryMuchFirst<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expressionable<T1, T2> whereLambda = null, bool blUseNoLock = false)
            where T1 : class, new()
            where T2 : class, new();
        Task<TResult> QueryMuchFirstAsync<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expressionable<T1, T2> whereLambda = null, bool blUseNoLock = false)
            where T1 : class, new()
            where T2 : class, new();
        Task<PageResult<TResult>> QueryMuchPageAsync<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression, Expression<Func<T1, T2, TResult>> selectExpression, Expressionable<T1, T2> whereLambda = null, int pagenumber = 1, int pagesize = 20, bool blUseNoLock = false)
            where T1 : class, new()
            where T2 : class, new();
        PageResult<T> QueryPageListByClause(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false);
        PageResult<T> QueryPageListByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false);
        PageResult<T> QueryPageListByClause(string strWhere, string orderBy = "", int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false);
        Task<PageResult<T>> QueryPageListByClauseAsync(Expressionable<T> whereLambda = null, string orderBy = "", int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false);
        Task<PageResult<T>> QueryPageListByClauseAsync(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false);
        Task<PageResult<T>> QueryPageListByClauseAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, int pageIndex = 1, int pagesize = 20, bool blUseNoLock = false);
        List<T> SqlQuery(string sql, List<SugarParameter> parameters);
        Task<List<T>> SqlQueryable(string sql);
        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where);
        bool Update(List<T> entity);
        bool Update(string strSql, SugarParameter[] parameters = null);
        bool Update(T entity);
        bool Update(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");
        bool Update(T entity, string strWhere);
        Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where);
        Task<bool> UpdateAsync(List<T> entity);
        Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateAsync(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");
        Task<bool> UpdateAsync(T entity, string strWhere);
    }
}