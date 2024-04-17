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
using Microsoft.AspNetCore.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System;

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
                    Limit=60,
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
        public static void ConfigureIdentity (this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength =6;
                    options.User.RequireUniqueEmail = true;
                }) 
                .AddEntityFrameworkStores<RepositoriesContext>()
                .AddDefaultTokenProviders();
        }
        //Jwt bölümü
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("JwtSettings");
            var secretKey = jwtSetting["secretKey"];

            services.AddAuthentication(opt =>
            {
                //Shema tanımlanmayınca ve controller methodunda authentication kullanıyorsan o methodu çağırdığında 404 not found hatası alırsın.
                //Burada varsayılan bir şemayı kullandırtıyor. Buna göre de appsetttings ayarlıyabiliyoruz.
                //Bu kod ile başka methodlar da authentication çalıştırdığımızda belirli bir şemayı kabul etmesini sağlıyoruz. BU olmaz ise 401 unauthorize dönüyor methotlar.
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // Authentication şemasının eklenmesi için bu   opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; configurasyon işe yarıyor. 
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }

            ).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //  jwtsettings["buraya girilen parametre adı hatalı girildiyse  "String reference not set to an instance of a String. (Parameter 's')" şeklinde hata verir."]
                    ValidIssuer = jwtSetting["validIssuer"],
                    ValidAudience = jwtSetting["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                }
            );

        }
        //Jwt bölümü Son

        public static void ConfigureSwagger (this IServiceCollection services)
        {

            services.AddSwaggerGen(

                s =>
                {
                    s.SwaggerDoc("v1", new OpenApiInfo { Title = "Btk Akademi", Version = "v1"
                    
                        ,Description="BTK Akademi ASP.Net Core Web API"
                        ,TermsOfService = new Uri("https://www.btkakademi.gov.tr")
                        ,Contact = new OpenApiContact()
                        {
                            Name="Mustafa Yiğit KARAKOCA"
                            ,Email= "ygt99krk@gmail.com"
                            ,Url= new Uri("https://github.com/mstfyk0")

                        }


                    }
                    
                    
                    );
                    s.SwaggerDoc("v2", new OpenApiInfo { Title = "Btk Akademi Version 2", Version = "v2" });

                    s.AddSecurityDefinition("Bearer"
                        , new OpenApiSecurityScheme()
                        {
                            In = ParameterLocation.Header,
                            Description = "Place to add JWT with Bearer",
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer"
                        });

                    s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference= new OpenApiReference
                                {

                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                                ,Name="Bearer"

                            },new List<string >()
                        }
                    });
                });
        }
    }
}