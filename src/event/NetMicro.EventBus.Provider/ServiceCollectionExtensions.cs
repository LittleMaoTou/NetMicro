using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using NetMicro.EventBus.Abstractions;
using NetMicro.EventBus.Abstractions.Tracker;
using System;

namespace NetMicro.EventBus.Provider
{
    /// <summary>
    /// 事件总线注册
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册事件总线服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="action">配置操作</param>
        public static IServiceCollection UseEventBus(this IServiceCollection services, Action<CapOptions> cap, bool tracker = false, Action<ExpiredOptions> expired = null)
        {
            if (expired == null)
                expired = m => { m.ExpiredTime = 15 * 24 * 3600; };
            services.Configure(expired);
            services.AddSingleton<IEventBus, EventBus>();
            if (tracker)
            {
                services.AddSingleton<IConsumerTracker, RedisTracker>();
            }
            services.AddCap(cap);
            return services;
        }


    }
}
