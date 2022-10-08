namespace ZeroFramework.DeviceCenter.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(string commandId);

        Task CreateRequestForCommandAsync<T>(string commandId);
    }
}