using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.RegularExpressions;
using ZeroFramework.DeviceCenter.Domain.Aggregates.PermissionAggregate;

namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public class PermissionStore(IPermissionGrantRepository permissionGrantRepository, IPermissionDefinitionManager permissionDefinitionManager, IDistributedCache distributedCache, ILogger<PermissionStore> logger) : IPermissionStore
    {
        private readonly IPermissionGrantRepository _permissionGrantRepository = permissionGrantRepository;

        private readonly IPermissionDefinitionManager _permissionDefinitionManager = permissionDefinitionManager;

        private readonly IDistributedCache _distributedCache = distributedCache;

        private readonly ILogger<PermissionStore> _logger = logger ?? NullLogger<PermissionStore>.Instance;

        private const string CacheKeyFormat = "pn:{0},pk:{1},on:{2},rg:{3}"; //<object-type>:<id>:<field1>.<field2> Or <object-type>:<id>:<field1>-<field2>

        public async Task<bool> IsGrantedAsync(string operationName, string providerName, string providerKey, Guid? resourceGroupId)
        {
            return await GetCacheItemAsync(operationName, providerName, providerKey, resourceGroupId);
        }

        public async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] operationNames, string providerName, string providerKey, Guid? resourceGroupId)
        {
            if (operationNames is null)
            {
                throw new ArgumentNullException(nameof(operationNames));
            }

            MultiplePermissionGrantResult result = new();

            if (operationNames.Length == 1)
            {
                string operationName = operationNames.First();
                result.Result.Add(operationName, await IsGrantedAsync(operationNames.First(), providerName, providerKey, resourceGroupId) ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined);
                return result;
            }

            var cacheItems = await GetCacheItemsAsync(operationNames, providerName, providerKey, resourceGroupId);

            foreach (var (Key, IsGranted) in cacheItems)
            {
                result.Result.Add(GetPermissionInfoFormCacheKey(Key).OperationName, IsGranted ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined);
            }

            return result;
        }

        protected virtual async Task<bool> GetCacheItemAsync(string operationName, string providerName, string providerKey, Guid? resourceGroupId)
        {
            var cacheKey = string.Format(CacheKeyFormat, providerName, providerKey, operationName, resourceGroupId);

            _logger.LogDebug("PermissionStore.GetCacheItemAsync: {cacheKey}", cacheKey);

            string? cacheItem = await _distributedCache.GetStringAsync(cacheKey);

            if (cacheItem is not null)
            {
                _logger.LogDebug("Found in the cache: {cacheKey}", cacheKey);
                return Convert.ToBoolean(cacheItem);
            }

            _logger.LogDebug("Not found in the cache: {cacheKey}", cacheKey);

            return await SetCacheItemsAsync(providerName, providerKey, operationName, resourceGroupId);
        }

        protected virtual async Task<bool> SetCacheItemsAsync(string providerName, string providerKey, string operationName, Guid? resourceGroupId)
        {
            var permissions = _permissionDefinitionManager.GetPermissions();

            _logger.LogDebug("Getting all granted permissions from the repository for this provider name,key: {providerName},{providerKey}", providerName, providerKey);

            var permissionGrants = await _permissionGrantRepository.GetListAsync(providerName, providerKey, resourceGroupId);

            var grantedPermissionsHashSet = new HashSet<string>(permissionGrants.Select(p => p.OperationName));

            _logger.LogDebug("Setting the cache items. Count: {permissionsCount}", permissions.Count);

            var cacheItems = new List<(string Key, bool IsGranted)>();

            bool currentResult = false;

            foreach (var permission in permissions)
            {
                var isGranted = grantedPermissionsHashSet.Contains(permission.Name);

                cacheItems.Add((string.Format(CacheKeyFormat, providerName, providerKey, permission.Name, resourceGroupId), isGranted));

                if (permission.Name == operationName)
                {
                    currentResult = isGranted;
                }
            }

            List<Task> setCacheItemTasks = [];

            foreach ((string Key, bool IsGranted) in cacheItems)
            {
                setCacheItemTasks.Add(_distributedCache.SetStringAsync(Key, IsGranted.ToString()));
            }

            await Task.WhenAll(setCacheItemTasks);

            _logger.LogDebug("Finished setting the cache items. Count: {permissionsCount}", permissions.Count);

            return currentResult;
        }

        protected virtual async Task<Dictionary<string, bool>> GetCacheItemsAsync(string[] operationNames, string providerName, string providerKey, Guid? resourceGroupId)
        {
            var cacheKeys = operationNames.Select(x => string.Format(CacheKeyFormat, providerName, providerKey, x, resourceGroupId)).ToList();

            _logger.LogDebug("PermissionStore.GetCacheItemAsync: {cacheKeys}", string.Join(",", cacheKeys));

            List<Task<KeyValuePair<string, string>>> getCacheItemTasks = [];

            foreach (string cacheKey in cacheKeys)
            {
                getCacheItemTasks.Add(Task.Run(() => new KeyValuePair<string, string>(cacheKey, _distributedCache.GetStringAsync(cacheKey).Result!)));
            }

            var cacheItems = await Task.WhenAll(getCacheItemTasks);

            if (cacheItems.All(x => x.Value is not null))
            {
                _logger.LogDebug("Found in the cache: {cacheKeys}", string.Join(",", cacheKeys));
                return cacheItems.ToDictionary(item => item.Key, item => Convert.ToBoolean(item.Value));
            }

            var notCacheKeys = cacheItems.Where(x => x.Value is null).Select(x => x.Key).ToList();

            _logger.LogDebug("Not found in the cache: {notCacheKeys}", string.Join(",", notCacheKeys));

            return await SetCacheItemsAsync(providerName, providerKey, resourceGroupId, notCacheKeys);
        }

        protected virtual async Task<Dictionary<string, bool>> SetCacheItemsAsync(string providerName, string providerKey, Guid? resourceGroupId, List<string> notCacheKeys)
        {
            var permissions = _permissionDefinitionManager.GetPermissions().Where(x => notCacheKeys.Any(k => GetPermissionInfoFormCacheKey(k).OperationName == x.Name)).ToList();

            _logger.LogDebug("Getting not cache granted permissions from the repository for this provider name,key: {providerName},{providerKey}", providerName, providerKey);

            var operationNames = notCacheKeys.Select(k => GetPermissionInfoFormCacheKey(k).OperationName).ToArray();

            var permissionGrants = await _permissionGrantRepository.GetListAsync(operationNames, providerName, providerKey, resourceGroupId);

            var grantedPermissionsHashSet = new HashSet<string>(permissionGrants.Select(p => p.OperationName));

            _logger.LogDebug("Setting the cache items. Count: {permissionsCount}", permissions.Count);

            Dictionary<string, bool> cacheItems = [];

            foreach (PermissionDefinition? permission in permissions)
            {
                var isGranted = grantedPermissionsHashSet.Contains(permission.Name);
                cacheItems.Add(string.Format(CacheKeyFormat, providerName, providerKey, permission.Name, resourceGroupId), isGranted);
            }

            List<Task> setCacheItemTasks = [];

            foreach ((string Key, bool IsGranted) in cacheItems)
            {
                setCacheItemTasks.Add(_distributedCache.SetStringAsync(Key, IsGranted.ToString()));
            }

            await Task.WhenAll(setCacheItemTasks);

            _logger.LogDebug("Finished setting the cache items. Count: {permissionsCount}", permissions.Count);

            return cacheItems;
        }

        protected virtual (string ProviderName, string ProviderKey, string OperationName, string ResourceGroupId) GetPermissionInfoFormCacheKey(string key)
        {
            string pattern = @"^pn:(?<providerName>.+),pk:(?<providerKey>.+),on:(?<operationName>.+),rg:(?<resourceGroupId>.+)$";

            Match match = Regex.Match(key, pattern, RegexOptions.IgnoreCase);

            string providerName = match.Groups["providerName"].Value;
            string providerKey = match.Groups["providerKey"].Value;
            string operationName = match.Groups["operationName"].Value;
            string resourceGroupId = match.Groups["resourceGroupId"].Value;

            return (providerName, providerKey, operationName, resourceGroupId);
        }
    }
}