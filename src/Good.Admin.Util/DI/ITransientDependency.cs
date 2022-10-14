namespace Good.Admin.Util
{
    /// <summary>
    /// 注入标记,生命周期为Transient 每次使用（获取这个服务的时候）时都会创建新的服务，适合轻量级的服务
    /// </summary>
    public interface ITransientDependency
    {

    }
}
