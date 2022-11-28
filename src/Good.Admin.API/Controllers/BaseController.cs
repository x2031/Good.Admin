using Good.Admin.IBusiness;
using Good.Admin.Util;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Good.Admin.API.Controllers
{
    /// <summary>
    /// 基控制器
    /// </summary>
    //[FormatResponse]
    public class BaseController : Controller
    {
        /// <summary>
        /// 新增-初始化基础数据
        /// </summary>
        /// <param name="obj"></param>
        protected void InitEntity(object obj)
        {
            var op = HttpContext.RequestServices.GetService<IOperator>();
            if (obj.ContainsProperty("Id"))
                obj.SetPropertyValue("Id", IdHelper.NextId());
            if (obj.ContainsProperty("CreateTime"))
                obj.SetPropertyValue("CreateTime", DateTime.Now);
            if (obj.ContainsProperty("CreatorId"))
                obj.SetPropertyValue("CreatorId", op?.UserId);
            if (obj.ContainsProperty("CreatorRealName"))
                obj.SetPropertyValue("CreatorRealName", op?.Property?.RealName);
        }
        /// <summary>
        /// 更新-初始化
        /// </summary>
        /// <param name="obj"></param>
        protected void UpdateInitEntity(object obj)
        {
            var op = HttpContext.RequestServices.GetService<IOperator>();

            if (obj.ContainsProperty("UpdateTime"))
                obj.SetPropertyValue("UpdateTime", DateTime.Now);
            if (obj.ContainsProperty("UpdateId"))
                obj.SetPropertyValue("UpdateId", op?.UserId);
        }



        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="virtualPath">相对路径</param>
        /// <returns></returns>
        protected string GetAbsolutePath(string virtualPath)
        {
            string path = virtualPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (path[0] == '~')
                path = path.Remove(0, 2);
            string rootPath = HttpContext.RequestServices.GetService<IWebHostEnvironment>().WebRootPath;

            return Path.Combine(rootPath, path);
        }

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns></returns>
        protected ContentResult JsonContent(string jsonStr)
        {
            return base.Content(jsonStr, "application/json", Encoding.UTF8);
        }
        /// <summary>
        /// 返回html
        /// </summary>
        /// <param name="body">html内容</param>
        /// <returns></returns>
        protected ContentResult HtmlContent(string body)
        {
            return base.Content(body);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        protected AjaxResult Success()
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Code = 200,
                Msg = "请求成功！",
            };

            return res;
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        protected AjaxResult<T> Success<T>(T data)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = true,
                Code = 200,
                Msg = "操作成功",
                Data = data
            };

            return res;
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <param name="msg">返回消息</param>
        /// <returns></returns>
        protected AjaxResult<T> Success<T>(T data, string msg)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = true,
                Code = 200,
                Msg = msg,
                Data = data
            };


            return res;
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        protected AjaxResult Error()
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Code = 400,
                Msg = "请求失败！",
            };

            return res;
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        protected AjaxResult Error(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Code = 400,
                Msg = msg,
            };

            return res;
        }

    }
}
