using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;
using ZeroFramework.EventBus.Extensions;

namespace ZeroFramework.DeviceCenter.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger) => _logger = logger ?? throw new ArgumentException(nameof(ILogger));

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            string? typeName = request?.GetGenericTypeName();

            TResponse? response = default;

            using (TransactionScope? scope = new(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {
                    response = await next();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
                    throw;
                }
            }

            return response;
        }
    }
}
