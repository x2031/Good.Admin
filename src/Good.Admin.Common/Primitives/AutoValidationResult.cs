using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace Good.Admin.Common
{
    public class AutoValidationResult : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            //TODO 自定义返回数据
            return new BadRequestObjectResult(new { Title = "Validation errors", ValidationErrors = validationProblemDetails?.Errors });
        }
    }
}
