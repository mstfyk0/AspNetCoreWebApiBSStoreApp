using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;
using Repositories.Contracts;
using Services.Contracts;
using Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Presentation.ActionFilters;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Presentation.Controllers;
using Marvin.Cache.Headers;
using System.Collections.Generic;
using AspNetCoreRateLimit;

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
            services.AddScoped<ValidateMediaTypeAttribute>();   


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
        public static void ConfigureDataShaper(this IServiceCollection services)
        {

            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();

        }
        //OfType filtreme yapmak için kullanılmaktadır.
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters.OfType<SystemTextJsonOutputFormatter>()?
                .FirstOrDefault();

                if (systemTextJsonOutputFormatter is not null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.hateoas+json");

                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.apiroot+json");
                }

                var xmlOutputFormatter = config
                .OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

                if (xmlOutputFormatter is not null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.hateoas+xml");

                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.btkakademi.apiroot+xml");
                }
            });
        }

        public static void ConfigureVersioning (this IServiceCollection services)
        {

            services.AddApiVersioning(opt=>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified=true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
                //Controllerda bulunan api bilgisinin Conventions yapısı ile servicleri tanımladığımız yerde kullanabiliyoruz.
                opt.Conventions.Controller<BooksController>()
                    .HasApiVersion(new ApiVersion(1, 0));
                //Controllerda bulunan api bilgisinin Conventions yapısı ile servicleri tanımladığımız yerde kullanabiliyoruz. Deprecate edilen api versiyonuda aynı şekilde yapabiliyoruz. HasDeprecatedApiVersion kullanrak.
                opt.Conventions.Controller<BooksV2Controller>()
                   .HasDeprecatedApiVersion(new ApiVersion(2, 0));
            }
            );
        }

        //Api caching mekanizmasını entegre ediyoruz.
        public static void ConfigureResponseCaching(this IServiceCollection services) =>

            services.AddResponseCaching();

        public static void  ConfigureHttpCacheHeaders(this IServiceCollection services) =>
            services.AddHttpCacheHeaders(expirationOpt =>
            {

                expirationOpt.MaxAge = 90;
                expirationOpt.CacheLocation = CacheLocation.Public;
            },
                validationOPt =>
                {
                    validationOPt.MustRevalidate = false;
                }

            );

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
                new RateLimitRule()
                {
                    Endpoint="*",
                    Limit=3,
                    Period="1m"

                }
            };

            services.Configure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = rateLimitRules;
            }); 

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();  
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();  
            services.AddSingleton<IRateLimitConfiguration,  RateLimitConfiguration>();  
            services.AddSingleton<IProcessingStrategy , AsyncKeyLockProcessingStrategy>();  
        }
    }
}