using SqlSugar;

namespace Good.Admin.Entity
{
    ///<summary>
    ///生成测试表
    ///</summary>
    [SugarTable("Base_BuildTest")]
    public class Base_BuildTest
    {
        public Base_BuildTest()
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
        /// Desc:列1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Column1 { get; set; }

        /// <summary>
        /// Desc:列2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Column2 { get; set; }

        /// <summary>
        /// Desc:列3
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Column3 { get; set; }

        /// <summary>
        /// Desc:列4
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Column4 { get; set; }

        /// <summary>
        /// Desc:列5
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Column5 { get; set; }

    }
}
