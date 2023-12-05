using AutoMapper;
using ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering;
using ZeroFramework.DeviceCenter.Application.Models.Ordering;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using ZeroFramework.DeviceCenter.Domain.Services.Ordering;
using ZeroFramework.EventBus.Abstractions;

namespace ZeroFramework.DeviceCenter.Application.Services.Ordering
{
    public class OrderApplicationService(IOrderDomainService orderDomainService, IRepository<Order> orderRepository, IEventBus eventBus, IMapper mapper) : IOrderApplicationService
    {
        private readonly IOrderDomainService _orderDomainService = orderDomainService;

        private readonly IRepository<Order> _orderRepository = orderRepository;

        private readonly IEventBus _eventBus = eventBus;

        private readonly IMapper _mapper = mapper;

        public async Task<bool> CreateAsync(OrderCreateRequestModel model, CancellationToken cancellationToken = default)
        {
            Order order = _mapper.Map<Order>(model);

            await _orderDomainService.AddAsync(order, cancellationToken);

            await _eventBus.PublishAsync(new OrderStartedIntegrationEvent(Guid.NewGuid()), cancellationToken);

            return await Task.FromResult(true);
        }

        public async Task<List<OrderListResponseModel>> GetListAsync(OrderListRequestModel model, CancellationToken cancellationToken = default)
        {
            List<Order> orders = await _orderRepository.GetListAsync(model.PageNumber, model.PageSize, sorting: o => o.CreationTime, cancellationToken: cancellationToken);

            return _mapper.Map<List<OrderListResponseModel>>(orders);
        }
    }
}
