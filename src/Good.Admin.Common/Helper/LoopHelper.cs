namespace Good.Admin.Common.Helper
{
    /// <summary>
    /// 循环帮助类
    /// </summary>
    public class LoopHelper
    {
        /// <summary>
        /// 循环指定次数
        /// </summary>
        /// <param name="count">循环次数</param>
        /// <param name="method">执行的方法</param>
        public static void Loop(int count, Action method)
        {
            for (var i = 0; i < count; i++)
            {
                method();
            }
        }

        /// <summary>
        /// 循环指定次数
        /// </summary>
        /// <param name="count">循环次数</param>
        /// <param name="method">执行的方法</param>
        public static void Loop(int count, Action<int> method)
        {
            for (var i = 0; i < count; i++)
            {
                method(i);
            }
        }
    }
}
