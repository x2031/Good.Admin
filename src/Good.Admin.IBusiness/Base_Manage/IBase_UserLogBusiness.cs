
using Good.Admin.Common;
using Good.Admin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.IBusiness
{
    public interface IBase_UserLogBusiness
    {
        Task<PageResult<SystemLogDTO>> GetLogListAsync(PageInput<UserLogsDTO> input);
    }
}
