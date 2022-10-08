using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ZeroFramework.DeviceCenter.Application.Extensions.Caching
{
    public static class CustomDistributedCacheExtensions
    {
        public static void SetObject<TItem>(this IDistributedCache cache, string key, TItem value)
        {
            cache.SetObject(key, value, new DistributedCacheEntryOptions());
        }

        public static void SetObject<TItem>(this IDistributedCache cache, string key, TItem value, DistributedCacheEntryOptions options)
        {
            cache.SetString(key, JsonSerializer.Serialize(value), options);
        }

        public static Task SetObjectAsync<TItem>(this IDistributedCache cache, string key, TItem value, CancellationToken token = default)
        {
            return cache.SetObjectAsync(key, value, new DistributedCacheEntryOptions(), token);
        }

        public static Task SetObjectAsync<TItem>(this IDistributedCache cache, string key, TItem value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            return cache.SetStringAsync(key, JsonSerializer.Serialize(value), options, token);
        }

        public static TItem? GetObject<TItem>(this IDistributedCache cache, string key)
        {
            var dataString = cache.GetString(key);
            if (dataString is null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<TItem>(dataString);
        }

        public static async Task<TItem?> GetObjectAsync<TItem>(this IDistributedCache cache, string key, CancellationToken token = default)
        {
            var dataString = await cache.GetStringAsync(key, token);
            if (dataString is null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<TItem>(dataString);
        }

        public static TItem? GetOrCreate<TItem>(this IDistributedCache cache, string key, Func<DistributedCacheEntryOptions, TItem> factory)
        {
            if (!cache.TryGetValue(key, out TItem? result))
            {
                var entryOptions = new DistributedCacheEntryOptions();
                result = factory(entryOptions);
                cache.SetObjectAsync(key, result, entryOptions);
            }

            return result;
        }

        public static async Task<TItem?> GetOrCreateAsync<TItem>(this IDistributedCache cache, string key, Func<DistributedCacheEntryOptions, Task<TItem>> factory)
        {
            if (!cache.TryGetValue(key, out TItem? result))
            {
                var entryOptions = new DistributedCacheEntryOptions();
                result = await factory(entryOptions);
                await cache.SetObjectAsync(key, result, entryOptions);
            }

            return result;
        }

        public static bool TryGetValue<TItem>(this IDistributedCache cache, string key, out TItem? value)
        {
            var dataString = cache.GetString(key);

            if (dataString is null)
            {
                value = default;
                return false;
            }

            value = JsonSerializer.Deserialize<TItem>(dataString);
            return true;
        }
    }
}