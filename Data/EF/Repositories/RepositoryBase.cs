using Business;
using Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Data {
    public class RepositoryBase<T> : IAsyncRepository<T> where T : RootEntity {
        private readonly DbSet<T> _dbSet;
        private readonly EFContext _dbContext;

        public RepositoryBase(EFContext dbContext) {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        public EFContext getDbContext() {
            return _dbContext;
        }

        public async Task<T> AddAsync(T entity) {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities) {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public Task<bool> DeleteAsync(T entity) {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> expression) {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<bool> CheckIfAnyExistAsync(Expression<Func<T, bool>> expression) {
            var itemCount = await ListCountAsync(expression);
            if (itemCount > 0) {
                return true;
            }
            return false;
        }

        internal IQueryable<T> GetExpandedQueryable(List<string>? expandExpressions, IQueryable<T>? queryableObject = null) {
            IQueryable<T> result = _dbSet;
            if (queryableObject != null) {
                result = queryableObject;
            }
            // handle expanding table with related tables
            if (expandExpressions != null) {
                foreach (string expandExpression in expandExpressions) {
                    result = result.Include(expandExpression);
                }
            }
            return result;
        }

        internal IQueryable<T> GetFilteredQueryable(List<Expression<Func<T, bool>>?> expressions, IQueryable<T>? queryableObject = null) {
            IQueryable<T> result = _dbSet;
            if (queryableObject != null) {
                result = queryableObject;
            }
            // expression
            foreach (var expression in expressions) {
                if (expression != null) {
                    result = result.Where(expression);
                }
            }
            return result;
        }

        internal IQueryable<T> GetSortedQueryable(List<SortingParameter>? sortingExpressions, IQueryable<T>? queryableObject = null) {
            IQueryable<T> result = _dbSet;
            if (queryableObject != null) {
                result = queryableObject;
            }
            // handle sorting
            IOrderedQueryable<T>? query = null;
            if (sortingExpressions != null) {
                foreach (SortingParameter sortingExpression in sortingExpressions) {
                    var queryString = sortingExpression.Parameter + (sortingExpression.IsAscending ? $" {SortingConstant.ASC.Code}" : $" {SortingConstant.DESC.Code}");
                    var nullLastQuery = sortingExpression.Parameter + $" {SortingConstant.NULL_LAST.Code}";
                    if (query == null) {
                        query = result.OrderBy(nullLastQuery).ThenBy(queryString);
                    } else {
                        query = query.ThenBy(nullLastQuery).ThenBy(queryString);
                    }
                }
                if (query != null) {
                    result = query;
                }
            } else {
                result = result.OrderBy(SortingConstant.DEFAULT_SORT_FIELD_NAME.Code);
            }
            return result;
        }

        public Task<List<T>> ListAsync(Expression<Func<T, bool>> expression) {
            return _dbSet.Where(expression).ToListAsync();
        }

        public Task<List<T>> ListAsync() {
            return _dbSet.ToListAsync();
        }

        public Task<int> ListCountAsync(Expression<Func<T, bool>> expression) {
            return _dbSet.Where(expression).CountAsync();
        }

        public Task<int> ListCountAsync(List<Expression<Func<T, bool>>?> expressions) {
            IQueryable<T> result = _dbSet;
            result = GetFilteredQueryable(expressions, result);
            return result.CountAsync();
        }

        public async Task<int> ListCountAsync(Expression<Func<T, bool>> serverEvalExpression, Func<T, bool>? clientEvalExpression) {
            if (clientEvalExpression == null) {
                return await _dbSet.Where(serverEvalExpression).CountAsync();
            }
            return (await _dbSet.Where(serverEvalExpression).ToListAsync()).Count(clientEvalExpression);
        }

        public async Task<int> ListCountAsync(List<Expression<Func<T, bool>>?> serverEvalExpressions, Func<T, bool>? clientEvalExpression) {
            IQueryable<T> query = _dbSet;
            query = GetFilteredQueryable(serverEvalExpressions, query);
            if (clientEvalExpression == null) {
                return await query.CountAsync();
            }
            return (await query.ToListAsync()).Count(clientEvalExpression);
        }

        public MemberExpression GetMemberExpression(Expression param, string propertyName) {
            if (propertyName.Contains(FilteringConstants.Separator)) {
                int index = propertyName.IndexOf(FilteringConstants.Separator);
                var subParameter = Expression.Property(param, propertyName.Substring(0, index));
                return GetMemberExpression(subParameter, propertyName.Substring(index + 1));
            }
            return Expression.Property(param, propertyName);
        }

        public Expression<Func<T, bool>>? GetExactEqualsExpression<T1>(string propertyName, T1? filter) {
            var parameter = Expression.Parameter(typeof(T), nameof(T));
            MemberExpression property = GetMemberExpression(parameter, propertyName);

            ConstantExpression? constant = null;
            if (typeof(T1) == typeof(string)) {
                string? filterString = filter as string;
                if (!String.IsNullOrWhiteSpace(filterString)) {
                    constant = Expression.Constant(filter);
                } else {
                    return null;
                }
            } else if (typeof(T1) == typeof(Boolean)) {
                constant = Expression.Constant(filter, typeof(Nullable<Boolean>));
            } else if (filter != null) {
                constant = Expression.Constant(filter);
            } else {
                return null;
            }
            MethodInfo? method = property.Type.GetMethod(FilteringConstants.ExpressionMethod.Equal, new[] { constant.Type });
            if (method == null) {
                throw new InternalException("GetNotNullExactEqualExpression for property " + propertyName + " and " + filter + " not working, please make sure the type of filter is correct");
            }
            var expression = Expression.Lambda<Func<T, bool>>(
                Expression.Call(property, method, constant), parameter
            );
            return expression;
        }

        public Expression<Func<T, bool>>? GetArrayContainsExpression<T1>(string propertyName, T1?[]? filters) {
            var parameter = Expression.Parameter(typeof(T), nameof(T));
            MemberExpression property = GetMemberExpression(parameter, propertyName);

            if (filters == null) {
                return null;
            }
            var filterList = filters.ToList();
            var method = filterList.GetType().GetMethod(FilteringConstants.ExpressionMethod.Contain);
            if (method == null) {
                throw new InternalException("GetArrayContainsExpression for property " + propertyName + " not working, please make sure the type of filter is correct");
            }
            var expression = Expression.Lambda<Func<T, bool>>(
                Expression.Call(Expression.Constant(filterList), method, property), parameter
            );

            return expression;
        }

        public Expression<Func<T, bool>>? GetNumberCompareExpression<T1>(string propertyName, T1? filter, ExpressionType expressionType) {
            var parameter = Expression.Parameter(typeof(T), nameof(T));
            MemberExpression property = GetMemberExpression(parameter, propertyName);

            ConstantExpression? constant = null;
            if (filter != null) {
                constant = Expression.Constant(filter);
            } else {
                return null;
            }

            MethodInfo? method = property.Type.GetMethod(FilteringConstants.ExpressionMethod.CompareTo, new[] { constant.Type });
            if (method == null) {
                throw new InternalException("GetNumberCompareExpression for property " + propertyName + " and " + filter + " not working, please make sure the type of filter is correct");
            }
            var result = Expression.Call(property, method, constant);
            var expression = Expression.Lambda<Func<T, bool>>(
                Expression.MakeBinary(expressionType, result, Expression.Constant(0)), parameter
            );
            return expression;
        }

        public Expression<Func<T, bool>>? GetDateTimeCompareExpression(string propertyName, DateTime? from, DateTime? to) {
            var parameter = Expression.Parameter(typeof(T), nameof(T));
            MemberExpression property = GetMemberExpression(parameter, propertyName);

            if (from != null) {
                from = from.Value.ToLocalTime();
            }
            if (to != null) {
                to = to.Value.ToLocalTime();
            }

            ConstantExpression? fromConstant = Expression.Constant(from, typeof(Nullable<DateTime>));
            ConstantExpression? toConstant = Expression.Constant(to, typeof(Nullable<DateTime>));
            Expression fromExpression = Expression.GreaterThanOrEqual(property, fromConstant);
            Expression toExpression = Expression.LessThanOrEqual(property, toConstant);
            if (from == null && to == null) {
                return null;
            } else if (from != null && to != null) {
                if (from > to) {
                    var errorResponse = new ErrorPayloadResponse<ValidationError>();
                    errorResponse.Append(new ValidationError(FilteringErrorCategories.DateTimeValuesError));
                    throw new FilteringException(errorResponse);
                }
                return Expression.Lambda<Func<T, bool>>(
                    Expression.And(
                        fromExpression,
                        toExpression
                    ), parameter
                );
            } else if (from != null) {
                return Expression.Lambda<Func<T, bool>>(fromExpression, parameter);
            }
            return Expression.Lambda<Func<T, bool>>(toExpression, parameter);
        }

        public Expression<Func<T, bool>>? GetStringLikeExpression(string propertyName, string? filter, string type = FilteringConstants.LikeExpressionType.Contain) {
            var parameter = Expression.Parameter(typeof(T), nameof(T));
            MemberExpression property = GetMemberExpression(parameter, propertyName);
            ConstantExpression? likePattern = null;
            if (filter == null) {
                return null;
            }
            switch (type) {
                case FilteringConstants.LikeExpressionType.Contain:
                    likePattern = Expression.Constant($"%{filter}%");
                    break;
                case FilteringConstants.LikeExpressionType.StartWith:
                    likePattern = Expression.Constant($"{filter}%");
                    break;
                case FilteringConstants.LikeExpressionType.EndWith:
                    likePattern = Expression.Constant($"%{filter}");
                    break;
                default:
                    return null;
            }
            var expression = Expression.Lambda<Func<T, bool>>(
                Expression.Call(
                    typeof(DbFunctionsExtensions), FilteringConstants.ExpressionMethod.Like, Type.EmptyTypes, Expression.Constant(EF.Functions), property, likePattern),
                parameter);
            return expression;
        }

        public Expression<Func<T, bool>>? GetStringLikeOrLogicExpression(string propertyName, string[]? filters, string type = FilteringConstants.LikeExpressionType.Contain) {
            if (filters == null || !filters.Any()) {
                return null;
            }

            var orLogicExpresionAggreateList = new List<Expression<Func<T, bool>>?>();
            foreach (var filter in filters) {
                var expression = this.GetStringLikeExpression(propertyName, filter, type);
                orLogicExpresionAggreateList.Add(expression);
            }

            var exp = orLogicExpresionAggreateList.Aggregate((exp1, exp2) => {
                if (exp1 == null || exp2 == null)
                    return null;
                var invokedExpr = Expression.Invoke(exp2, exp1.Parameters.Cast<Expression>());
                return Expression.Lambda<Func<T, bool>>(Expression.OrElse(exp1.Body, invokedExpr), exp1.Parameters);
            });

            return exp;
        }


        public Task<List<T>> ListAsyncByExpandingSortingPaging(List<Expression<Func<T, bool>>?> expressions, Func<T, bool>? clientEvalExpression, List<string>? expandExpressions, List<SortingParameter>? sortingExpressions, int? pageNo, int? pageSize) {
            // handle limit = 0
            if (pageSize == 0) {
                return Task.FromResult<List<T>>(new List<T>());
            }

            IQueryable<T> result = _dbSet;

            // handle expanding table with related tables
            result = GetExpandedQueryable(expandExpressions, result);

            // expression
            result = GetFilteredQueryable(expressions, result);

            // handle sorting
            result = GetSortedQueryable(sortingExpressions, result);

            // only server side eval require, return directly calling ToListAsync
            if (clientEvalExpression == null) {
                // handle pagination
                result = result.Skip((pageNo - 1) * pageSize ?? 0);

                if (pageSize != null && pageSize != 0) {
                    result = result.Take((int)pageSize);
                }

                return result.AsSplitQuery().ToListAsync();
            }
            return ClientSidePostEval(result.AsSplitQuery().ToListAsync(), clientEvalExpression, pageNo, pageSize);

        }

        private async static Task<List<T>> ClientSidePostEval(Task<List<T>> result, Func<T, bool> clientEvalExpression, int? pageNo, int? pageSize) {
            List<T> serverEvalResult = await result;

            var clientEvalResult = serverEvalResult.Where(clientEvalExpression);

            clientEvalResult = clientEvalResult.Skip((pageNo - 1) * pageSize ?? 0);

            if (pageSize != null && pageSize != 0) {
                clientEvalResult = clientEvalResult.Take((int)pageSize);
            }
            return clientEvalResult.ToList();
        }

        public Task<List<T>> ListAsyncByPagging(Expression<Func<T, bool>> expression, int pageSize, int pageNo) {
            return _dbSet.Where(expression).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public IQueryable<T> ListAsyncByPaggingQueryable(Expression<Func<T, bool>> expression, int pageSize, int pageNo) {
            return _dbSet.Where(expression).Skip((pageNo - 1) * pageSize).Take(pageSize);
        }
        public Task<IQueryable<T>> ListAsyncByExpandingQueryable(List<Expression<Func<T, bool>>?> expressions, List<string>? expandExpressions, IQueryable<T>? queryableObject = null) {
            IQueryable<T> result = GetExpandedQueryable(expandExpressions, queryableObject);
            result = GetFilteredQueryable(expressions, result);
            return Task.FromResult(result);
        }
        public Task<T> UpdateAsync(T entity) {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public Task<T> ConcurrencyUpdateAsync(byte[]? rowVersion, T entity) {
            if (!(entity is IRowVersionContract)) {
                throw new Exception($"Entity ${typeof(T).FullName} does not inherit IRowVersionContract which cannot use ConcurrencyUpdateAsync(byte[] rowVersion, T entity).");
            }

            if (rowVersion is null) {
                string entityName = typeof(T).FullName + "";
                entityName = entityName.Replace(Constants.DB_ENTITY_PREFIX, "");
                var errorResponse = new ErrorPayloadResponse<ConcurrencyUpdateError>();
                var error = new ConcurrencyUpdateError(ConcurrencyUpdateErrorCategories.RowVersionNotProvidedError);
                error.Extra = new Dictionary<string, object>() {
                    {"relatedEntity",entityName}
                };
                errorResponse.Append(error);
                throw new ConcurrencyUpdateException(errorResponse);
            }
            /*
             * must mutate original value rowVersion
             * https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/concurrency?view=aspnetcore-6.0#update-edit-methods
             **/
            _dbContext.Entry(entity).OriginalValues["RowVersion"] = rowVersion;
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public virtual void ConcurrencyCheck(DTObase updateRequest, T entity) {
            string entityName = typeof(T).FullName + "";
            entityName = entityName.Replace(Constants.DB_ENTITY_PREFIX, "");
            if (updateRequest.RowVersion is null) {
                var errorResponse = new ErrorPayloadResponse<ConcurrencyUpdateError>();
                var error = new ConcurrencyUpdateError(ConcurrencyUpdateErrorCategories.RowVersionNotProvidedError);
                error.Extra = new Dictionary<string, object>() {
                    {"relatedEntity",entityName}
                };
                errorResponse.Append(error);
                throw new ConcurrencyUpdateException(errorResponse);
            }
            if (entity is IRowVersionContract updateEntity && updateRequest.RowVersion != null && !updateEntity.RowVersion.SequenceEqual(updateRequest.RowVersion)) {
                var rowVersionErrorResponse = new ErrorPayloadResponse<ConcurrencyUpdateError>();
                var error = new ConcurrencyUpdateError(ConcurrencyUpdateErrorCategories.RowVersionConflictError);
                error.Extra = new Dictionary<string, object>() {
                    {"relatedEntity",entityName}
                };
                rowVersionErrorResponse.Append(error);
                throw new RowVersionConflictException(rowVersionErrorResponse);
            }
        }
    }
}
