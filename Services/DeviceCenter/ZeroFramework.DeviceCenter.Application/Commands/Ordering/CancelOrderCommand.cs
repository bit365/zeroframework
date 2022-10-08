using MediatR;
using System.Runtime.Serialization;

namespace ZeroFramework.DeviceCenter.Application.Commands.Ordering
{
    public class CancelOrderCommand : IRequest<bool>
    {
        [DataMember]
        public Guid OrderId { get; private set; }

        public CancelOrderCommand(Guid orderId) => OrderId = orderId;
    }
}
