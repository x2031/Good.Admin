﻿using System.Text;

namespace Good.Admin.Common
{
    /// <summary>
    /// 异常处理帮助类
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// 获取异常位置
        /// </summary>
        /// <param name="e">异常</param>
        /// <returns></returns>
        private static string GetExceptionAddr(Exception e)
        {
            var excAddrBuilder = new StringBuilder();
            e?.StackTrace?.Split("\r\n".ToArray())?.ToList()?.ForEach(item =>
            {
                if (item.Contains("行号") || item.Contains("line"))
                    excAddrBuilder.Append($"    {item}\r\n");
            });

            var addr = excAddrBuilder.ToString();

            return addr.IsNullOrEmpty() ? "    无" : addr;
        }

        /// <summary>
        /// 获取异常消息
        /// </summary>
        /// <param name="ex">捕捉的异常</param>
        /// <param name="level">内部异常层级</param>
        /// <returns></returns>
        private static string GetExceptionAllMsg(Exception ex, int level)
        {
            var builder = new StringBuilder();
            builder.Append($@"
{level}层错误:
  消息:
    {ex?.Message}
  位置:
{GetExceptionAddr(ex)}
");
            if (ex.InnerException != null)
            {
                builder.Append(GetExceptionAllMsg(ex.InnerException, level + 1));
            }

            return builder.ToString();
        }

        /// <summary>
        /// 获取异常消息
        /// </summary>
        /// <param name="ex">捕捉的异常</param>
        /// <returns></returns>
        public static string GetExceptionAllMsg(Exception ex)
        {
            var msg = GetExceptionAllMsg(ex, 1);
            //            try
            //            {
            //                msg += $@"
            ////url:{HttpContextCore.Current.Request.GetDisplayUrl()}
            ////body:{HttpContextCore.Current.Request.Body.ReadToString()}
            //";
            //            }
            //            catch
            //            {

            //            }
            return msg;
        }
    }
}