using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MonitoringAggregate
{
    public class MonitoringFactor : BaseAggregateRoot<int>
    {
        [AllowNull]
        public string FactorCode { get; set; }

        [AllowNull]
        public string ChineseName { get; set; }

        [AllowNull]
        public string EnglishName { get; set; }

        public string? Unit { get; set; }

        /// <summary>
        /// The number of decimal places in the return value.
        /// </summary>
        public int Decimals { get; set; }

        public string? Remarks { get; set; }
    }
}