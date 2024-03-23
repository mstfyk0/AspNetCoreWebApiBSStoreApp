using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Repositories.EFCore;
using System.IO;

namespace WebApi.RepositoryFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoriesContext>
    {
        public RepositoriesContext CreateDbContext(string[] args)
        {
            //Configuration 
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //DbContextOptionBuilder
            var builder = new DbContextOptionsBuilder<RepositoriesContext>()
                .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                prj => prj.MigrationsAssembly("WebApi"));

            return new RepositoriesContext(builder.Options);

        }
    }
}
