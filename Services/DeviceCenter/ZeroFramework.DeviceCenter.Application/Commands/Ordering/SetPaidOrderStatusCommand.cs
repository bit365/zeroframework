using MediatR;
using System.Runtime.Serialization;

namespace ZeroFramework.DeviceCenter.Application.Commands.Ordering
{
    public class SetPaidOrderStatusCommand : IRequest<bool>
    {
        [DataMember]
        public Guid OrderId { get; private set; }

        public SetPaidOrderStatusCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
