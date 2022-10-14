using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Good.Admin.IBusiness;
using Good.Admin.Util;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SqlSugar;

namespace Good.Admin.Business
{
    /// <summary>
    /// 自动缓存数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class RedisCacheAopAttribute : BaseAOPAttribute
    {
        /// <summary>
        /// 缓存绝对过期时间（分钟）
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 30;



        public override async Task Befor(IAOPContext context)
        {

            string key = CustomCacheKey(context);


            var op = context.ServiceProvider.GetService<IOperator>();
            var result = await op.GetCache(key);
            if (result != null)
            {
                context.ReturnValue = result;
                return;
            }
            
            await Task.CompletedTask;
        }

        public override async Task After(IAOPContext context)
        {
            string key = CustomCacheKey(context);
            var op = context.ServiceProvider.GetService<IOperator>();
            var type = context.GenericArguments;
            var value = context.ReturnValue;
            if (!key.IsNullOrEmpty())
            {
                await op.SetCache(AbsoluteExpiration, key, value);
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 自定义缓存的key
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        protected string CustomCacheKey(IAOPContext context)
        {
            var typeName = context.TargetType.Name;
            var methodName = context.Method.Name;
            var methodArguments = context.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多三个

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key = $"{key}{param}:";
            }

            return key.TrimEnd(':');
        }
        /// <summary>
        /// object 转 string
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected static string GetArgumentValue(object arg)
        {
            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (!arg.IsNullOrEmpty())
                return arg.ObjToString();

            if (arg != null)
            {
                if (arg is Expression)
                {
                    var obj = arg as Expression;
                    var result = Resolve(obj);
                    return result?.ToMD5String16();
                }
                else if (arg.GetType().IsClass)
                {
                    return JsonConvert.SerializeObject(arg)?.ToMD5String16();
                }

                return $"value:{arg.ObjToString()}";
            }
            return string.Empty;
        }
        private static string Resolve(Expression expression)
        {
            ExpressionContext expContext = new ExpressionContext();
            expContext.Resolve(expression, ResolveExpressType.WhereSingle);
            var value = expContext.Result.GetString();
            var pars = expContext.Parameters;

            pars.ForEach(s =>
            {
                value = value.Replace(s.ParameterName, s.Value.ObjToString());
            });

            return value;
        }

    }
}
