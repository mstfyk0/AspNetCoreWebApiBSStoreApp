using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;
using Repositories.Contracts;
using Services.Contracts;
using Services;

namespace WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSQLContext(this IServiceCollection services , IConfiguration configuration)  
        => services.AddDbContext<RepositoriesContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection") ) );

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped< IRepositoryManager,RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerService, LoggerManager>();
                
    }
}
