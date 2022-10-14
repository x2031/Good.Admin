using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Good.Admin.Entity
{
    ///<summary>
    ///用户角色表
    ///</summary>
    [SugarTable("Base_UserRole")]
    public  class Base_UserRole
    {
           public Base_UserRole(){


           }
           /// <summary>
           /// Desc:主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string Id {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

           /// <summary>
           /// Desc:创建人Id
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string CreatorId {get;set;}

           /// <summary>
           /// Desc:否已删除
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public byte Deleted {get;set;}

           /// <summary>
           /// Desc:用户Id
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string UserId {get;set;}

           /// <summary>
           /// Desc:角色Id
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string RoleId {get;set;}

    }
}
