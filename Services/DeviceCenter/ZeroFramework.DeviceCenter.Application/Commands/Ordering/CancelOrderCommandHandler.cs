using MediatR;
using ZeroFramework.DeviceCenter.Application.Infrastructure;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;
using ZeroFramework.DeviceCenter.Infrastructure.Idempotency;

namespace ZeroFramework.DeviceCenter.Application.Commands.Ordering
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Handler which processes the command when customer executes cancel order from app
        /// </summary>
        public async Task<bool> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetAsync(command.OrderId);

            if (orderToUpdate != null)
            {
                orderToUpdate.SetCancelledStatus();
                await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

            return await Task.FromResult(true);
        }
    }

    /// <summary>
    /// Use for Idempotency in Command process
    /// </summary>
    public class CancelOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CancelOrderCommand, bool>
    {
        public CancelOrderIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for processing order.
        }
    }
}