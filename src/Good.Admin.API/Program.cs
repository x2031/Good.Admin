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
            #region ��������Ϣ
            AnsiConsole.Write(new FigletText("Good.Admin").LeftJustified().Color(Color.Red));
            var rule = new Rule();
            rule.RuleStyle("green dim");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);
            #endregion
#endif
            //TODO У�����ò���
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;
            IWebHostEnvironment environment = builder.Environment;
            var services = builder.Services;
            //ѩ��id
            builder.Host.UseIdHelper();
            builder.Host.UseCache();
            builder.Host.ConfigureLoggingDefaults();
            builder.Host.UseDefaultServiceProvider(options => options.ValidateScopes = true);

            services.AddMvcCore();
            services.AddSingleton(new Appsettings(configuration));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers(options =>
            {
                options.Filters.Add<ValidFilterAttribute>();//����У��
                options.Filters.Add<GlobalExceptionFilter>();//ȫ���쳣
            }).ConfigureApiBehaviorOptions(options =>
            {
                //����model��֤ʧ�ܺ���Զ�400��Ӧ
                options.SuppressModelStateInvalidFilter = true;
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            //ע��FluentValidation ������֤
            services.AddFluentValidation(c =>
               {
                   c.RegisterValidatorsFromAssemblyContaining<Base_UsersDTOValidator>();
               });
            //ע��Sqlsugar 
            services.AddSqlsugarSetup();
            //�Զ�ע����Ҫע���
            services.AddFxServices();
            services.AddScoped<MyContext>();//ע��db������ط���
            services.AddHttpContextAccessor();
            #region ���MiniProfiler����
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";// �趨���ʷ������URL��·�ɻ���ַ
            });
            #endregion
            #region Openapi���
            services.AddOpenApiDocument((settings, serviceProvider) =>
               {
                   var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider.GetService<FluentValidationSchemaProcessor>();
                   // Add the fluent validations schema processor
                   settings.SchemaProcessors.Add(fluentValidationSchemaProcessor);
                   settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
                   settings.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                   {
                       Description = "�������JWT token����",
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
                //����������Ա����ҳ��
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
            app.UseMiniProfiler();//���ܼ��            
            app.UseMiddleware<RequestBodyMiddleware>();//��ʽ�������м��            
            app.UseMiddleware<RequestLogMiddleware>();//Log�м��            
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
                options.Path = "/api"; //����·��
                //�۵����з���
                options.DocExpansion = "list";
                options.DefaultModelsExpandDepth = -1; //����models
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
            // ������������
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