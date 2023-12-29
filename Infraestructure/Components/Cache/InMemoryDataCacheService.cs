﻿using Application.Components;
using Microsoft.Extensions.Caching.Memory;

namespace Infraestructure.Components.Cache
{
    public class InMemoryDataCacheService : IDataCacheService
    {
        public readonly TimeSpan DefualtCacheTime = TimeSpan.FromMinutes(5);
        private readonly IMemoryCache _cache;
        public InMemoryDataCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T> Get<T>(string key)
        {
            var data = _cache.Get<T>(key);
            return Task.FromResult(data);
        }

        public Task<object> Get(string key)
        {
            var data = _cache.Get(key);
            return Task.FromResult(data);
        }

        public Task Put(string key, object value, TimeSpan time)
        {
            _cache.Set(key, value, time);
            return Task.CompletedTask;
        }

        public Task Put(string key, object value)
        {
            _cache.Set(key, value, DefualtCacheTime);
            return Task.CompletedTask;
        }
    }
}
