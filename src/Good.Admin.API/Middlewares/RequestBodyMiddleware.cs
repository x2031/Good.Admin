using Good.Admin.Util;
using System.Text;

namespace Good.Admin.API
{
    public class RequestBodyMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestBodyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if ((context.Request.ContentType ?? string.Empty).Contains("application/json"))
            {
                context.Request.EnableBuffering();
                string body = await context.Request.Body?.ReadToStringAsync(Encoding.UTF8);
                context.RequestServices.GetService<RequestBody>().Body = body;
            }
            await _next(context);
        }
    }
}
