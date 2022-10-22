using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Good.Admin.API
{
    public static class SerilogExtentions
    {
        /// <summary>
        /// 配置日志
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
                //是否输出到Elasticsearch中
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
