namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public interface IPermissionStore
    {
        Task<bool> IsGrantedAsync(string operationName, string providerName, string providerKey, Guid? resourceGroupId);

        Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] operationNames, string providerName, string providerKey, Guid? resourceGroupId);
    }
}