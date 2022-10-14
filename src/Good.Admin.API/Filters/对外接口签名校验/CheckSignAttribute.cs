using Good.Admin.IBusiness;
using Good.Admin.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace Good.Admin.API
{
    public class CheckSignAttribute : BaseActionFilterAsync
    {
        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public async override Task OnActionExecuting(ActionExecutingContext filterContext)
        {
            //判断是否需要签名
            if (filterContext.ContainsFilter<IgnoreSignAttribute>())
                return;
            var request = filterContext.HttpContext.Request;
            IServiceProvider serviceProvider = filterContext.HttpContext.RequestServices;
            IBase_AppSecretBusiness appSecretBus = serviceProvider.GetService<IBase_AppSecretBusiness>();
            ILogger logger = serviceProvider.GetService<ILogger<CheckSignAttribute>>();
            var cache = serviceProvider.GetService<IDistributedCache>();

            string appId = request.Headers["appId"].ToString();
            if (appId.IsNullOrEmpty())
            {
                ReturnError("缺少header:appId");
                return;
            }
            string time = request.Headers["time"].ToString();
            if (time.IsNullOrEmpty())
            {
                ReturnError("缺少header:time");
                return;
            }
            if (time.ToDateTime() < DateTime.Now.AddMinutes(-5) || time.ToDateTime() > DateTime.Now.AddMinutes(5))
            {
                ReturnError("time过期");
                return;
            }

            string guid = request.Headers["guid"].ToString();
            if (guid.IsNullOrEmpty())
            {
                ReturnError("缺少header:guid");
                return;
            }

            string guidKey = $"ApiGuid_{guid}";
            if (cache.GetString(guidKey).IsNullOrEmpty())
                cache.SetString(guidKey, "1", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            else
            {
                ReturnError("禁止重复调用!");
                return;
            }

            request.EnableBuffering();
            string body = await request.Body.ReadToStringAsync();

            string sign = request.Headers["sign"].ToString();
            if (sign.IsNullOrEmpty())
            {
                ReturnError("缺少header:sign");
                return;
            }

            //string appSecret = await appSecretBus.GetAppSecretAsync(appId);
            // TODO 需要编写获取口令
            string appSecret = "";
            if (appSecret.IsNullOrEmpty())
            {
                ReturnError("header:appId无效");
                return;
            }
            string newSign = BuildApiSign(appId, appSecret, guid, time.ToDateTime(), body);
            if (sign != newSign)
            {
                string log = $@"sign签名错误!headers:{request.Headers.ToJson()}body:{body}正确sign:{newSign}";
                logger.LogWarning(log);
                ReturnError("header:sign签名错误");
                return;
            }

            void ReturnError(string msg)
            {
                filterContext.Result = Error(msg);
            }
        }

        /// <summary>
        /// 生成接口签名sign
        /// 注：md5(appId+time+guid+body+appSecret)
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="appSecret">应用密钥</param>
        /// <param name="guid">唯一GUID</param>
        /// <param name="time">时间</param>
        /// <param name="body">请求体</param>
        /// <returns></returns>
        private string BuildApiSign(string appId, string appSecret, string guid, DateTime time, string body)
        {
            return $"{appId}{time.ToString("yyyy-MM-dd HH:mm:ss")}{guid}{body}{appSecret}".ToMD5String();
        }
    }
}
