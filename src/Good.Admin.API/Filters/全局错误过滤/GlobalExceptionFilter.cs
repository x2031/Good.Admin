using FluentValidation;
using Good.Admin.Util;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Good.Admin.API
{
    public class GlobalExceptionFilter : BaseActionFilterAsync, IAsyncExceptionFilter
    {
        readonly ILogger _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            Exception ex = context.Exception;
            if (ex is BusException busEx)
            {
                _logger.LogInformation(busEx.Message);
                context.Result = Error(busEx.Message, busEx.ErrorCode);
            }
            else if (ex is ValidationException validationException)
            {
                _logger.LogInformation(validationException.Message);
                context.Result = Error(validationException.Message, 500);
            }
            else
            {
                _logger.LogError(ex, "");
                context.Result = Error("系统繁忙");
            }
            await Task.CompletedTask;
        }
    }
}
