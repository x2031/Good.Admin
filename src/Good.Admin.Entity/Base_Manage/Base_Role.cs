using SqlSugar;

namespace Good.Admin.Entity
{
    ///<summary>
    ///系统角色表
    ///</summary>
    [SugarTable("Base_Role")]
    public class Base_Role
    {
        public Base_Role()
        {


        }
        /// <summary>
        /// Desc:主键
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
        /// Desc:角色名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string RoleName { get; set; }

    }
}
