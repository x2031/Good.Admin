namespace Good.Admin.Util
{
    /// <summary>
    /// 注入标记,生命周期为Singleton 全局只创建一次的服务,第一次被请求的时候被创建,然后就一直使用同一个
    /// </summary>
    public interface ISingletonDependency
    {

    }
}
