using MediatR;

namespace ZeroFramework.DeviceCenter.Application.Infrastructure
{
    public class IdentifiedCommand<TRequest, TResponse> : IRequest<TResponse> where TRequest : IRequest<TResponse>
    {
        public TRequest Command { get; }

        public string Id { get; }

        public IdentifiedCommand(TRequest command, string id)
        {
            Command = command;
            Id = id;
        }
    }
}
