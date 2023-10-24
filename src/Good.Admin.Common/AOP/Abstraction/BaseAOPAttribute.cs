namespace Good.Admin.Common
{
    /// <summary>
    /// AOP基类
    /// 注:不支持控制器,需要定义接口并实现接口,自定义AOP特性放到接口实现类上
    /// </summary>
    public abstract class BaseAOPAttribute : Attribute
    {
        public async virtual Task Befor(IAOPContext context)
        {
            await Task.CompletedTask;
        }

        public async virtual Task After(IAOPContext context)
        {
            await Task.CompletedTask;
        }
    }
}
