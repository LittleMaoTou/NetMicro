namespace NetMicro.Cache.Abstractions
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// 获取redis提供者
        /// </summary>
        /// <returns></returns>
        IRedisCache CreateRedis();

        /// <summary>
        /// 获取内存提供者
        /// </summary>
        /// <returns></returns>
        IInMemoryCache CreateCache();


    }
}
