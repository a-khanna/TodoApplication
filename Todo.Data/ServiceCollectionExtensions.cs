using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Todo.Core.Abstractions.Data;
using Todo.Data.Repositories;

namespace Todo.Data
{
    /// <summary>
    /// Extensions for service collection registrations
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers dependencies for the data layer
        /// </summary>
        /// <param name="services">Service Collection object</param>
        public static void RegisterDataDependencies(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TodoDbContext>(opts => opts.UseSqlServer(connectionString));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();
        }
    }
}
