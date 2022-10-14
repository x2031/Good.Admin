﻿namespace Good.Admin.API
{
    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// 使用内存缓存(不支持分布式)
        /// </summary>
        Memory,
        /// <summary>
        /// 使用Redis缓存(支持分布式)
        /// </summary>
        Redis
    }
}
