using System.Linq.Expressions;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Repositories
{
    public static class RepositoryAsyncExtensions
    {
        #region Contains

        public static Task<bool> ContainsAsync<T>(this IRepository<T> repository, T item, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.ContainsAsync(repository.Query, item, cancellationToken);
        }

        #endregion

        #region Any/All

        public static Task<bool> AnyAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AnyAsync(repository.Query, predicate, cancellationToken);
        }

        public static Task<bool> AllAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AllAsync(repository.Query, predicate, cancellationToken);
        }

        #endregion

        #region Count/LongCount

        public static Task<int> CountAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.CountAsync(repository.Query, cancellationToken);
        }

        public static Task<int> CountAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.CountAsync(repository.Query, predicate, cancellationToken);
        }

        public static Task<long> LongCountAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.LongCountAsync(repository.Query, cancellationToken);
        }

        public static Task<long> LongCountAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.LongCountAsync(repository.Query, predicate, cancellationToken);
        }

        #endregion

        #region First/FirstOrDefault

        public static Task<T> FirstAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.FirstAsync(repository.Query, cancellationToken);
        }

        public static Task<T> FirstAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.FirstAsync(repository.Query, predicate, cancellationToken);
        }

        public static Task<T?> FirstOrDefaultAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.FirstOrDefaultAsync(repository.Query, cancellationToken);
        }

        public static Task<T?> FirstOrDefaultAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.FirstOrDefaultAsync(repository.Query, predicate, cancellationToken);
        }

        #endregion

        #region Last/LastOrDefault

        public static Task<T> LastAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.LastAsync(repository.Query, cancellationToken);
        }

        public static Task<T> LastAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.LastAsync(repository.Query, predicate, cancellationToken);
        }

        public static Task<T?> LastOrDefaultAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.LastOrDefaultAsync(repository.Query, cancellationToken);
        }

        public static Task<T?> LastOrDefaultAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.LastOrDefaultAsync(repository.Query, predicate, cancellationToken);
        }

        #endregion

        #region Single/SingleOrDefault

        public static Task<T> SingleAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SingleAsync(repository.Query, cancellationToken);
        }

        public static Task<T> SingleAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SingleAsync(repository.Query, predicate, cancellationToken);
        }

        public static Task<T?> SingleOrDefaultAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SingleOrDefaultAsync(repository.Query, cancellationToken);
        }

        public static Task<T?> SingleOrDefaultAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SingleOrDefaultAsync(repository.Query, predicate, cancellationToken);
        }

        #endregion

        #region Min

        public static Task<T> MinAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.MinAsync(repository.Query, cancellationToken);
        }

        public static Task<TResult> MinAsync<T, TResult>(this IRepository<T> repository, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.MinAsync(repository.Query, selector, cancellationToken);
        }

        #endregion

        #region Max

        public static Task<T> MaxAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.MaxAsync(repository.Query, cancellationToken);
        }

        public static Task<TResult> MaxAsync<T, TResult>(this IRepository<T> repository, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.MaxAsync(repository.Query, selector, cancellationToken);
        }

        #endregion

        #region Sum

        public static Task<decimal> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<decimal?> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<int> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<int?> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<long> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<long?> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<double> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<double?> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<float> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<float?> SumAsync<T>(this IRepository<T> repository, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
        }

        #endregion

        #region Average

        public static Task<decimal> AverageAsync<T>(this IRepository<T> repository, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<decimal?> AverageAsync<T>(this IRepository<T> repository, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<double> AverageAsync<T>(this IRepository<T> repository, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<T>(this IRepository<T> repository, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<double> AverageAsync<T>(this IRepository<T> repository, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<T>(this IRepository<T> repository, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<double> AverageAsync<T>(this IRepository<T> repository, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<T>(this IRepository<T> repository, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
        }

        public static Task<float?> AverageAsync<T>(this IRepository<T> repository, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
        }

        #endregion

        #region ToList/Array

        public static Task<List<T>> ToListAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.ToListAsync(repository.Query, cancellationToken);
        }

        public static Task<T[]> ToArrayAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.ToArrayAsync(repository.Query, cancellationToken);
        }

        #endregion
    }
}