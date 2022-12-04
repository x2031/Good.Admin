using Microsoft.AspNetCore.Mvc.Filters;

namespace Good.Admin.API
{
    /// <summary>
    /// 全局参数验证
    /// </summary>
    public class ValidFilterAttribute : BaseActionFilterAsync
    {
        public override async Task OnActionExecuting(ActionExecutingContext context)
        {
            //判断是否需要校验
            if (context.ContainsFilter<IgnoreVaildAttribute>())
                return;

            //判断验证是否通过，并返回错误信息
            if (!context.ModelState.IsValid)
            {
                var msgList = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                context.Result = Error(string.Join(",", msgList), 500);
            }
            await Task.CompletedTask;
        }
    }
}
