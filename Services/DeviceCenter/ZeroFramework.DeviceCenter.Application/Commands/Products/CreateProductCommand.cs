using MediatR;
using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Application.Commands.Products
{
    public class CreateProductCommand : IRequest<ProductGetResponseModel>
    {
        [AllowNull]
        public string Name { get; set; }

        public ProductNodeType NodeType { get; set; }

        public ProductNetType NetType { get; set; }

        public ProductProtocolType ProtocolType { get; set; }

        public ProductDataFormat DataFormat { get; set; }

        public ProductFeatures? Features { get; set; }

        public string? Remark { get; set; }

        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;
    }
}
