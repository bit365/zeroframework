using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using ZeroFramework.DeviceCenter.Application.Commands.Ordering;
using ZeroFramework.DeviceCenter.Application.Infrastructure;
using ZeroFramework.DeviceCenter.Application.Queries.Ordering;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OrdersController : Controller
    {
        private readonly IOrderQueries _orderQueries;

        private readonly IMediator _mediator;

        public OrdersController(IOrderQueries orderQueries, IMediator mediator)
        {
            _orderQueries = orderQueries;
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(OrderViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetOrderAsync(Guid orderId)
        {
            //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
            //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
            OrderViewModel orderViewModel = await _orderQueries.GetOrderAsync(orderId);
            return Ok(orderViewModel);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelOrderAsync([FromBody] CancelOrderCommand command, [FromHeader(Name = "X-Request-Id")] string? requestId)
        {
            requestId ??= Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var identifiedCommand = new IdentifiedCommand<CancelOrderCommand, bool>(command, requestId);
            await _mediator.Send(identifiedCommand);

            return Ok();
        }
    }
}