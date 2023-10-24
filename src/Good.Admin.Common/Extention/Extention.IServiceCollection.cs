using Castle.DynamicProxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nest;
using SqlSugar;
using StackExchange.Profiling;
using System.Text;

namespace Good.Admin.Common
{
    public static partial class Extention
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();
        /// <summary>
        /// 注入继承ITransientDependency、IScopeDependency、ISingletonDependency的服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns></returns>
        public static IServiceCollection AddFxServices(this IServiceCollection services)
        {
            Dictionary<Type, ServiceLifetime> lifeTimeMap = new Dictionary<Type, ServiceLifetime>
            {
                { typeof(ITransientDependency), ServiceLifetime.Transient},
                { typeof(IScopedDependency),ServiceLifetime.Scoped},
                { typeof(ISingletonDependency),ServiceLifetime.Singleton}
            };

            GlobalAssemblies.AllTypes.ForEach(aType =>
            {
                lifeTimeMap.ToList().ForEach(aMap =>
                {
                    var theDependency = aMap.Key;
                    if (theDependency.IsAssignableFrom(aType) && theDependency != aType && !aType.IsAbstract && aType.IsClass)
                    {
                        //注入实现
                        services.Add(new ServiceDescriptor(aType, aType, aMap.Value));
                        var interfaces = GlobalAssemblies.AllTypes.Where(x => x.IsAssignableFrom(aType) && x.IsInterface && x != theDependency).ToList();
                        //有接口则注入接口
                        if (interfaces.Count > 0)
                        {
                            interfaces.ForEach(aInterface =>
                            {
                                //注入AOP
                                services.Add(new ServiceDescriptor(aInterface, serviceProvider =>
                                {
                                    CastleInterceptor castleInterceptor = new CastleInterceptor(serviceProvider);
                                    return _generator.CreateInterfaceProxyWithTarget(aInterface, serviceProvider.GetService(aType), castleInterceptor);
                                }, aMap.Value));
                            });
                        }
                        //无接口则注入自己
                        else
                        {
                            services.Add(new ServiceDescriptor(aType, aType, aMap.Value));
                        }
                    }
                });
            });
            return services;
        }
        /// <summary>
        /// 注入ElasticClient
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddElasticClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LogOptions>(configuration.GetSection("log"));
            var logOptions = configuration.GetSection("log").Get<LogOptions>();
            if (logOptions.Elasticsearch.Enabled)
            {
                var connectionSettings = ElasticsearchHelper.CreateElasticsearchConnStr(logOptions);
                services.AddSingleton<IElasticClient>(new ElasticClient(connectionSettings));
            }
            return services;
        }
        /// <summary>
        ///  注入Jwt
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                //Token Validation Parameters
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,//到时间立即过期
                    ValidateIssuerSigningKey = true,
                    //获取或设置要使用的Microsoft.IdentityModel.Tokens.SecurityKey用于签名验证。
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
                    GetBytes(jwtOptions.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            return services;
        }
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
                //var memoryCache = o.GetRequiredService<IMemoryCache>();
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
                                            MiniProfiler.Current.CustomTiming("SQL：", SqlsugarHelper.GetParas(p) + "【SQL语句】：" + sql);
                                            // LogLock.OutSql2Log("SqlLog", new string[] { GetParas(p), "【SQL语句】：" + sql });
                                        });
                                    }
                                    if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "OutToConsole", "Enabled" }).ObjToBool())
                                    {
                                        //ConsoleHelper.WriteColorLine(string.Join("\r\n", new string[] { "--------", "【SQL语句】：" + GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
                                        // Console.WriteLine(string.Join("\r\n", new string[] { "--------", "【SQL语句】：" + GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
                                    }
                                    Console.WriteLine("SQL：", SqlsugarHelper.GetParas(p) + "【SQL语句】：" + sql);
                                    Console.WriteLine(string.Join("\r\n", new string[] { "--------", "【SQL语句】：" + SqlsugarHelper.GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
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
    }
}
