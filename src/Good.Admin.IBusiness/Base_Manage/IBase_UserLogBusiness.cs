
using Good.Admin.Entity;
using Good.Admin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.IBusiness
{
    public interface IBase_UserLogBusiness
    {
        Task<PageResult<SystemLogDTO>> GetLogListAsync(PageInput<UserLogsInputDTO> input);
    }
}
