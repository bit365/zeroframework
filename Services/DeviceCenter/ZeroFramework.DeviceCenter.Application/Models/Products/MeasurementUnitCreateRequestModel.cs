using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Products
{
    public class MeasurementUnitCreateRequestModel
    {
        [AllowNull]
        public string UnitName { get; set; }

        [AllowNull]
        public string Unit { get; set; }

        [AllowNull]
        public string? Remark { get; set; }
    }
}
