namespace ZeroFramework.IdentityServer.API.Models.Tenants
{
    public class TenantClaimModel
    {
        /// <summary>
        /// Gets or sets the identifier for this tenant claim.
        /// </summary>
        public virtual int? Id { get; set; }

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
    }
}
