//using EFCore.Sharding;

namespace Good.Admin.Common.Primitives
{
    public class DatabaseOptions
    {
        public string ConnId { get; set; }
        public string ConnectionString { get; set; }
        //public DatabaseType DatabaseType { get; set; }
        public int HitRate { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DbType { get; set; }
    }
}
