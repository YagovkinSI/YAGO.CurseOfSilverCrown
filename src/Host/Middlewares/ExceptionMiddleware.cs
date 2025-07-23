using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Host.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            var yagoWorldException = ex as YagoException;

            if (yagoWorldException != null)
                _logger.LogWarning(ex, ex.Message);
            else
                _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = yagoWorldException?.ErrorCode ?? 500;
            context.Response.ContentType = "application/json";
            var message = yagoWorldException?.Message ?? "Неизвестная ошибка";
            var result = new
            {
                Title = message,
                Status = context.Response.StatusCode,
                Errors = new Dictionary<string, string>
                {
                    { "Exception", ex.Message },
                    { "StackTrace", ex.StackTrace ?? "NULL" }
                },
                TraceId = context.TraceIdentifier
            };
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}