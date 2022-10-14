﻿using Good.Admin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.IBusiness
{
    public interface IPermissionBusiness
    {
        Task<List<string>> GetUserPermissionValuesAsync(string userId);
        Task<List<Base_ActionDTO>> GetUserMenuListAsync(string userId);
    }
}
