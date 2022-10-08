using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroFramework.IdentityServer.API.IdentityStores
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>, Tenants.IMultiTenant
    {
        [Column(Order = 100)]
        public string? TenantUserName { get; set; }

        [Column(Order = 101)]
        public Guid? TenantId { get; set; }

        [Column(Order = 102)]
        public string? DisplayName { get; set; }

        [Column(Order = 103)]
        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;
    }
}