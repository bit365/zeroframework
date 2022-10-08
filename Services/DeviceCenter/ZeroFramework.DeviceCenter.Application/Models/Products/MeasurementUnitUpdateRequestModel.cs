using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Products
{
    public class MeasurementUnitUpdateRequestModel
    {
        public int Id { get; set; }

        [AllowNull]
        public string UnitName { get; set; }

        [AllowNull]
        public string Unit { get; set; }

        [AllowNull]
        public string? Remark { get; set; }
    }
}
