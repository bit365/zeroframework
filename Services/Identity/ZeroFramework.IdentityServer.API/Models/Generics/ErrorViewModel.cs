using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Generics
{
    public class ErrorViewModel
    {
        [AllowNull]
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public Duende.IdentityServer.Models.ErrorMessage? Error { get; set; }
    }
}