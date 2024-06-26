
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
    config.CacheProfiles.Add("5mins", new CacheProfile { Duration = 300 });
}
)
    .AddCustomCsvFormatter()
    //.AddXmlDataContractSerializerFormatters()
    //xml format�nda da ��kt� verme imkan�n� a��yoruz.
    .AddApplicationPart(typeof(AssemblyReference).Assembly)
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    }
    );

//ServiceExtension class�nda bunu tanmlad�k. bu sebeple sadece 52 sat�rdaki kod yeterli oluyor.
//builder.Services.AddScoped<ValidationFilterAttribute>(); //IOC

//varsay�lan davran�� de�erini de�i�tiriyoruz.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

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
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

builder.Services.ConfigureIdentity();
//Jwt b�l�m�
//Eklenilen bir �zelli�in program a��l�nca direk entegre olmas�n� sa�lanmazsa program o methodlar�n ve �zelliklerin var olup olmad���na dair bir bilgisi olmaz. Bu sebeple Jwt entegrasyonun ger�ekle�tirip program cs dosyas�na service kayd�n� yapmazsan controllerda [Authorize] diye kullan�lan methodlar i�in �al��ma ger�ekle�micektir. 
//Jwt eklendi service extension class�na kayd� yap�ld� ama program cse eklenmedi�inde 404 not found diye hata al�yoruz.
builder.Services.ConfigureJWT(builder.Configuration);
//builder.Services.ConfigureJWT();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        s=>
        {

            s.SwaggerEndpoint("/swagger/v1/swagger.json", "Btk Akademi");
            s.SwaggerEndpoint("/swagger/v2/swagger.json", "Btk Akademi v2");
        }
        );
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