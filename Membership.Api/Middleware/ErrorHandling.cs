using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Membership.Core;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Membership.Api.Middleware
{
    /// <summary>
    /// Custom error handling Middleware,
    /// ensures the errors are logged
    /// </summary>
    public class ErrorHandling
    {
        readonly ILogger<ErrorHandling> _logger;
        readonly RequestDelegate _next;

        public ErrorHandling(
            ILogger<ErrorHandling> logger,
            RequestDelegate next)
        {
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
            _next = next 
                ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToStringWithInnerExceptions());
                if (false == context.Response.HasStarted)
                {
                    await WriteErrorResponse(context);
                }
            }
        }

        async Task WriteErrorResponse(HttpContext context)
        {
            context.Response.StatusCode = 500;
            try
            {
                await context.Response.WriteAsync(
                    "Internal Server Error",
                    CancellationToken.None
                );
            } catch (Exception) { /* discard */ }
        }
    }
}
