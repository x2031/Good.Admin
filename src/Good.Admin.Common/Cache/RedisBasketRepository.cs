using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Good.Admin.Common
{
    public class RedisBasketRepository : IRedisBasketRepository
    {
        private readonly ILogger<RedisBasketRepository> _logger;
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _redisdatabase;

        public RedisBasketRepository(ILogger<RedisBasketRepository> logger, IConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
            _redisdatabase = redis.GetDatabase();
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

        public async Task ClearAsync()
        {
            foreach (var endPoint in _redis.GetEndPoints())
            {
                var server = GetServer();
                foreach (var key in server.Keys())
                {
                    await _redisdatabase.KeyDeleteAsync(key);
                }
            }
        }
        /// <summary>
        /// 是否存在key得缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(string key)
        {
            return await _redisdatabase.KeyExistsAsync(key);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _redisdatabase.StringGetAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _redisdatabase.KeyDeleteAsync(key);
        }
        public async Task RemoveAsync(List<string> keys)
        {
            foreach (var key in keys)
            {
                await _redisdatabase.KeyDeleteAsync(key);
            }
        }

        public async Task SetAsync(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                if (value is string cacheValue)
                {
                    // 字符串无需序列化
                    await _redisdatabase.StringSetAsync(key, cacheValue, cacheTime);
                }
                else
                {
                    //序列化，将object值生成RedisValue
                    await _redisdatabase.StringSetAsync(key, JsonConvert.SerializeObject(value), cacheTime);
                }
            }
        }

        public async Task<TEntity> GetAsync<TEntity>(string key)
        {
            var value = await _redisdatabase.StringGetAsync(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return JsonConvert.DeserializeObject<TEntity>(value);
            }
            else
            {
                return default;
            }
        }




        /// <summary>
        /// 根据key获取RedisValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<RedisValue[]> ListRangeAsync(string redisKey)
        {
            return await _redisdatabase.ListRangeAsync(redisKey);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync(string redisKey, string redisValue, int db = -1)
        {
            return await _redisdatabase.ListLeftPushAsync(redisKey, redisValue);
        }
        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string redisKey, string redisValue, int db = -1)
        {
            return await _redisdatabase.ListRightPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入数组集合。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue, int db = -1)
        {
            var redislist = new List<RedisValue>();
            foreach (var item in redisValue)
            {
                redislist.Add(item);
            }
            return await _redisdatabase.ListRightPushAsync(redisKey, redislist.ToArray());
        }


        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素  反序列化
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string redisKey, int db = -1) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await _redisdatabase.ListLeftPopAsync(redisKey));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素   反序列化
        /// 只能是对象集合
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string redisKey, int db = -1) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await _redisdatabase.ListRightPopAsync(redisKey));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素   
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<string> ListLeftPopAsync(string redisKey, int db = -1)
        {
            return await _redisdatabase.ListLeftPopAsync(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素   
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<string> ListRightPopAsync(string redisKey, int db = -1)
        {
            return await _redisdatabase.ListRightPopAsync(redisKey);
        }

        /// <summary>
        /// 列表长度
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string redisKey, int db = -1)
        {
            return await _redisdatabase.ListLengthAsync(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1)
        {
            var result = await _redisdatabase.ListRangeAsync(redisKey);
            return result.Select(o => o.ToString());
        }

        /// <summary>
        /// 根据索引获取指定位置数据
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop, int db = -1)
        {
            var result = await _redisdatabase.ListRangeAsync(redisKey, start, stop);
            return result.Select(o => o.ToString());
        }

        /// <summary>
        /// 删除List中的元素 并返回删除的个数
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">元素</param>
        /// <param name="type">大于零 : 从表头开始向表尾搜索，小于零 : 从表尾开始向表头搜索，等于零：移除表中所有与 VALUE 相等的值</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0, int db = -1)
        {
            return await _redisdatabase.ListRemoveAsync(redisKey, redisValue, type);
        }

        /// <summary>
        /// 清空List
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        public async Task ListClearAsync(string redisKey, int db = -1)
        {
            await _redisdatabase.ListTrimAsync(redisKey, 1, 0);
        }
    }

}
