namespace ZeroFramework.DeviceCenter.Domain.Repositories
{
    /// <summary>
    /// Data seeding is the process of populating a database with an initial set of data.
    /// </summary>
    public interface IDataSeedProvider
    {
        Task SeedAsync(IServiceProvider serviceProvider);
    }
}