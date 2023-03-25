using Good.Admin.Entity;
using Good.Admin.Util;

namespace Good.Admin.IBusiness
{
    /// <summary>
    /// 操作者
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        string UserId { get; }
        /// <summary>
        /// 用户属性
        /// </summary>
        UserDTO UserProperty { get; }

        #region 操作方法
        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        bool IsAdmin();
        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="userLogType">用户日志类型</param>
        /// <param name="msg">内容</param>
        void WriteUserLog(UserLogType userLogType, string msg);
        #endregion
    }

}
