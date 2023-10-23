﻿using Good.Admin.Common.DataAccess;
using Good.Admin.Common.Helper;
using Good.Admin.Common;
using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using StackExchange.Profiling;

namespace Good.Admin.API
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    public static class SqlsugarExtentions
    {
        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());
        /// <summary>
        /// 注入SqlSugar
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 默认添加主数据库连接
            MainDb.CurrentDbConnId = Appsettings.app(new string[] { "MainDB" });

            // SqlSugarScope是线程安全，可使用单例注入
            // 参考：https://www.donet5.com/Home/Doc?typeId=1181
            services.AddSingleton<ISqlSugarClient>(o =>
            {
                var memoryCache = o.GetRequiredService<IMemoryCache>();
                // 连接字符串
                var listConfig = new List<ConnectionConfig>();
                // 从库
                var listConfig_Slave = new List<SlaveConnectionConfig>();
                BaseDBConfig.MutiConnectionString.slaveDbs.ForEach(s =>
                {
                    listConfig_Slave.Add(new SlaveConnectionConfig()
                    {
                        HitRate = s.HitRate,
                        ConnectionString = s.Connection
                    });
                });

                BaseDBConfig.MutiConnectionString.allDbs.ForEach(m =>
                {
                    listConfig.Add(new ConnectionConfig()
                    {
                        ConfigId = m.ConnId.ObjToString().ToLower(),
                        ConnectionString = m.Connection,
                        DbType = (DbType)m.DbType,
                        IsAutoCloseConnection = true,//是否自动关闭链接
                        // Check out more information: https://github.com/anjoy8/Blog.Core/issues/122
                        //IsShardSameThread = false,
                        AopEvents = new AopEvents
                        {
                            OnLogExecuting = (sql, p) =>
                            {
                                if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool())
                                {
                                    if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "OutToLogFile", "Enabled" }).ObjToBool())
                                    {
                                        Parallel.For(0, 1, e =>
                                        {
                                            MiniProfiler.Current.CustomTiming("SQL：", GetParas(p) + "【SQL语句】：" + sql);
                                            // LogLock.OutSql2Log("SqlLog", new string[] { GetParas(p), "【SQL语句】：" + sql });

                                        });
                                    }
                                    if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "OutToConsole", "Enabled" }).ObjToBool())
                                    {
                                        //ConsoleHelper.WriteColorLine(string.Join("\r\n", new string[] { "--------", "【SQL语句】：" + GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
                                        // Console.WriteLine(string.Join("\r\n", new string[] { "--------", "【SQL语句】：" + GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
                                    }
                                    Console.WriteLine("SQL：", GetParas(p) + "【SQL语句】：" + sql);
                                    Console.WriteLine(string.Join("\r\n", new string[] { "--------", "【SQL语句】：" + GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
                                }
                            },
                        },
                        MoreSettings = new ConnMoreSettings()
                        {
                            //IsWithNoLockQuery = true,
                            IsAutoRemoveDataCache = true
                        },
                        // 从库
                        SlaveConnectionConfigs = listConfig_Slave,
                        //TODO cache缓存数据 自定义特性
                        //ConfigureExternalServices = new ConfigureExternalServices()
                        //{
                        //    DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache),
                        //    EntityService = (property, column) =>
                        //    {
                        //        if (column.IsPrimarykey && property.PropertyType == typeof(int))
                        //        {
                        //            column.IsIdentity = true;
                        //        }
                        //    }
                        //},
                        InitKeyType = InitKeyType.Attribute
                    }
                   );
                });
                return new SqlSugarScope(listConfig);
            });
        }

        private static string GetWholeSql(SugarParameter[] paramArr, string sql)
        {
            foreach (var param in paramArr)
            {
                sql = sql.Replace(param.ParameterName, param.Value.ObjToString());
            }

            return sql;
        }

        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
        }
    }

}
