using System.Dynamic;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate
{
    public class DataType
    {
        public DataTypeDefinitions Type { get; set; }

        public ExpandoObject? Specs { get; set; }
    }
}