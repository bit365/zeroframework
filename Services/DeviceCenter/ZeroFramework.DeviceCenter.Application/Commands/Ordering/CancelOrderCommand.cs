using MediatR;
using System.Runtime.Serialization;

namespace ZeroFramework.DeviceCenter.Application.Commands.Ordering
{
    public class CancelOrderCommand(Guid orderId) : IRequest<bool>
    {
        [DataMember]
        public Guid OrderId { get; private set; } = orderId;
    }
}
