using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMicro.Cache.Abstractions;
using NetMicro.EventBus.Abstractions.Tracker;
using System;
using System.Threading.Tasks;

namespace NetMicro.EventBus.Provider
{
    public class RedisTracker : IConsumerTracker
    {
        private readonly ICacheFactory _cache;
        private readonly IOptions<ExpiredOptions> _options;
        private readonly ILogger<RedisTracker> _logger;

        public RedisTracker(ICacheFactory cache, IOptions<ExpiredOptions> options, ILogger<RedisTracker> logger)
        {
            _cache = cache;
            _options = options;
            _logger = logger;
        }


        public bool HasProcessed(string eventId)
        {
            try
            {
                var redis = _cache.CreateRedis();
                return redis.ContainsAsync($"{TrackerKey.Idempotent}:{eventId}").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"幂等判断出现异常：{ex}，幂等无效，不影响程序运行，及时解决");
                return true;
            }
        }

        public Task<bool> HasProcessedAsync(string eventId)
        {
            try
            {
                var redis = _cache.CreateRedis();
                return redis.ContainsAsync($"{TrackerKey.Idempotent}:{eventId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"幂等判断出现异常：{ex}，幂等无效，不影响程序运行，及时解决");
                return Task.FromResult(true);
            }
        }

        public bool MarkAsProcessed(string eventId)
        {
            try
            {
                var redis = _cache.CreateRedis();
                return redis.AddAsync($"{TrackerKey.Idempotent}:{eventId}", eventId, _options.Value.ExpiredTime).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"幂等判断出现异常：{ex}，幂等无效，不影响程序运行，及时解决");
                return true;
            }
        }

        public Task<bool> MarkAsProcessedAsync(string eventId)
        {
            try
            {
                var redis = _cache.CreateRedis();
                return redis.AddAsync($"{TrackerKey.Idempotent}:{eventId}", eventId, _options.Value.ExpiredTime);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"幂等判断出现异常：{ex}，幂等无效，不影响程序运行，及时解决");
                return Task.FromResult(true);
            }
        }
    }
}
