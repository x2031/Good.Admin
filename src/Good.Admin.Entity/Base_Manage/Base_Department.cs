using SqlSugar;

namespace Good.Admin.Entity
{
    ///<summary>
    ///部门表
    ///</summary>
    [SugarTable("Base_Department")]
    public class Base_Department
    {
        public Base_Department()
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
        public int Deleted { get; set; }

        /// <summary>
        /// Desc:部门名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:上级部门Id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ParentId { get; set; }

    }
}
