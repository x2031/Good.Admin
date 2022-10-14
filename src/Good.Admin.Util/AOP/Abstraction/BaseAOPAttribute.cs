﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.Util
{
    /// <summary>
    /// AOP基类
    /// 注:不支持控制器,需要定义接口并实现接口,自定义AOP特性放到接口实现类上
    /// </summary>
    public abstract class BaseAOPAttribute : Attribute
    {
        public virtual async Task Befor(IAOPContext context)
        {
            await Task.CompletedTask;
        }

        public virtual async Task After(IAOPContext context)
        {
            await Task.CompletedTask;
        }
    }
}
