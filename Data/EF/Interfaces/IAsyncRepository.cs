using Business;
using Common;
using System.Linq.Expressions;

namespace Data {
    public interface IAsyncRepository<T> where T : RootEntity {
        Task<T> AddAsync(T entity);

        EFContext getDbContext();

        Task<List<T>> AddRangeAsync(List<T> entities);

        Task<T> UpdateAsync(T entity);
        Task<T> ConcurrencyUpdateAsync(byte[]? rowVersion, T entity);
        void ConcurrencyCheck(DTObase updateRequest, T entity);
        Task<bool> DeleteAsync(T entity);

        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        Task<bool> CheckIfAnyExistAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> ListAsync();
        Expression<Func<T, bool>>? GetExactEqualsExpression<T1>(string propertyName, T1? filter);
        Expression<Func<T, bool>>? GetNumberCompareExpression<T1>(string propertyName, T1? filter, ExpressionType expressionType);
        Expression<Func<T, bool>>? GetDateTimeCompareExpression(string propertyName, DateTime? from, DateTime? to);
        Expression<Func<T, bool>>? GetStringLikeExpression(string propertyName, string? filter, string type = FilteringConstants.LikeExpressionType.Contain);
        Expression<Func<T, bool>>? GetStringLikeOrLogicExpression(string propertyName, string[]? filters, string type = FilteringConstants.LikeExpressionType.Contain);
        Expression<Func<T, bool>>? GetArrayContainsExpression<T1>(string propertyName, T1?[]? filters);
        Task<int> ListCountAsync(Expression<Func<T, bool>> expression);
        Task<int> ListCountAsync(List<Expression<Func<T, bool>>?> expressions);
        Task<int> ListCountAsync(Expression<Func<T, bool>> serverEvalExpression, Func<T, bool>? clientEvalExpression);
        Task<int> ListCountAsync(List<Expression<Func<T, bool>>?> serverEvalExpressions, Func<T, bool>? clientEvalExpression);
        Task<List<T>> ListAsyncByExpandingSortingPaging(List<Expression<Func<T, bool>>?> expressions, Func<T, bool>? clientEvalExpression, List<string>? expandExpressions, List<SortingParameter>? sortingExpressions, int? offset, int? limit);
        Task<List<T>> ListAsyncByPagging(Expression<Func<T, bool>> expression, int pageSize, int pageNo);
        IQueryable<T> ListAsyncByPaggingQueryable(Expression<Func<T, bool>> expression, int pageSize, int pageNo);
        Task<IQueryable<T>> ListAsyncByExpandingQueryable(List<Expression<Func<T, bool>>?> expressions, List<string>? expandExpressions, IQueryable<T>? queryableObject = null);
    }
}
