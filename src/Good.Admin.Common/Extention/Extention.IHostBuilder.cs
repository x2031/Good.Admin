﻿namespace Good.Admin.Common.Extention
{
    public static partial class Extention
    {
        /// <summary>
        /// 使用YitIdHelper
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
        /// 使用缓存
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
    }
}
