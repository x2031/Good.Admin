namespace Good.Admin.Common.Primitives
{
    /// <summary>
    /// 分页查询基类
    /// </summary>
    public class PageInput
    {
        private string _sortType { get; set; } = "asc";
        private int _pagerows { get; set; } = 20;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get => _pagerows; set => _pagerows = value <= 0 ? 20 : value; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string SortField { get; set; } = "Id";

        /// <summary>
        /// 排序类型
        /// </summary>
        public string SortType { get => _sortType; set => _sortType = (value ?? string.Empty).ToLower().Contains("desc") ? "desc" : "asc"; }
    }
}
