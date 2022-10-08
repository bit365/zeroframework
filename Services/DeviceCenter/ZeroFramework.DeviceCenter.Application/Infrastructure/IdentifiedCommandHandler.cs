using MediatR;
using ZeroFramework.DeviceCenter.Infrastructure.Idempotency;

namespace ZeroFramework.DeviceCenter.Application.Infrastructure
{
    /// <summary>
    /// Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the cases where
    /// a requestid sent by client is used to detect duplicate requests.
    /// </summary>
    public class IdentifiedCommandHandler<TRequest, TResponse> : IRequestHandler<IdentifiedCommand<TRequest, TResponse>, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IMediator _mediator;

        private readonly IRequestManager _requestManager;

        public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager)
        {
            _mediator = mediator;
            _requestManager = requestManager;
        }

        /// <summary>
        /// Creates the result value to return if a previous request was found
        /// </summary>
        protected virtual TResponse? CreateResultForDuplicateRequest() => default;

        /// <summary>
        /// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
        /// just enqueues the original inner command.
        /// </summary>
        public async Task<TResponse> Handle(IdentifiedCommand<TRequest, TResponse> command, CancellationToken cancellationToken)
        {
            if (await _requestManager.ExistAsync(command.Id))
            {
                return CreateResultForDuplicateRequest() ?? throw new NotImplementedException();
            }

            await _requestManager.CreateRequestForCommandAsync<TRequest>(command.Id);

            // Send the embeded business command to mediator so it runs its related CommandHandler 
            return await _mediator.Send(command.Command, cancellationToken);
        }
    }
}