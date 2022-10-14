using System.Collections.Generic;

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
            this.Page = page;
            this.Total = total;
            base.Data = data;

            if (pageSize != 0)
            {
                this.PageSize = pageSize;
            }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 20;
        /// <summary>
        /// 当前页标
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount => (int)Math.Ceiling((decimal)Total / PageSize);
    }
}
