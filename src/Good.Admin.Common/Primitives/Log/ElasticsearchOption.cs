namespace Good.Admin.Common
{
    public class ElasticsearchOption : EnableOption
    {
        /// <summary>
        /// ES节点
        /// </summary>
        public List<string> Nodes { get; set; } = new List<string>();

        /// <summary>
        /// 索引格式:custom-index-{0:yyyy.MM}
        /// </summary>
        public string IndexFormat { get; set; }
        /// <summary>
        /// 默认索引库
        /// </summary>
        public string DefaultIndex { get; set; }
    }
}
