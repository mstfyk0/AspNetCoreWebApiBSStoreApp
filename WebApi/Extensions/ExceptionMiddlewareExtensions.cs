using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Services.Contracts;

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
                            context.Response.ContentType = "application/json";
                            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if (contextFeature is not null)
                            {
                                context.Response.StatusCode = contextFeature.Error switch
                                {
                                    NotFoundException => StatusCodes.Status404NotFound,
                                    _ => StatusCodes.Status500InternalServerError

                                };

                                logger.LogError($"Something went wrong : {contextFeature.Error}");
                                context.Response.WriteAsync(new ErrorDetails
                                {
                                    StatusCode = context.Response.StatusCode,
                                    Message = contextFeature.Error.Message
                                }.ToString());

                            }
                        });
                });

        }
    }
}
