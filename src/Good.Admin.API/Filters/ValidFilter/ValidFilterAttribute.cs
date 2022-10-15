using Microsoft.AspNetCore.Mvc.Filters;

namespace Good.Admin.API
{
    public class ValidFilterAttribute : BaseActionFilterAsync
    {
        // TODO 尝试能否全局自动验证
        public override async Task OnActionExecuting(ActionExecutingContext context)
        {
            //判断是否需要校验
            if (context.ContainsFilter<IgnoreVaildAttribute>())
                return;


            if (!context.ModelState.IsValid)
            {
                var msgList = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                context.Result = Error(string.Join(",", msgList), 500);
            }
            await Task.CompletedTask;
        }
    }
}
