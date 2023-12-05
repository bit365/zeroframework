using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using ZeroFramework.DeviceCenter.Application.Commands.Products;
using ZeroFramework.DeviceCenter.Application.Infrastructure;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.PermissionProviders;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Application.Services.Products;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    /// <summary>
    /// For more information on enabling Web API for empty projects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductApplicationService productService, IMediator mediator) : ControllerBase
    {
        private readonly IProductApplicationService _productService = productService;

        private readonly IMediator _mediator = mediator;

        // GET: api/<ProductsController>
        [HttpGet]
        [Authorize(ProductPermissions.Products.Default)]
        public async Task<PagedResponseModel<ProductGetResponseModel>> GetProducts([FromQuery] ProductPagedRequestModel model)
        {
            return await _productService.GetListAsync(model);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        [Authorize(ProductPermissions.Products.Default)]
        public async Task<ProductGetResponseModel> GetProduct(int id)
        {
            return await _productService.GetAsync(id);
        }

        // POST api/<ProductsController>
        [HttpPost]
        [Authorize(ProductPermissions.Products.Create)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostProduct([FromBody] CreateProductCommand command, [FromHeader(Name = "X-Request-Id")] string? requestId)
        {
            requestId ??= Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var identifiedCommand = new IdentifiedCommand<CreateProductCommand, ProductGetResponseModel>(command, requestId);
            ProductGetResponseModel result = await _mediator.Send(identifiedCommand);

            return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        [Authorize(ProductPermissions.Products.Edit)]
        public async Task<ProductGetResponseModel> PutProduct(int id, [FromBody] ProductUpdateRequestModel value)
        {
            value.Id = id;
            return await _productService.UpdateAsync(id, value);
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        [Authorize(ProductPermissions.Products.Delete)]
        public async Task DeleteProduct(int id)
        {
            await _productService.DeleteAsync(id);
        }
    }
}