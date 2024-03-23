using Entities.ErrorModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using System.Net;

namespace WebApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication web
            ,ILoggerService logger)
        {
            web.UseExceptionHandler(
                appError =>
                {
                    appError.Run(
                        async context => 
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";
                            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if (contextFeature is not null)
                            {
                                logger.LogError($"Something went wrong : {contextFeature.Error}");
                                context.Response.WriteAsync(new ErrorDetails
                                {
                                    StatusCode = context.Response.StatusCode,
                                    Message = "Internal Server Error "
                                }.ToString());

                            }
                        });
                });

        }
    }
}
