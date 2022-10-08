namespace ZeroFramework.IdentityServer.API.IdentityStores
{
    /// <summary>
    /// Represents a tenant in the identity system
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the tenant.</typeparam>
    public class IdentityTenant
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityTenant{TKey}"/>.
        /// </summary>
        public IdentityTenant() { }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityTenant{TKey}"/>.
        /// </summary>
        /// <param name="tenantName">The tenant name.</param>
        public IdentityTenant(string tenantName) : this()
        {
            Name = tenantName;
        }

        /// <summary>
        /// Gets or sets the primary key for this tenant.
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name for this tenant.
        /// </summary>
        public virtual string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the normalized name for this tenant.
        /// </summary>
        public virtual string NormalizedName { get; set; } = string.Empty;

        /// <summary>
        /// A random value that should change whenever a tenant is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the display name for this tenant.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the creation time for this tenant.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Returns the name of the tenant.
        /// </summary>
        /// <returns>The name of the tenant.</returns>
        public override string ToString() => Name;
    }
}
