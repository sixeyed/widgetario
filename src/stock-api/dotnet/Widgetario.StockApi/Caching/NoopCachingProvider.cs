using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Widgetario.StockApi.Caching
{
    public class NoopCachingProvider : IEasyCachingProvider
    {
        public bool IsDistributedCache => throw new NotImplementedException();

        public int MaxRdSecond => throw new NotImplementedException();

        public CachingProviderType CachingProviderType => throw new NotImplementedException();

        public CacheStats CacheStats => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public bool UseLock => throw new NotImplementedException();

        public object Database => throw new NotImplementedException();

        public bool Exists(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public Task FlushAsync()
        {
            throw new NotImplementedException();
        }

        public Task FlushAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public CacheValue<T> Get<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public CacheValue<T> Get<T>(string cacheKey, Func<T> dataRetriever, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, CacheValue<T>> GetAll<T>(IEnumerable<string> cacheKeys)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, CacheValue<T>>> GetAllAsync<T>(IEnumerable<string> cacheKeys)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, CacheValue<T>>> GetAllAsync<T>(IEnumerable<string> cacheKeys, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAllKeysByPrefix(string prefix)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllKeysByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<CacheValue<T>> GetAsync<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<CacheValue<T>> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<CacheValue<T>> GetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public Task<CacheValue<T>> GetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAsync(string cacheKey, Type type)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAsync(string cacheKey, Type type, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, CacheValue<T>> GetByPrefix<T>(string prefix)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, CacheValue<T>>> GetByPrefixAsync<T>(string prefix)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, CacheValue<T>>> GetByPrefixAsync<T>(string prefix, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public int GetCount(string prefix = "")
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(string prefix = "")
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(string prefix = "", CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetExpiration(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan> GetExpirationAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan> GetExpirationAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ProviderInfo GetProviderInfo()
        {
            throw new NotImplementedException();
        }

        public void Remove(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(IEnumerable<string> cacheKeys)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync(IEnumerable<string> cacheKeys)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync(IEnumerable<string> cacheKeys, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();
        }

        public void RemoveByPrefix(string prefix)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByPatternAsync(string pattern)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByPrefixAsync(string prefix)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public void SetAll<T>(IDictionary<string, T> value, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public Task SetAllAsync<T>(IDictionary<string, T> value, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public Task SetAllAsync<T>(IDictionary<string, T> value, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool TrySet<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TrySetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TrySetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
