using Microsoft.Extensions.DependencyInjection;
using System;
using NetMicro.Cache.Abstractions;
using NetMicro.Cache.Provider.Memory;
using NetMicro.Cache.Provider.StackExchange;

namespace NetMicro.Cache.Provider
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IInMemoryCache, MemoryCache>();
            services.AddSingleton<ICacheFactory, CacheFactory>();
            services.AddSingleton<IRedisCache, StackExchangeCache>();
            return services;
        }

        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseCache(this IServiceCollection services, string connection)
        {
            services.UseCache(m =>
            {
                m.Connection = connection;
            });
            return services;
        }


        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseCache(this IServiceCollection services, Action<RedisOptions> action)
        {
            services.Configure(action);
            services.UseCache();
            return services;
        }
    }
}
