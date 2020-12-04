
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetMicro.Core.Maps
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapper(this IServiceCollection services, params Assembly[] assemblies)
            => AddMapperClasses(services, null, assemblies);



        public static IServiceCollection AddMapper(this IServiceCollection services, params Type[] profileAssemblyMarkerTypes)
            => AddMapperClasses(services, null, profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly));

        public static IServiceCollection AddMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction, params Type[] profileAssemblyMarkerTypes)
            => AddMapperClasses(services, (sp, cfg) => configAction?.Invoke(cfg), profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly));

        public static IServiceCollection AddMapper(this IServiceCollection services, Action<IServiceProvider, IMapperConfigurationExpression> configAction, params Type[] profileAssemblyMarkerTypes)
            => AddMapperClasses(services, configAction, profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly));

        public static IServiceCollection AddMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction,
            IEnumerable<Type> profileAssemblyMarkerTypes, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            => AddMapperClasses(services, (sp, cfg) => configAction?.Invoke(cfg), profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly), serviceLifetime);

        public static IServiceCollection AddMapper(this IServiceCollection services, Action<IServiceProvider, IMapperConfigurationExpression> configAction,
            IEnumerable<Type> profileAssemblyMarkerTypes, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            => AddMapperClasses(services, configAction, profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly), serviceLifetime);

        private static IServiceCollection AddMapperClasses(IServiceCollection services, Action<IServiceProvider, IMapperConfigurationExpression> configAction,
            IEnumerable<Assembly> assembliesToScan, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            // Just return if we've already added AutoMapper to avoid double-registration
            if (services.Any(sd => sd.ServiceType == typeof(IMapper)))
                return services;

            assembliesToScan = assembliesToScan as Assembly[] ?? assembliesToScan.ToArray();

            var allTypes = assembliesToScan
                .Where(a => !a.IsDynamic && a.GetName().Name != nameof(AutoMapper))
                .Distinct() // avoid AutoMapper.DuplicateTypeMapConfigurationException
                .SelectMany(a => a.DefinedTypes)
                .ToArray();

            void ConfigAction(IServiceProvider serviceProvider, IMapperConfigurationExpression cfg)
            {
                configAction?.Invoke(serviceProvider, cfg);

                cfg.AddMaps(assembliesToScan);
            }

            var openTypes = new[]
            {
                typeof(IValueResolver<,,>),
                typeof(IMemberValueResolver<,,,>),
                typeof(ITypeConverter<,>),
                typeof(IValueConverter<,>),
                typeof(IMappingAction<,>)
            };
            foreach (var type in openTypes.SelectMany(openType => allTypes
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && t.AsType().ImplementsGenericInterface(openType))))
            {
                services.AddTransient(type.AsType());
            }

            services.AddSingleton<IConfigurationProvider>(sp => new MapperConfiguration(cfg => ConfigAction(sp, cfg)));
            services.Add(new ServiceDescriptor(typeof(IMapper),
                sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService), serviceLifetime));

            return services;
        }

        private static bool ImplementsGenericInterface(this Type type, Type interfaceType)
            => type.IsGenericType(interfaceType) || type.GetTypeInfo().ImplementedInterfaces.Any(@interface => @interface.IsGenericType(interfaceType));

        private static bool IsGenericType(this Type type, Type genericType)
            => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }
}
