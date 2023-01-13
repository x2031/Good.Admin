using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Util;
using Newtonsoft.Json;
using SqlSugar;

namespace Good.Admin.API.DI
{
    /// <summary>
    /// 操作者
    /// </summary>
    public class Operator : IOperator, ISingletonDependency
    {
        readonly IServiceProvider _serviceProvider;
        //readonly ICaching _cache;
        readonly IRedisBasketRepository _rediscache;

        public Operator(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider, IRedisBasketRepository rediscache)
        {
            _serviceProvider = serviceProvider;
            //_cache = cache;
            _rediscache = rediscache;
            UserId = httpContextAccessor?.HttpContext?.User.Claims
                .Where(x => x.Type == "userId").FirstOrDefault()?.Value;
        }

        private Base_UserDTO _property;
        private object _lockObj = new object();

        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// 用户属性
        /// </summary>
        public Base_UserDTO UserProperty {
            get {
                if (UserId.IsNullOrEmpty())
                    return default;

                if (_property == null)
                {
                    lock (_lockObj)
                    {
                        if (_property == null)
                        {
                            _property = AsyncHelper.RunSync(() => _rediscache.GetAsync<Base_UserDTO>(UserId));
                        }
                    }
                }
                return _property;
            }
        }
        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            var role = UserProperty.RoleType;
            if (UserId == GlobalAssemblies.ADMINID || role.HasFlag(RoleTypes.超级管理员))
                return true;
            else
                return false;
        }

        public void WriteUserLog(UserLogType userLogType, string msg)
        {
            var log = new Base_UserLog
            {
                Id = IdHelper.NextId(),
                CreateTime = DateTime.Now,
                CreatorId = UserId,
                CreatorRealName = UserProperty.RealName,
                LogContent = msg,
                LogType = userLogType.ToString()
            };

            //Task.Factory.StartNew(async () =>
            //{
            //    using (var scop = _serviceProvider.CreateScope())
            //    {
            //        var db = scop.ServiceProvider.GetService<ISqlSugarClient>();
            //        await db.InsertAsync(log);
            //    }
            //}, TaskCreationOptions.LongRunning);
        }


        public async Task<object?> GetCache(string key)
        {
            var result = await _rediscache.GetValueAsync(key);
            if (result.IsNullOrEmpty())
            {
                return null;
            }
            var obj = JsonConvert.DeserializeObject(result);
            return obj;
        }

        public async Task SetCache(int AbsoluteExpiration, string key, object? value)
        {
            var seconds = TimeSpan.FromSeconds(AbsoluteExpiration * 60);
            await _rediscache.SetAsync(key, JsonConvert.SerializeObject(value), seconds);
        }
    }


}
