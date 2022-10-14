using FluentValidation.AspNetCore;
using Good.Admin.Entity;
using Good.Admin.Util;
using MicroElements.NSwag.FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Spectre.Console;
using System.Reflection;

namespace Good.Admin.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Assembly  _assembly = Assembly.GetExecutingAssembly();
            //TODO У�����ò���
#if DEBUG
            #region ��������Ϣ
            AnsiConsole.Write(new FigletText("Good.Admin").LeftAligned().Color(Color.Red));
            var rule = new Rule("[red]X2031[/]");
            rule.RuleStyle("green dim");
            rule.Alignment = Justify.Left;
            AnsiConsole.Write(rule);
            #endregion
#endif
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;
            IWebHostEnvironment environment = builder.Environment;
            var services = builder.Services;
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
                options.SuppressModelStateInvalidFilter = true;//����model��֤ʧ�ܺ���Զ�400��Ӧ
            })
            .AddNewtonsoftJson();
            //ע��FluentValidation ������֤
            services.AddFluentValidation(c =>
               {
                   c.RegisterValidatorsFromAssemblyContaining<Base_UsersInputDTOValidator>();
               });
            services.AddSqlsugarSetup(); //ע��Sqlsugar            
            services.AddFxServices();//�Զ�ע����Ҫע���            
            services.AddScoped<MyContext>();//ע��db������ط���
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
                   //settings.DocumentProcessors.Add(new FluentValidationDocumentProcessor());
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
                app.UseDeveloperExceptionPage();
            }
            #region ����
            app.UseCors(x =>
            {
                x.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .DisallowCredentials();
            });
            #endregion
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/octet-stream"
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
                endpoints.MapControllers();
            });
            app.UseOpenApi(); // serve OpenAPI/Swagger documents            
            var miniProfiler_headstream = _assembly.GetManifestResourceStream("Good.Admin.API.miniProfiler_head.js");
            // serve Swagger UI
            app.UseSwaggerUi3(async options =>
            {
                // Define web UI route
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