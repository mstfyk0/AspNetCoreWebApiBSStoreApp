
using WebApi.Extensions;
using Presentation;
using NLog;
using Services.Contracts;
using Microsoft.AspNetCore.Builder;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Services;
using AspNetCoreRateLimit;
var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(),"/nlog.config"));

// Add services to the container.

builder.Services.AddControllers(config =>
{
    //api sonucunun farkl� formatlarda �rne�in: csv, xml, vb formatlarda ��kt� al�nmas�na izin verilmesi.
    config.RespectBrowserAcceptHeader = true;
    //E�er desteklenmeyen bir formatta istek gelirse 406 koduyla hata vermesi sa�land�. 
    config.ReturnHttpNotAcceptable = true;
    //cache profile �zelli�ini entegre etme
    config.CacheProfiles.Add("5mins",new CacheProfile { Duration=300});
}
)
    .AddCustomCsvFormatter()
    //xml format�nda da ��kt� verme imkan�n� a��yoruz.
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(AssemblyReference).Assembly);
    //.AddNewtonsoftJson();

//ServiceExtension class�nda bunu tanmlad�k. bu sebeple sadece 52 sat�rdaki kod yeterli oluyor.
//builder.Services.AddScoped<ValidationFilterAttribute>(); //IOC

//varsay�lan davran�� de�erini de�i�tiriyoruz.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSQLContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureCors();
builder.Services.ConfigureDataShaper();
builder.Services.AddCustomMediaTypes();
builder.Services.AddScoped<IBookLinks, BookLinks>();
builder.Services.ConfigureVersioning();
//response caching yap�s�n haz�rland� ve IoC kayd� yap�ld�.
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
//service kayd�n� cors dan sonra yapmak tavsiye ediliyor.
app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();