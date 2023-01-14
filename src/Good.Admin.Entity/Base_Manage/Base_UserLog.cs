using SqlSugar;

namespace Good.Admin.Entity
{
    ///<summary>
    ///系统日志表
    ///</summary>
    [SugarTable("Base_UserLog")]
    public class Base_UserLog
    {
        public Base_UserLog()
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
        /// Desc:创建人姓名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CreatorRealName { get; set; }

        /// <summary>
        /// Desc:日志类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string LogType { get; set; }

        /// <summary>
        /// Desc:日志内容
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string LogContent { get; set; }

    }
}
