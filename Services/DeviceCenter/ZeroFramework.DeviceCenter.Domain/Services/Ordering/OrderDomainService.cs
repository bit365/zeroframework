using MediatR;
using System.Linq.Expressions;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;
using ZeroFramework.DeviceCenter.Domain.Constants;
using ZeroFramework.DeviceCenter.Domain.Events.Ordering;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using ZeroFramework.DeviceCenter.Domain.Specifications.Builder;

namespace ZeroFramework.DeviceCenter.Domain.Services.Ordering
{
    public class OrderDomainService : IOrderDomainService
    {
        private readonly IRepository<Order, Guid> _orderRepository;

        private readonly IMediator _mediator;

        public OrderDomainService(IRepository<Order, Guid> orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<Order> AddAsync(Order entity, CancellationToken cancellationToken = default)
        {
            return await _orderRepository.InsertAsync(entity, true, cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<Order, bool>> spec, CancellationToken cancellationToken = default)
        {
            return await _orderRepository.CountAsync(spec, cancellationToken);
        }

        public async Task DeleteAsync(Order entity, CancellationToken cancellationToken = default)
        {
            await _mediator.Publish(new OrderCancelledDomainEvent(entity), cancellationToken);

            await _orderRepository.DeleteAsync(entity, true, cancellationToken);
        }

        public async Task<Order> FirstAsync(Expression<Func<Order, bool>> spec, CancellationToken cancellationToken = default)
        {
            return await _orderRepository.FirstAsync(spec, cancellationToken);
        }

        public async Task<Order?> FirstOrDefaultAsync(Expression<Func<Order, bool>> spec, CancellationToken cancellationToken = default)
        {
            return await _orderRepository.FirstOrDefaultAsync(spec, cancellationToken);
        }

        public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _orderRepository.GetAsync(id, cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _orderRepository.GetListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> ListAsync(Expression<Func<Order, bool>> spec, int pageNumber, CancellationToken cancellationToken = default)
        {
            var specification = new OrderPagingSpecification(pageNumber, PagingConstants.DefaultPageSize);

            var specificationBuilder = new SpecificationBuilder<Order>(specification);

            specificationBuilder.Where(spec);

            return await _orderRepository.GetListAsync(specificationBuilder.Specification, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
        {
            await _orderRepository.UpdateAsync(entity, true, cancellationToken);
        }
    }
}