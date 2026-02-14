using BluntServe.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BluntServe.Filters
{
    public class LogFilter : IAsyncActionFilter
    {
        private readonly ILogService _logService; 

        public LogFilter(ILogService logService)
        {
            _logService = logService;
        }
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
