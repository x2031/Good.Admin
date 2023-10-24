using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using StackExchange.Redis;
using System.Reflection;
using Yitter.IdGenerator;

namespace Good.Admin.Common
{
    public static partial class Extention
    {
        /// <summary>
        /// 使用YitIdHelper-雪花id
        /// </summary>
        /// <param name="hostBuilder">建造者</param>
        /// <returns></returns>
        public static IHostBuilder UseIdHelper(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((buidler, services) =>
           {
               var options = new IdGeneratorOptions(ushort.Parse(buidler.Configuration["WorkerId"].ToString()));
               IdHelper.SetIdGenerator(options);
           });

            return hostBuilder;
        }
        /// <summary>
        /// 使用缓存-Memory或Redis
        /// </summary>
        /// <param name="hostBuilder">建造者</param>
        /// <returns></returns>
        public static IHostBuilder UseCache(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((buidlerContext, services) =>
            {
                var cacheOption = buidlerContext.Configuration.GetSection("Cache").Get<CacheOptions>();
                switch (cacheOption.CacheType)
                {
                    case CacheType.Memory:
                        {
                            services.AddDistributedMemoryCache();
                        }; break;

                    case CacheType.Redis:
                        {
                            services.AddTransient<IRedisBasketRepository, RedisBasketRepository>();

                            var configuration = ConfigurationOptions.Parse(cacheOption.ConnectionString, true);
                            configuration.ResolveDns = true;
                            ConnectionMultiplexer stackRedis = ConnectionMultiplexer.Connect(configuration);

                            services.AddSingleton<IConnectionMultiplexer>(stackRedis);
                        }; break;
                    default: throw new Exception("缓存类型无效");
                }
            });
            return hostBuilder;
        }
        /// <summary>
        /// 配置日志-默认Serilog
        /// </summary>
        /// <param name="hostBuilder">建造者</param>
        /// <returns></returns>
        public static IHostBuilder ConfigureLoggingDefaults(this IHostBuilder hostBuilder)
        {
            var rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var path = Path.Combine(rootPath, "logs", "log.txt");
            SelfLog.Enable(Console.Error);

            return hostBuilder.UseSerilog((hostingContext, serviceProvider, serilogConfig) =>
            {
                var envConfig = hostingContext.Configuration;
                LogOptions logConfig = new LogOptions();
                envConfig.GetSection("log").Bind(logConfig);
                logConfig.Overrides.ForEach(aOverride =>
                {
                    serilogConfig
                        .MinimumLevel
                        .Override(aOverride.Source, (LogEventLevel)aOverride.MinLevel);
                });
                serilogConfig.MinimumLevel.Is((LogEventLevel)logConfig.MinLevel);
                //是否输出到控制台
                if (logConfig.Console.Enabled)
                {
                    serilogConfig.WriteTo.Console();
                }
                //是否开启Debug模式
                if (logConfig.Debug.Enabled)
                {
                    serilogConfig.WriteTo.Debug();
                }
                //是否输出到文件中
                if (logConfig.File.Enabled)
                {
                    string template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3} {SourceContext:l}] {Message:lj}{NewLine}{Exception}";

                    serilogConfig.WriteTo.File(
                        path,
                        outputTemplate: template,
                        rollingInterval: RollingInterval.Day,
                        shared: true,
                        fileSizeLimitBytes: 10 * 1024 * 1024,
                        rollOnFileSizeLimit: true
                        );
                }
                //是否输出到Elasticsearch中0
                if (logConfig.Elasticsearch.Enabled)
                {
                    var uris = logConfig.Elasticsearch.Nodes.Select(x => new Uri(x)).ToList();
                    serilogConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(uris)
                    {
                        IndexFormat = logConfig.Elasticsearch.IndexFormat,
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        InlineFields = true,
                    });
                }
                //自定义属性
                serilogConfig.Enrich.WithProperty("MachineName", Environment.MachineName);
                serilogConfig.Enrich.WithProperty("ApplicationName", Assembly.GetEntryAssembly().GetName().Name);
                serilogConfig.Enrich.WithProperty("ApplicationVersion", Assembly.GetEntryAssembly().GetName().Version);
                serilogConfig.Enrich.WithProperty("ThreadId", Thread.CurrentThread.ManagedThreadId);

                var httpContext = serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
                if (httpContext != null)
                {
                    serilogConfig.Enrich.WithProperty("Operator", httpContext.User.Claims.Where(x => x.Type == "userId").FirstOrDefault()?.Value);
                    serilogConfig.Enrich.WithProperty("RequestPath", httpContext.Request.Path);
                    //serilogConfig.Enrich.WithProperty("RequestIp", httpContext.Connection.RemoteIpAddress);
                }
            });
        }
    }
}
