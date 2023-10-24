using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Good.Admin.Common.AOP.Abstraction;
using Good.Admin.Common;

namespace Good.Admin.Business
{
    public abstract class WriteDataLogAttribute : BaseAOPAttribute
    {
        public WriteDataLogAttribute(UserLogType logType, string nameField, string dataName)
        {
            _logType = logType;
            _dataName = dataName;
            _nameField = nameField;
        }
        protected UserLogType _logType { get; }
        protected string _dataName { get; }
        protected string _nameField { get; }
        protected Type _entityType { get; }
    }
}
