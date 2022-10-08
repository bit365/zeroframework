using System.Linq.Expressions;

namespace ZeroFramework.DeviceCenter.Domain.Repositories
{
    public interface IAsyncQueryableProvider
    {
        bool CanExecute<T>(IQueryable<T> queryable);

        #region Contains

        Task<bool> ContainsAsync<T>(IQueryable<T> queryable, T item, CancellationToken cancellationToken = default);

        #endregion

        #region Any/All

        Task<bool> AnyAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> AllAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion

        #region Count/LongCount

        Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<int> CountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<long> LongCountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<long> LongCountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion

        #region First/FirstOrDefault

        Task<T> FirstAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<T> FirstAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion

        #region Last/LastOrDefault

        Task<T> LastAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<T> LastAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T?> LastOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<T?> LastOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion

        #region Single/SingleOrDefault

        Task<T> SingleAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<T> SingleAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion

        #region Min

        Task<T> MinAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<TResult> MinAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);

        #endregion

        #region Max

        Task<T> MaxAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<TResult> MaxAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);

        #endregion

        #region Sum

        Task<decimal> SumAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default);

        Task<decimal?> SumAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default);

        Task<decimal> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default);

        Task<decimal?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default);

        Task<int> SumAsync(IQueryable<int> source, CancellationToken cancellationToken = default);

        Task<int?> SumAsync(IQueryable<int?> source, CancellationToken cancellationToken = default);

        Task<int> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default);

        Task<int?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default);

        Task<long> SumAsync(IQueryable<long> source, CancellationToken cancellationToken = default);

        Task<long?> SumAsync(IQueryable<long?> source, CancellationToken cancellationToken = default);

        Task<long> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default);

        Task<long?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default);

        Task<double> SumAsync(IQueryable<double> source, CancellationToken cancellationToken = default);

        Task<double?> SumAsync(IQueryable<double?> source, CancellationToken cancellationToken = default);

        Task<double> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default);

        Task<double?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default);

        Task<float> SumAsync(IQueryable<float> source, CancellationToken cancellationToken = default);

        Task<float?> SumAsync(IQueryable<float?> source, CancellationToken cancellationToken = default);

        Task<float> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default);

        Task<float?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default);

        #endregion

        #region Average

        Task<decimal> AverageAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default);

        Task<decimal?> AverageAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default);

        Task<decimal> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default);

        Task<decimal?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default);

        Task<double> AverageAsync(IQueryable<int> source, CancellationToken cancellationToken = default);

        Task<double?> AverageAsync(IQueryable<int?> source, CancellationToken cancellationToken = default);

        Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default);

        Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default);

        Task<double> AverageAsync(IQueryable<long> source, CancellationToken cancellationToken = default);

        Task<double?> AverageAsync(IQueryable<long?> source, CancellationToken cancellationToken = default);

        Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default);

        Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default);

        Task<double> AverageAsync(IQueryable<double> source, CancellationToken cancellationToken = default);

        Task<double?> AverageAsync(IQueryable<double?> source, CancellationToken cancellationToken = default);

        Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default);

        Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default);

        Task<float> AverageAsync(IQueryable<float> source, CancellationToken cancellationToken = default);

        Task<float?> AverageAsync(IQueryable<float?> source, CancellationToken cancellationToken = default);

        Task<float> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default);

        Task<float?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default);

        #endregion

        #region ToList/Array

        Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

        #endregion
    }
}
