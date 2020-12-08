using Microsoft.Extensions.DependencyInjection;
using Todo.Core.Abstractions.Logic;
using Todo.Core.Logic;

namespace Todo.Core
{
    /// <summary>
    /// Extensions for service collection registrations
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers dependencies for the logic layer
        /// </summary>
        /// <param name="services">Service Collection object</param>
        public static void RegisterLogicDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserLogic, UserLogic>();
        }
    }
}
