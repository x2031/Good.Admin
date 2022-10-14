using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.Util
{
    public static class GlobalAssemblies
    {
        /// <summary>
        /// 解决方案所有程序集
        /// </summary>
        public static readonly Assembly[] AllAssemblies = new Assembly[]
        {
            Assembly.Load("Good.Admin.Repository"),
            Assembly.Load("Good.Admin.Entity"),
            Assembly.Load("Good.Admin.Util"),
            Assembly.Load("Good.Admin.IBusiness"),
            Assembly.Load("Good.Admin.Business"),
            Assembly.Load("Good.Admin.Api"),
    
        };

        /// <summary>
        /// 解决方案所有自定义类
        /// </summary>
        public static readonly Type[] AllTypes = AllAssemblies.SelectMany(x => x.GetTypes()).ToArray();

        /// <summary>
        /// 超级管理员UserIId
        /// </summary>
        public const string ADMINID = "Admin";
    }
}
