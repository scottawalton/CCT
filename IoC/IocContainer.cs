using System;
using Microsoft.Extensions.DependencyInjection;

namespace CCT
{
    /// <summary>
    /// Gives us a service provider we can use called provider
    /// (Currently supplies IServiceProvider)
    /// </summary>
    public static class IocContainer
    {
        public static IServiceProvider provider {get; set;}
    }

    /// <summary>
    /// Shorthand for calling the Service Provider
    /// </summary>
    public class Ioc 
    {
        /// <summary>
        /// scoped instance of the DatabaseContext
        /// </summary>
        /// <typeparam name="AppDBContext"></typeparam>
        /// <returns></returns>
        public AppDBContext AppDBContext = IocContainer.provider.GetService<AppDBContext>();
    }
}