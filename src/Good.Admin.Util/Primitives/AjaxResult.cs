namespace Good.Admin.Util
{
    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>

        public bool success { get; set; } = true;
        /// <summary>
        /// 状态码
        /// </summary>

        public int code { get; set; } = 200;
        /// <summary>
        /// 返回消息
        /// </summary>

        public string msg { get; set; }
    }
}
