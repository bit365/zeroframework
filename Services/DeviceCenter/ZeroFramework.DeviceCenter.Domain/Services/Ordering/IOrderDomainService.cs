using System.Linq.Expressions;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Services.Ordering
{
    public interface IOrderDomainService
    {
        Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Order>> ListAllAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Order>> ListAsync(Expression<Func<Order, bool>> spec, int pageNumber, CancellationToken cancellationToken = default);

        Task<Order> AddAsync(Order entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(Order entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(Order entity, CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<Order, bool>> spec, CancellationToken cancellationToken = default);

        Task<Order> FirstAsync(Expression<Func<Order, bool>> spec, CancellationToken cancellationToken = default);

        Task<Order?> FirstOrDefaultAsync(Expression<Func<Order, bool>> spec, CancellationToken cancellationToken = default);
    }
}