
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    public static class ServiceCollectionExtensions
    {
        public static void Configure<TConfig>(this IServiceCollection services, string name, IConfigurationSection section) where TConfig : class
        {
            var config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            section.Bind(config);
            services.Configure<TConfig>(name, options =>
            {
                section.Bind(options);
            });
        }
        public static void Configure<TConfig>(this IServiceCollection services, IConfigurationSection section) where TConfig : class
        {
            var config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            section.Bind(config);
            services.Configure<TConfig>(options =>
            {
                section.Bind(options);
            });
        }
    }
}
