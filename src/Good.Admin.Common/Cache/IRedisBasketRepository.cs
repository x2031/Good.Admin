using StackExchange.Redis;

namespace Good.Admin.Common.Cache
{
    /// <summary>
    /// Redis缓存接口
    /// </summary>
    public interface IRedisBasketRepository
    {

        //获取 Reids 缓存值
        Task<string> GetValueAsync(string key);

        //获取值，并序列化
        Task<TEntity> GetAsync<TEntity>(string key);

        //保存
        Task SetAsync(string key, object value, TimeSpan cacheTime);

        //判断是否存在
        Task<bool> ExistAsync(string key);

        //移除某一个缓存值
        Task RemoveAsync(string key);
        /// <summary>
        /// 移除list中的所有缓存值
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task RemoveAsync(List<string> keys);

        //全部清除
        Task ClearAsync();

        Task<RedisValue[]> ListRangeAsync(string redisKey);
        Task<long> ListLeftPushAsync(string redisKey, string redisValue, int db = -1);
        Task<long> ListRightPushAsync(string redisKey, string redisValue, int db = -1);
        Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue, int db = -1);
        Task<T> ListLeftPopAsync<T>(string redisKey, int db = -1) where T : class;
        Task<T> ListRightPopAsync<T>(string redisKey, int db = -1) where T : class;
        Task<string> ListLeftPopAsync(string redisKey, int db = -1);
        Task<string> ListRightPopAsync(string redisKey, int db = -1);
        Task<long> ListLengthAsync(string redisKey, int db = -1);
        Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1);
        Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop, int db = -1);
        Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0, int db = -1);
        Task ListClearAsync(string redisKey, int db = -1);

    }

}
