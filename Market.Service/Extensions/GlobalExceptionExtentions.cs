using Market.Domain.Exceptions;
using Market.Domain.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Market.Service.Extensions
{
    public static class GlobalExceptionExtentions
    {
        public static void UseGlobalExceptionProcess(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(async (httpContext) =>
            {
                var logger = httpContext.RequestServices.GetService<ILogger>();
                var exceptionFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionFeature.Error;

                var resp = new BaseResponse<object>();
                resp.Code = -1;

                if (exception is ValidateException)
                {
                    logger?.LogInformation(exception, exception?.Message);
                    resp.Message = exception?.Message;
                }
                else
                {
                    logger?.LogError(exception, exception?.Message);
                    resp.Message = "Something went wrong. Please try again!";
                }
                var jsonRes = JsonSerializer.Serialize(resp);
                await httpContext.Response.WriteAsync(jsonRes);
            });
        }
    }
}
