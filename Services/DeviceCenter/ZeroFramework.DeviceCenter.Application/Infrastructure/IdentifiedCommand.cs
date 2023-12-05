using MediatR;

namespace ZeroFramework.DeviceCenter.Application.Infrastructure
{
    public class IdentifiedCommand<TRequest, TResponse>(TRequest command, string id) : IRequest<TResponse> where TRequest : IRequest<TResponse>
    {
        public TRequest Command { get; } = command;

        public string Id { get; } = id;
    }
}
