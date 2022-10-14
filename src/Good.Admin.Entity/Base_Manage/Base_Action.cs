using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Good.Admin.Entity
{
    ///<summary>
    ///系统权限表
    ///</summary>
    [SugarTable("Base_Action")]
    public  class Base_Action
    {
           public Base_Action(){


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
           /// Desc:父级Id
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ParentId {get;set;}

           /// <summary>
           /// Desc:类型,菜单=0,页面=1,权限=2
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Type {get;set;}

           /// <summary>
           /// Desc:权限名/菜单名
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:菜单地址
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Url {get;set;}

           /// <summary>
           /// Desc:权限值
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Value {get;set;}

           /// <summary>
           /// Desc:是否需要权限(仅页面有效)
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public byte NeedAction {get;set;}

           /// <summary>
           /// Desc:图标
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Icon {get;set;}

           /// <summary>
           /// Desc:排序
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int Sort {get;set;}

    }
}
