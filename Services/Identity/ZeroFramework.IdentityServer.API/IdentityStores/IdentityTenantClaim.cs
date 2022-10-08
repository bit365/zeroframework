using System.Security.Claims;

namespace ZeroFramework.IdentityServer.API.IdentityStores
{
    /// <summary>
    /// Represents a claim that is granted to all users within a tenant.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key of the tenant associated with this claim.</typeparam>
    public class IdentityTenantClaim
    {
        /// <summary>
        /// Gets or sets the identifier for this tenant claim.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the of the primary key of the tenant associated with this claim.
        /// </summary>
        public virtual Guid TenantId { get; set; }

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        public virtual string ClaimType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        public virtual string? ClaimValue { get; set; }

        /// <summary>
        /// Constructs a new claim with the type and value.
        /// </summary>
        /// <returns>The <see cref="Claim"/> that was produced.</returns>
        public virtual Claim? ToClaim()
        {
            return ClaimType is not null ? new Claim(ClaimType, ClaimValue!) : null;
        }

        /// <summary>
        /// Initializes by copying ClaimType and ClaimValue from the other claim.
        /// </summary>
        /// <param name="other">The claim to initialize from.</param>
        public virtual void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type ?? string.Empty;
            ClaimValue = other?.Value;
        }
    }
}
