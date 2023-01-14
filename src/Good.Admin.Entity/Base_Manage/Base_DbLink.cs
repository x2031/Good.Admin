using SqlSugar;

namespace Good.Admin.Entity
{
    ///<summary>
    ///数据库连接表
    ///</summary>
    [SugarTable("Base_DbLink")]
    public class Base_DbLink
    {
        public Base_DbLink()
        {


        }
        /// <summary>
        /// Desc:自然主键
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Desc:创建人Id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CreatorId { get; set; }

        /// <summary>
        /// Desc:否已删除
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public byte Deleted { get; set; }

        /// <summary>
        /// Desc:连接名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string LinkName { get; set; }

        /// <summary>
        /// Desc:连接字符串
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ConnectionStr { get; set; }

        /// <summary>
        /// Desc:数据库类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DbType { get; set; }

    }
}
