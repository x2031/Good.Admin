using Good.Admin.Common;
using Good.Admin.Common.AOP.Abstraction;
using Good.Admin.Common.Primitives;
using Good.Admin.IBusiness;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Dynamic.Core;

namespace Good.Admin.Business
{
    public class DataDeleteLogAttribute : WriteDataLogAttribute
    {
        public DataDeleteLogAttribute(UserLogType logType, string nameField, string dataName)
            : base(logType, nameField, dataName)
        {
        }

        private string _names;
        public async override Task Befor(IAOPContext context)
        {
            List<string> ids = context.Arguments[0] as List<string>;
            var q = context.InvocationTarget.GetType().GetMethod("GetIQueryable").Invoke(context.InvocationTarget, new object[] { }) as IQueryable;
            var deleteList = q.Where("@0.Contains(Id)", ids).CastToList<object>();

            _names = string.Join(",", deleteList.Select(x => x.GetPropertyValue(_nameField)?.ToString()));

            await Task.CompletedTask;
        }
        public async override Task After(IAOPContext context)
        {
            var op = context.ServiceProvider.GetService<IOperator>();

            op.WriteUserLog(_logType, $"删除{_dataName}:{_names}");

            await Task.CompletedTask;
        }
    }

}
