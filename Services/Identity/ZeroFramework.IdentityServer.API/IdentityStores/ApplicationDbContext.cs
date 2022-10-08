using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API.IdentityStores
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ICurrentTenant _currentTenant;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor, ICurrentTenant currentTenant) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _currentTenant = currentTenant;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.TenantUserName).HasMaxLength(256).IsRequired();
                b.Property(u => u.DisplayName).HasMaxLength(256);
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.Property(u => u.TenantRoleName).HasMaxLength(256).IsRequired();
                b.Property(r => r.DisplayName).HasMaxLength(256);
            });

            builder.Entity<IdentityTenant>(b =>
            {
                b.HasKey(t => t.Id);
                b.HasIndex(t => t.NormalizedName).IsUnique();
                b.ToTable("AspNetTenants", "dbo");
                b.Property(t => t.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(t => t.Name).HasMaxLength(256);
                b.Property(t => t.NormalizedName).HasMaxLength(256);
                b.Property(t => t.DisplayName).HasMaxLength(256);

                b.HasMany<IdentityTenantClaim>().WithOne().HasForeignKey(tc => tc.TenantId);
                b.HasMany<ApplicationRole>().WithOne().HasForeignKey(r => r.TenantId);
                b.HasMany<ApplicationUser>().WithOne().HasForeignKey(u => u.TenantId);
            });

            builder.Entity<IdentityTenantClaim>(b =>
            {
                b.HasKey(tc => tc.Id);
                b.ToTable("AspNetTenantClaims", "dbo");
            });

            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsAssignableTo(typeof(IMultiTenant)))
                {
                    builder.Entity(entityType.ClrType).AddQueryFilter<IMultiTenant>(t => IgnoreQueryFilters(_httpContextAccessor) || t.TenantId == _currentTenant.Id);
                }
            }

            base.OnModelCreating(builder);
        }

        protected virtual bool IgnoreQueryFilters(IHttpContextAccessor httpContextAccessor)
        {
            Endpoint? endpoint = _httpContextAccessor?.HttpContext?.GetEndpoint();

            if (endpoint != null && endpoint.Metadata.Any(item => item is ApiControllerAttribute))
            {
                return false;
            }

            return true;
        }
    }
}