using MediatR;
using System.Runtime.Serialization;

namespace ZeroFramework.DeviceCenter.Application.Commands.Ordering
{
    public class SetPaidOrderStatusCommand(Guid orderId) : IRequest<bool>
    {
        [DataMember]
        public Guid OrderId { get; private set; } = orderId;
    }
}
