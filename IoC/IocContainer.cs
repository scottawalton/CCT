using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CCT
{
    /// <summary>
    /// Defines the units we use for the application (e.g. Service Provider, Configuration, etc..)
    /// (Currently supplies IServiceProvider for Service Provider)
    /// (Currently supplies IConfiguration for Configuration)
    /// </summary>
    public static class IocContainer
    {
        public static IServiceProvider provider {get; set;}
        public static IConfiguration Configuration {get; set;}
    }

    /// <summary>
    /// Shorthand for calling the Service Provider
    /// </summary>
    public class Ioc 
    {
        /// <summary>
        /// scoped instance of the DatabaseContext
        /// </summary>
        public AppDBContext AppDBContext = IocContainer.provider.GetService<AppDBContext>();
    }
}