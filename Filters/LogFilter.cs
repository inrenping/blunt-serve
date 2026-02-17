using BluntServe.Attributes;
using BluntServe.Interfaces;
using BluntServe.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Security.Claims;

namespace BluntServe.Filters
{
    public class LogFilter : IAsyncActionFilter
    {
        private readonly ILogService _logService; 

        public LogFilter(ILogService logService)
        {
            _logService = logService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.HttpContext.Request;
            var logAttribute = context.ActionDescriptor.EndpointMetadata.OfType<LogAttribute>().FirstOrDefault();
            var executedContext = await next();
            stopwatch.Stop();
            if (logAttribute != null)
            {
                var log = new SysLog
                {
                    UserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    UserName = context.HttpContext.User.Identity?.Name,
                    LogType = "OPERATE",
                    ModuleName = logAttribute.Module,
                    OpDesc = logAttribute.Operation,
                    ReqUrl = request.Path + request.QueryString,
                    ReqMethod = request.Method,
                    IpAddress = context.HttpContext.Connection.RemoteIpAddress,
                    UserAgent = request.Headers["User-Agent"].ToString(),
                    DurationMs = (int)stopwatch.ElapsedMilliseconds,
                    CreatedAt = DateTime.UtcNow,
                };

                await _logService.SaveLogAsync(log);
            }
        }
    }
}
