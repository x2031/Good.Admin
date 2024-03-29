﻿using Good.Admin.Common;
using Good.Admin.IBusiness;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Good.Admin.API
{
    /// <summary>
    /// 接口权限校验
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ApiPermissionAttribute : BaseActionFilterAsync
    {
        /// <summary>
        /// 权限值
        /// </summary>
        /// <param name="permissionValue"></param>
        /// <exception cref="Exception"></exception>
        public ApiPermissionAttribute(string permissionValue)
        {
            if (permissionValue.IsNullOrEmpty())
                throw new Exception("permissionValue不能为空");

            _permissionValue = permissionValue;
        }

        public string _permissionValue { get; }

        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="context">过滤器上下文</param>
        public async override Task OnActionExecuting(ActionExecutingContext context)
        {
            var permissions = new List<string>();
            if (context.ContainsFilter<NoApiPermissionAttribute>())
                return;

            IServiceProvider serviceProvider = context.HttpContext.RequestServices;

            IPermissionBusiness _permissionBus = serviceProvider.GetService<IPermissionBusiness>();
            IOperator _operator = serviceProvider.GetService<IOperator>();

            if (_permissionBus != null && _operator != null && !string.IsNullOrEmpty(_operator?.UserId))
            {
                permissions = await _permissionBus.GetUserPermissionValuesAsync(_operator.UserId);
            }

            if (!permissions.Contains(_permissionValue))
                context.Result = Error("权限不足!");
        }
    }
}
