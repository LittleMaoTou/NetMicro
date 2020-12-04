using System;
using NetMicro.Cache.Abstractions;
using NetMicro.Core.Attributes;

namespace NetMicro.Cache.Provider
{
    [Singleton]
    public class CacheFactory : ICacheFactory
    {
        private readonly IRedisCache _csredis;
        private readonly IInMemoryCache _memory;
        public CacheFactory(IRedisCache csredis, IInMemoryCache memory)
        {
            _csredis = csredis;
            _memory = memory;
        }

        public IInMemoryCache CreateCache()
        {
            if (_memory == null)
                throw new NotImplementedException("内存缓存未注册");
            return _memory;
        }

        public IRedisCache CreateRedis()
        {
            if (_csredis == null)
                throw new NotImplementedException("redis缓存未注册");
            return _csredis;
        }
    }
}
