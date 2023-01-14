using SqlSugar;

namespace Good.Admin.Entity
{
    ///<summary>
    ///应用密钥表
    ///</summary>
    [SugarTable("Base_AppSecret")]
    public class Base_AppSecret
    {
        public Base_AppSecret()
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
        /// Desc:应用Id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AppId { get; set; }

        /// <summary>
        /// Desc:应用密钥
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AppSecret { get; set; }

        /// <summary>
        /// Desc:应用名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AppName { get; set; }

    }
}
