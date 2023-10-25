using FluentValidation.AspNetCore;
using Good.Admin.Common;
using Good.Admin.Entity;
using MicroElements.NSwag.FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json.Serialization;
using NSwag;
using NSwag.Generation.Processors.Security;
using Spectre.Console;
using System.Reflection;
namespace Good.Admin.API
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();

#if DEBUG
            #region 输出框架信息
            AnsiConsole.Write(new FigletText("Good.Admin").LeftJustified().Color(Color.Red));
            var rule = new Rule();
            rule.RuleStyle("green dim");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);
            #endregion
#endif
            //TODO 校验配置参数
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;
            IWebHostEnvironment environment = builder.Environment;
            var services = builder.Services;
            //雪花id
            builder.Host.UseIdHelper();
            builder.Host.UseCache();
            builder.Host.ConfigureLoggingDefaults();
            builder.Host.UseDefaultServiceProvider(options => options.ValidateScopes = true);

            services.AddMvcCore();
            services.AddSingleton(new Appsettings(configuration));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers(options =>
            {
                options.Filters.Add<ValidFilterAttribute>();//参数校验
                options.Filters.Add<GlobalExceptionFilter>();//全局异常
            }).ConfigureApiBehaviorOptions(options =>
            {
                //禁用model验证失败后的自动400响应
                options.SuppressModelStateInvalidFilter = true;
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            //注入FluentValidation 参数验证
            services.AddFluentValidation(c =>
               {
                   c.RegisterValidatorsFromAssemblyContaining<Base_UsersDTOValidator>();
               });
            //注入Sqlsugar 
            services.AddSqlsugarSetup();
            //自动注入需要注入的
            services.AddFxServices();
            services.AddScoped<MyContext>();//注入db启动相关服务
            services.AddHttpContextAccessor();
            #region 添加MiniProfiler服务
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";// 设定访问分析结果URL的路由基地址
            });
            #endregion
            #region Openapi相关
            services.AddOpenApiDocument((settings, serviceProvider) =>
               {
                   var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider.GetService<FluentValidationSchemaProcessor>();
                   // Add the fluent validations schema processor
                   settings.SchemaProcessors.Add(fluentValidationSchemaProcessor);
                   settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
                   settings.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                   {
                       Description = "输入你的JWT token令牌",
                       Type = OpenApiSecuritySchemeType.Http,
                       In = OpenApiSecurityApiKeyLocation.Header,
                       Scheme = JwtBearerDefaults.AuthenticationScheme,
                       BearerFormat = "JWT",
                   });
               });
            services.AddScoped<FluentValidationSchemaProcessor>();
            #endregion
            #region jwt
            services.Configure<JwtOption>(configuration.GetSection(typeof(JwtOption).Name));
            services.AddJwt(configuration);
            #endregion
            //ElasticExtentions
            //services.AddElasticClient(configuration);
            var app = builder.Build();
            if (environment.IsDevelopment())
            {
                //启动开发人员错误页面
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/octet-stream"
            });
            app.UseCors(x =>
            {
                x.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .DisallowCredentials();
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiniProfiler();//性能检测            
            app.UseMiddleware<RequestBodyMiddleware>();//格式化返回中间件            
            app.UseMiddleware<RequestLogMiddleware>();//Log中间件            
            #region SwaggerUI
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
            // serve OpenAPI/Swagger documents              
            app.UseOpenApi(settings =>
            {
                settings.PostProcess = (document, request) =>
                {
                    document.Info.Title = "Good.Admin.API";
                };
            });
            var miniProfiler_headstream = _assembly.GetManifestResourceStream("Good.Admin.API.miniProfiler_head.js");
            // serve Swagger UI
            app.UseSwaggerUi3(async options =>
            {
                // Define web UI route
                options.DocumentTitle = "Good.Admin.API";
                options.Path = "/api"; //访问路径
                //折叠所有方法
                options.DocExpansion = "list";
                options.DefaultModelsExpandDepth = -1; //隐藏models
                options.CustomJavaScriptPath = "/js/MiniProfiler_head.js";
                options.CustomHeadContent = await miniProfiler_headstream.ReadToStringAsync();
            });
            // serve ReDoc UI
            app.UseReDoc(options =>
            {
                options.Path = "/api_redoc";
            });
            #endregion

#if DEBUG
            // 生成种子数据
            //var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            //var myContext = scope.ServiceProvider.GetRequiredService<MyContext>();
            //app.UseSeedDataMiddle(myContext, environment.WebRootPath);
            app.Run();
#endif
#if NETCOREAPP
            app.Run(configuration["WebRootUrl"]);
#endif

        }
    }
}