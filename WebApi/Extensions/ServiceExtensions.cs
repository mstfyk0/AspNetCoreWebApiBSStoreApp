﻿using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;
using Repositories.Contracts;
using Services.Contracts;
using Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Presentation.ActionFilters;

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


        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();

        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination")
                );
            });

        }

    }
}
