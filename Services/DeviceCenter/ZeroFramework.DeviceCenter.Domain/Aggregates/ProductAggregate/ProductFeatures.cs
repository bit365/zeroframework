namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate
{
    public class ProductFeatures
    {
        /// <summary>
        /// 属性定义
        /// </summary>
        public IEnumerable<PropertyFeature>? Properties { get; set; }

        /// <summary>
        /// 服务定义
        /// </summary>
        public IEnumerable<ServiceFeature>? Services { get; set; }

        /// <summary>
        /// 事件定义
        /// </summary>
        public IEnumerable<EventFeature>? Events { get; set; }
    }
}
