namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate
{
    public class ServiceFeature : AbstractFeature
    {
        public ServiceInvokeMethod InvokeMethod { get; set; }

        public IEnumerable<DataParameter>? InputData { get; set; }

        public IEnumerable<DataParameter>? OutputData { get; set; }
    }
}