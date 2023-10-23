﻿using Good.Admin.Common;
using Good.Admin.Common.AOP.Abstraction;
using Good.Admin.Common.Primitives;
using Good.Admin.IBusiness;
using Microsoft.Extensions.DependencyInjection;

namespace Good.Admin.Business
{
    public class DataAddLogAttribute : WriteDataLogAttribute
    {
        public DataAddLogAttribute(UserLogType logType, string nameField, string dataName)
            : base(logType, nameField, dataName)
        {
        }

        public async override Task After(IAOPContext context)
        {
            var op = context.ServiceProvider.GetService<IOperator>();
            var obj = context.Arguments[0];
            op.WriteUserLog(_logType, $"添加{_dataName}:{obj.GetPropertyValue(_nameField)?.ToString()}");

            await Task.CompletedTask;
        }
    }
}
