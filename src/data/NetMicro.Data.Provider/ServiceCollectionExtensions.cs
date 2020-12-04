using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using NetMicro.Data.Abstractions.Options;

namespace NetMicro.Data.Provider
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册仓储上下文(默认使用控制台注册)
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public static IServiceCollection AddContext<TContext>(this IServiceCollection services, Action<DbOptions> action)
            where TContext : DbContext
        {
            services.AddContext<TContext>(action, LoggerFactory.Create(builder => { builder.AddConsole(); }));
            return services;
        }

        /// <summary>
        /// 注册仓储上下文
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public static IServiceCollection AddContext<TContext>(this IServiceCollection services, Action<DbOptions> action, ILoggerFactory loggerFactory)
            where TContext : DbContext
        {
            var options = new DbOptions();
            action.Invoke(options);
            var context = (TContext)Activator.CreateInstance(typeof(TContext), options, loggerFactory);
            services.AddSingleton(ct => context);
            return services;
        }
    }
}
