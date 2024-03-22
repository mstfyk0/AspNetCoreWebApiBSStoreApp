using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Repositories.Contracts;

namespace WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSQLContext(this IServiceCollection services , IConfiguration configuration)  
        => services.AddDbContext<RepositoriesContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection") ) );

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped< IRepositoryManager,RepositoryManager>();
    }
}
