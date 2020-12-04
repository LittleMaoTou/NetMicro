using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nest;
using System;

namespace NetMicro.Elasticsearch
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElastic(this IServiceCollection services, Action<ESConfig> action)
        {
            services.Configure<ESConfig>(action);
            services.TryAddScoped<IConnectionPool, ESConnectionPool>();
            services.TryAddScoped<IElasticClient, ESClient>();
            return services;
        }
    }
}
