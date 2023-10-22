using Good.Admin.Util;
using System.Diagnostics;

namespace Good.Admin.API
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLogMiddleware(RequestDelegate next, ILogger<RequestLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch watch = Stopwatch.StartNew();
            string resContent = string.Empty;
            //返回Body需特殊处理
            Stream originalResponseBody = context.Response.Body;

            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            try
            {
                await _next(context);
                memStream.Position = 0;
                resContent = new StreamReader(memStream).ReadToEnd();
                memStream.Position = 0;
                await memStream.CopyToAsync(originalResponseBody);
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Response.Body = originalResponseBody;

                watch.Stop();

                if (resContent?.Length > 1000)
                {
                    resContent = new string(resContent.Copy(0, 1000).ToArray());
                    resContent += "......内容太长已忽略";
                }

                //if (!UserId.IsNullOrEmpty())
                //{
                //    LogContext.PushProperty("Operator",UserId);
                //}

                string log =
            @"IP:{RemoteIpAddress}
Method:{Method}
Path:{Path}
ContentType:{ContentType}
Query:{Query}
Operator:{Operator}
Time:{Time}ms
Body:
{Body}
StatusCode:{StatusCode}
Response:{Response}
";

                if (context.Request.Path != "/swagger/v1/swagger.json" &&
                  context.Request.Path != "/js/MiniProfiler_head.js" &&
                  context.Request.Path != "/api/index.html" &&
                  context.Request.Path != "/api")
                {
                    //写日志
                    _logger.LogInformation(
                    log,
                    context.Connection.RemoteIpAddress,
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.ContentType,
                    context.Request.Query,
                    context?.User.Claims.Where(x => x.Type == "userId").FirstOrDefault()?.Value,
                    (int)watch.ElapsedMilliseconds,
                    GetRequestBody(context),
                    context.Response.StatusCode,
                    resContent
                    );
                }

            }
        }
        /// <summary>
        /// 获取body参数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetRequestBody(HttpContext context)
        {
            string requestBody;
            if (context == null || context.Request == null)
            {
                return null;
            }

            try
            {
                requestBody = context.RequestServices.GetService<RequestBody>()?.Body;
            }
            catch (Exception ex)
            {
                return "获取出错";
            }

            return requestBody;
        }
    }
}
