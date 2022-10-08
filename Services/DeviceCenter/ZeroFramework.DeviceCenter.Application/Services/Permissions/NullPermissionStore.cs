namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public class NullPermissionStore : IPermissionStore
    {
        public Task<bool> IsGrantedAsync(string operationName, string providerName, string providerKey, Guid? resourceGroupId)
        {
            return Task.FromResult(true);
        }

        public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] operationNames, string providerName, string providerKey, Guid? resourceGroupId)
        {
            return Task.FromResult(new MultiplePermissionGrantResult(operationNames, PermissionGrantResult.Prohibited));
        }
    }
}