//using AutoMapper;
using Castle.DynamicProxy;
using Good.Admin.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

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
                services.AddSingleton<IElasticClient>(new ElasticClient(CreateconnentionSettiongs(logOptions)));
            }

            return services;
        }
    }
}
