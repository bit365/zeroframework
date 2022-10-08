using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Generics
{
    public enum SortingOrder { Ascending, Descending }

    public class SortingDescriptor
    {
        [AllowNull]
        public string PropertyName { get; set; }

        public SortingOrder SortDirection { get; set; }
    }
}
