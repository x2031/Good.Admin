using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Good.Admin.Common.AOP.Abstraction;
using Good.Admin.Common;
using Good.Admin.IBusiness;
using Good.Admin.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Good.Admin.Business
{
    public class DataEditLogAttribute : WriteDataLogAttribute
    {
        public DataEditLogAttribute(UserLogType logType, string nameField, string dataName)
            : base(logType, nameField, dataName)
        {
        }

        public override async Task After(IAOPContext context)
        {
            var op = context.ServiceProvider.GetService<IOperator>();
            var obj = context.Arguments[0];
              op.WriteUserLog(_logType, $"修改{_dataName}:{obj.GetPropertyValue(_nameField)?.ToString()}");
            await Task.CompletedTask;
        }
    }

}
