using AutoMapper;
using MediatR;
using ZeroFramework.DeviceCenter.Application.Infrastructure;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using ZeroFramework.DeviceCenter.Infrastructure.Idempotency;

namespace ZeroFramework.DeviceCenter.Application.Commands.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductGetResponseModel>
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductGetResponseModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = _mapper.Map<Product>(request);
            product = await _productRepository.InsertAsync(product, true, cancellationToken);
            return _mapper.Map<ProductGetResponseModel>(product);
        }
    }

    public class CreateProductIdentifiedCommandHandler : IdentifiedCommandHandler<CreateProductCommand, ProductGetResponseModel>
    {
        public CreateProductIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
        {
        }

        protected override ProductGetResponseModel? CreateResultForDuplicateRequest() => null; // Ignore duplicate requests for processing.
    }
}