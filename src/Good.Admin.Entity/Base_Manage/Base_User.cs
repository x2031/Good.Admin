using SqlSugar;
using System.ComponentModel;

namespace Good.Admin.Entity
{
    ///<summary>
    ///系统用户表
    ///</summary>
    [SugarTable("Base_User")]
    public class Base_User
    {
        public Base_User()
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
        /// Desc:用户名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UserName { get; set; }

        /// <summary>
        /// Desc:密码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Password { get; set; }

        /// <summary>
        /// Desc:姓名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string RealName { get; set; }

        /// <summary>
        /// Desc:性别(1为男，0为女)
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public Sex Sex { get; set; }

        /// <summary>
        /// Desc:出生日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Desc:所属部门Id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DepartmentId { get; set; }

    }
    public enum Sex
    {
        [Description("男人")]
        Man = 1,

        [Description("女人")]
        Woman = 0
    }
}
