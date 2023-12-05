using MediatR;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Application.Commands.Ordering
{
    public class SetPaidOrderStatusCommandHandler(IOrderRepository orderRepository) : IRequestHandler<SetPaidOrderStatusCommand, bool>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        /// <summary>
        /// Handler which processes the command when Shipment service confirms the payment
        /// </summary>
        public async Task<bool> Handle(SetPaidOrderStatusCommand command, CancellationToken cancellationToken)
        {
            // Simulate a work time for validating the payment
            await Task.Delay(10000, cancellationToken);

            var orderToUpdate = await _orderRepository.GetAsync(command.OrderId);

            if (orderToUpdate == null)
            {
                return false;
            }

            orderToUpdate.SetPaidStatus();

            await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
