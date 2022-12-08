namespace Good.Admin.Util
{
    /// <summary>
    /// 分页返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T> : AjaxResult<List<T>>
    {
        public PageResult() { }

        public PageResult(int page, int total, int pageSize, List<T> data)
        {
            this.page = page;
            this.dotal = total;
            base.data = data;

            if (pageSize != 0)
            {
                this.pageSize = pageSize;
            }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int dotal { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int pageSize { set; get; } = 20;
        /// <summary>
        /// 当前页标
        /// </summary>
        public int page { get; set; } = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount => (int)Math.Ceiling((decimal)dotal / pageSize);
    }
}
