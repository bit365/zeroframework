using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Services.Generics
{
    public enum SortingOrder { Ascending, Descending }

    public class SortingDescriptor
    {
        [AllowNull]
        public string PropertyName { get; set; }

        public SortingOrder SortDirection { get; set; }
    }
}