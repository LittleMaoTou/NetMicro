using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetMicro.Cache.Abstractions
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface IRedisCache
    {
        #region Key处理

        /// <summary>
        /// 删除指定Key
        /// </summary>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        bool Remove(string key, int index_db = -1);
        /// <summary>
        /// 删除指定key
        /// </summary>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key, int index_db = -1);
        /// <summary>
        /// 删除指定的key（一个或多个）
        /// </summary>
        /// <param name="keys">密钥集合</param>
        /// <returns></returns>
        long Remove(string[] keys, int index_db = -1);
        /// <summary>
        /// 删除指定的key（一个或多个）
        /// </summary>
        /// <param name="keys">密钥集合</param>
        /// <returns></returns>
        Task<long> RemoveAsync(string[] keys, int index_db = -1);
        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        bool Contains(string key, int index_db = -1);
        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<bool> ContainsAsync(string key, int index_db = -1);
        /// <summary>
        ///为给定 key 设置过期时间。超时结束后，key将自动被删除
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expiry">时间</param>
        /// <returns></returns>
        bool KeyExpire(string key, DateTime? expiry, int index_db = -1);
        /// <summary>
        ///为给定 key 设置过期时间。超时结束后，key将自动被删除
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expiry">时间</param>
        /// <returns></returns>
        Task<bool> KeyExpireAsync(string key, DateTime? expiry, int index_db = -1);
        /// <summary>
        /// 获取 key的有效时间（毫秒）
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        TimeSpan? KeyTimeToLive(string key, int index_db = -1);
        /// <summary>
        /// 获取 key的有效时间（毫秒）
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<TimeSpan?> KeyTimeToLiveAsync(string key, int index_db = -1);
        /// <summary>
        /// 返回 key 所储存的值的类型
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        RedisItemType KeyType(string key, int index_db = -1);
        /// <summary>
        /// 返回 key 所储存的值的类型
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<RedisItemType> KeyTypeAsync(string key, int index_db = -1);

        #endregion

        #region 字符串

        #region 赋值

        /// <summary>
        /// 设置指定key的值，过期时间，若key存在则是修改，否则新增
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="seconds">时间</param>
        /// <returns></returns>
        bool Set(string key, string value, int seconds = 0, int index_db = -1);
        /// <summary>
        /// 设置指定key的值，过期时间，若key存在则是修改，否则新增
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">key</param>
        /// <param name="t">实体</param>
        /// <param name="seconds">过期时间</param>
        /// <returns></returns>
        bool Set<T>(string key, T t, int seconds = 0, int index_db = -1) where T : class, new();
        /// <summary>
        /// 设置指定key的值，过期时间，若key存在则是修改，否则新增
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="seconds">时间</param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, string value, int seconds = 0, int index_db = -1);
        /// <summary>
        /// 设置指定key的值，过期时间，若key存在则是修改，否则新增
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="key">key</param>
        /// <param name="t">类</param>
        /// <param name="seconds">过期时间</param>
        /// <returns></returns>
        Task<bool> SetAsync<T>(string key, T t, int seconds = 0, int index_db = -1) where T : class, new();
        /// <summary>
        /// 保存多个key- value，若key存在则是修改，否则新增
        /// </summary>
        /// <param name="kvs">键值对</param>
        /// <returns></returns>
        bool Set(IDictionary<string, string> kvs, int index_db = -1);
        /// <summary>
        /// 保存多个key- value，若key存在则是修改，否则新增
        /// </summary>
        /// <param name="kvs">键值对</param>
        /// <returns></returns>
        Task<bool> SetAsync(IDictionary<string, string> kvs, int index_db = -1);

        #endregion

        #region 添加值
        /// <summary>
        /// 添加指定key的值，过期时间，若key不存在则新增，否则不会执行新增和修改
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="seconds">时间</param>
        /// <returns></returns>
        bool Add(string key, string value, int seconds = 0, int index_db = -1);
        /// <summary>
        /// 添加指定key的值，过期时间，若key不存在则新增，否则不会执行新增和修改
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="key">key</param>
        /// <param name="t">类</param>
        /// <param name="seconds">过期时间</param>
        /// <returns></returns>
        bool Add<T>(string key, T t, int seconds = 0, int index_db = -1) where T : class, new();
        /// <summary>
        /// 添加指定key的值，过期时间，若key不存在则新增，否则不会执行新增和修改
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="seconds">时间</param>
        /// <returns></returns>
        Task<bool> AddAsync(string key, string value, int seconds = 0, int index_db = -1);
        /// <summary>
        /// 保存多个key- value，若key不存在则新增，否则不会执行新增和修改
        /// </summary>
        /// <param name="kvs">键值对</param>
        /// <returns></returns>
        bool Add(IDictionary<string, string> kvs, int index_db = -1);
        /// <summary>
        /// 异步保存多个key- value，若key不存在则新增，否则不会执行新增和修改
        /// </summary>
        /// <param name="kvs">键值对</param>
        /// <returns></returns>
        Task<bool> AddAsync(IDictionary<string, string> kvs, int index_db = -1);

        #endregion

        #region 获取值
        /// <summary>
        /// 获取一个值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        string Get(string key, int index_db = -1);
        /// <summary>
        /// 获取一个实体类
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="key">key</param>
        /// <returns></returns>
        T Get<T>(string key, int index_db = -1) where T : class;
        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <param name="keys">key集合</param>
        /// <returns></returns>
        string[] Get(string[] keys, int index_db = -1);
        /// <summary>
        /// 获取一个key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<string> GetAsync(string key, int index_db = -1);
        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <param name="keys">key集合</param>
        /// <returns></returns>
        Task<string[]> GetAsync(string[] keys, int index_db = -1);

        #endregion

        #region 自增
        /// <summary>
        /// 将 key 所储存的值加上给定的增量值（increment）。
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        long Increment(string key, long value = 1, int index_db = -1);
        /// <summary>
        ///如果key不存在，那么key的值会先被初始化为0，然后再执行增量命令。
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        Task<long> IncrementAsync(string key, long value = 1, int index_db = -1);

        #endregion

        #region 自减
        /// <summary>
        /// 如果key不存在，那么key的值会先被初始化为0，然后再执行减量命令。
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">减量默认为1</param>
        /// <returns></returns>
        long Decrement(string key, long value = 1, int index_db = -1);
        /// <summary>
        /// 如果key不存在，那么key的值会先被初始化为0，然后再执行减量命令。
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">减量默认为1</param>
        /// <returns></returns>
        Task<long> DecrementAsync(string key, long value = 1, int index_db = -1);

        #endregion

        #region 获取旧值赋上新值
        /// <summary>
        ///  将给定 key 的值设为 value ，并返回 key 的旧值(old value)
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        string GetSet(string key, string value, int index_db = -1);
        /// <summary>
        ///  将给定 key 的值设为 value ，并返回 key 的旧值(old value)
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<string> GetSetAsync(string key, string value, int index_db = -1);

        #endregion

        #endregion

        #region 哈希

        #region 哈希赋值
        /// <summary>
        /// 如果hashId集合中存在key/value则不添加返回false，如果不存在在添加key/value,返回true
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool HashSet(string hashId, string key, string value, int index_db = -1);
        /// <summary>
        /// 如果hashId集合中存在key/value则不添加返回false，如果不存在在添加key/value,返回true
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> HashSetAsync(string hashId, string key, string value, int index_db = -1);
        /// <summary>
        /// 存储对象T t到hash集合中
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="key">key</param>
        /// <param name="entity">对象</param>
        void HashSet<T>(string key, T entity, int index_db = -1) where T : class, new();
        /// <summary>
        /// 存储对象T t到hash集合中
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="key">key</param>
        /// <param name="entity">对象</param>
        void HashSet<T>(string key, Expression<Func<T>> expression, int index_db = -1) where T : class, new();

        #endregion

        #region 哈希自增
        /// <summary>
        ///  给hashid数据集key的value加自增量，返回相加后的数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        long HashIncrement(string hashId, string key, long value = 1, int index_db = -1);
        /// <summary>
        ///  给hashid数据集key的value加自增量，返回相加后的数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<long> HashIncrementAsync(string hashId, string key, long value = 1, int index_db = -1);

        #endregion

        #region 哈希自减
        /// <summary>
        ///  给hashid数据集key的value减去自减量，返回相加后的数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        long HashDecrement(string hashId, string key, long value = 1, int index_db = -1);
        /// <summary>
        ///  给hashid数据集key的value减去自减量，返回相加后的数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<long> HashDecrementAsync(string hashId, string key, long value = 1, int index_db = -1);

        #endregion

        #region 查询数据是否存在
        /// <summary>
        /// 判断hashid数据集中是否存在key的数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        bool HashExists(string hashId, string key, int index_db = -1);
        /// <summary>
        /// 判断hashid数据集中是否存在key的数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<bool> HashExistsAsync(string hashId, string key, int index_db = -1);

        #endregion

        #region 删除
        /// <summary>
        /// 删除hashid数据集中的key数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        bool HashDelete(string hashId, string key, int index_db = -1);
        /// <summary>
        /// 删除hashid数据集中的key数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<bool> HashDeleteAsync(string hashId, string key, int index_db = -1);
        /// <summary>
        ///  删除hashid数据集中的key的集合数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="keys">keys</param>
        /// <returns></returns>
        long HashDelete(string hashId, string[] keys, int index_db = -1);
        /// <summary>
        ///  删除hashid数据集中的key的集合数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="keys">keys</param>
        /// <returns></returns>
        Task<long> HashDeleteAsync(string hashId, string[] keys, int index_db = -1);

        #endregion

        #region 获取单条
        /// <summary>
        /// 获取hashid数据集中，key的value数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        string HashGet(string hashId, string key, int index_db = -1);
        /// <summary>
        /// 获取hashid数据集中，key的value数据
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<RedisValue> HashGetAsync(string hashId, string key, int index_db = -1);

        #endregion

        #region 获取全部
        /// <summary>
        /// 获取所有hashid数据集的key/value数据集合
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <returns></returns>
        KeyValuePair<string, string>[] HashGetAll(string hashId, int index_db = -1);
        /// <summary>
        /// 获取所有hashid数据集的key/value数据集合
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <returns></returns>
        Task<KeyValuePair<string, string>[]> HashGetAllAsync(string hashId, int index_db = -1);

        #endregion

        #region 获取key集合
        /// <summary>
        /// 获取hashid数据集中所有key的集合
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        string[] HashKeys(string hashId, int index_db = -1);
        /// <summary>
        /// 获取hashid数据集中所有key的集合
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        Task<string[]> HashKeysAsync(string hashId, int index_db = -1);

        #endregion

        #region 获取值集合
        /// <summary>
        /// 获取hashid数据集中的所有value集合
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        string[] HashValues(string hashId, int index_db = -1);
        /// <summary>
        /// 获取hashid数据集中的所有value集合
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        Task<string[]> HashValuesAsync(string hashId, int index_db = -1);

        #endregion

        #region 获取数据总数
        /// <summary>
        /// 获取hashid数据集中的数据总数
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <returns></returns>
        long HashLength(string hashId, int index_db = -1);
        /// <summary>
        /// 获取hashid数据集中的数据总数
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <returns></returns>
        Task<long> HashLengthAsync(string hashId, int index_db = -1);

        #endregion



        #endregion

        #region 列表

        #region 左侧赋值
        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        long ListLeftPush(string key, string value, int index_db = -1);
        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        long ListLeftPush(string key, string[] values, int index_db = -1);
        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<long> ListLeftPushAsync(string key, string value, int index_db = -1);
        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        Task<long> ListLeftPushAsync(string key, string[] values, int index_db = -1);

        #endregion

        #region ListRightPush
        /// <summary>
        /// 从右侧向list中添加值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        long ListRightPush(string key, string value, int index_db = -1);
        /// <summary>
        /// 从右侧向list中添加值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        long ListRightPush(string key, string[] values, int index_db = -1);
        /// <summary>
        /// 从右侧向list中添加值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<long> ListRightPushAsync(string key, string value, int index_db = -1);
        /// <summary>
        /// 从右侧向list中添加值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        Task<long> ListRightPushAsync(string key, string[] values, int index_db = -1);

        #endregion

        #region ListLeftPop
        /// <summary>
        /// 从list的头部移除一个数据，返回移除的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        string ListLeftPop(string key, int index_db = -1);
        /// <summary>
        /// 从list的头部移除一个数据，返回移除的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<string> ListLeftPopAsync(string key, int index_db = -1);

        #endregion

        #region ListRightPop
        /// <summary>
        /// 从list的尾部移除一个数据，返回移除的数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        string ListRightPop(string key, int index_db = -1);
        /// <summary>
        /// 从list的尾部移除一个数据，返回移除的数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<string> ListRightPopAsync(string key, int index_db = -1);

        #endregion

        #region ListLength
        /// <summary>
        /// 获取list的长度
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        long ListLength(string key, int index_db = -1);
        /// <summary>
        /// 获取list的长度
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<long> ListLengthAsync(string key, int index_db = -1);

        #endregion

        #region ListRange
        /// <summary>
        /// 获取某个范围数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始范围</param>
        /// <param name="stop">结束范围</param>
        /// <returns></returns>
        string[] ListRange(string key, long start = 0, long stop = -1, int index_db = -1);
        /// <summary>
        /// 获取某个范围数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始范围</param>
        /// <param name="stop">结束范围</param>
        /// <returns></returns>
        Task<string[]> ListRangeAsync(string key, long start = 0, long stop = -1, int index_db = -1);

        #endregion

        #region ListRemove
        /// <summary>
        /// 删除,返回删除成功个数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">要从列表中删除的值</param>
        /// <param name="count">计数</param>
        /// <returns></returns>
        long ListRemove(string key, string value, long count = 0, int index_db = -1);
        /// <summary>
        /// 删除,返回删除成功个数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">要从列表中删除的值</param>
        /// <param name="count">计数</param>
        /// <returns></returns>
        Task<long> ListRemoveAsync(string key, string value, long count = 0, int index_db = -1);

        #endregion

        #region ListTrim
        /// <summary>
        /// 获取列表指定范围内的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        void ListTrim(string key, long start, long stop, int index_db = -1);
        /// <summary>
        /// 获取列表指定范围内的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        Task ListTrimAsync(string key, long start, long stop, int index_db = -1);

        #endregion

        #region ListInsertAfter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pivot"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long ListInsertAfter(string key, string pivot, string value, int index_db = -1);
        /// <summary>
        /// 在pivot之后插入值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="pivot">要在其后插入的值</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        Task<long> ListInsertAfterAsync(string key, string pivot, string value, int index_db = -1);

        #endregion

        #region ListInsertBefore
        /// <summary>
        /// 在pivot之前插入值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="pivot">要在其前插入的值</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        long ListInsertBefore(string key, string pivot, string value, int index_db = -1);
        /// <summary>
        /// 在pivot之前插入值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="pivot">要在其前插入的值</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        Task<long> ListInsertBeforeAsync(string key, string pivot, string value, int index_db = -1);

        #endregion

        #region ListGetByIndex
        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        string ListGetByIndex(string key, long index, int index_db = -1);
        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        Task<string> ListGetByIndexAsync(string key, long index, int index_db = -1);

        #endregion

        #region ListSetByIndex
        /// <summary>
        ///  通过索引设置列表元素的值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="index">索引</param>
        /// <param name="value">值</param>
        void ListSetByIndex(string key, long index, string value, int index_db = -1);
        /// <summary>
        ///  通过索引设置列表元素的值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="index">索引</param>
        /// <param name="value">值</param>
        Task ListSetByIndexAsync(string key, long index, string value, int index_db = -1);

        #endregion

        #endregion

        #region 集合

        #region SetAdd
        /// <summary>
        /// 向集合添加一个值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool SetAdd(string key, string value, int index_db = -1);
        /// <summary>
        /// 向集合添加一个或多个值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">一个或多个值</param>
        /// <returns></returns>
        long SetAdd(string key, string[] values, int index_db = -1);
        /// <summary>
        /// 向集合添加一个值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> SetAddAsync(string key, string value, int index_db = -1);
        /// <summary>
        /// 向集合添加一个或多个值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">一个或多个值</param>
        /// <returns></returns>
        Task<long> SetAddAsync(string key, string[] values, int index_db = -1);

        #endregion

        #region SetCombine
        /// <summary>
        /// 返回由对给定集合的指定操作产生的集合成员
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="first">第一组集合Key</param>
        /// <param name="second">第二组集合Key</param>
        /// <returns></returns>
        string[] SetCombine(RedisSetOperation operation, string first, string second, int index_db = -1);
        /// <summary>
        /// 返回由对给定集合的指定操作产生的集合成员
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="first">第一组集合Key</param>
        /// <param name="second">第二组集合Key</param>
        /// <returns></returns>
        Task<string[]> SetCombineAsync(RedisSetOperation operation, string first, string second, int index_db = -1);
        /// <summary>
        /// 返回由对给定集合的指定操作产生的集合成员
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="keys">要操作的集合key集合</param>
        /// <returns></returns>
        string[] SetCombine(RedisSetOperation operation, string[] keys, int index_db = -1);
        /// <summary>
        /// 返回由对给定集合的指定操作产生的集合成员
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="keys">要操作的集合key集合</param>
        /// <returns></returns>
        Task<string[]> SetCombineAsync(RedisSetOperation operation, string[] keys, int index_db = -1);

        #endregion

        #region SetCombineAndStore
        /// <summary>
        /// 此命令等于setcombine，但不返回结果集，而是存储在desctination中。如果desctination已经存在，它将被覆盖。
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="desctination">存储key</param>
        /// <param name="first">第一组集合</param>
        /// <param name="second">第二组集合</param>
        /// <returns></returns>
        long SetCombineAndStore(RedisSetOperation operation, string desctination, string first, string second, int index_db = -1);
        /// <summary>
        /// 此命令等于setcombine，但不返回结果集，而是存储在desctination中。如果desctination已经存在，它将被覆盖。
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="desctination">存储key</param>
        /// <param name="first">第一组集合</param>
        /// <param name="second">第二组集合</param>
        /// <returns></returns>
        Task<long> SetCombineAndStoreAsync(RedisSetOperation operation, string desctination, string first, string second, int index_db = -1);
        /// <summary>
        /// 此命令等于setcombine，但不返回结果集，而是存储在desctination中。如果desctination已经存在，它将被覆盖。
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="desctination">存储key</param>
        /// <param name="keys">要操作的集合key集合</param>
        /// <returns></returns>
        long SetCombineAndStore(RedisSetOperation operation, string desctination, string[] keys, int index_db = -1);
        /// <summary>
        /// 此命令等于setcombine，但不返回结果集，而是存储在desctination中。如果desctination已经存在，它将被覆盖。
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="desctination">存储key</param>
        /// <param name="keys">要操作的集合key集合</param>
        /// <returns></returns>
        Task<long> SetCombineAndStoreAsync(RedisSetOperation operation, string desctination, string[] keys, int index_db = -1);

        #endregion

        #region SetContains
        /// <summary>
        /// 判断 value 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool SetContains(string key, string value, int index_db = -1);
        /// <summary>
        /// 判断 value 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> SetContainsAsync(string key, string value, int index_db = -1);

        #endregion

        #region SetLength
        /// <summary>
        /// 查询集合的长度
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        long SetLength(string key, int index_db = -1);
        /// <summary>
        /// 查询集合的长度
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<long> SetLengthAsync(string key, int index_db = -1);

        #endregion

        #region SetMembers
        /// <summary>
        /// 查询集合下的所有成员
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        string[] SetMembers(string key, int index_db = -1);
        /// <summary>
        /// 查询集合下的所有成员
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<string[]> SetMembersAsync(string key, int index_db = -1);

        #endregion

        #region SetMove
        /// <summary>
        /// 从源集合种移除一个值到目标集合中，如果目标集合已存在，那么只从源集合中删除，目标集合依然会存在
        /// </summary>
        /// <param name="source">源集合</param>
        /// <param name="desctination">目标集合</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool SetMove(string source, string desctination, string value, int index_db = -1);
        /// <summary>
        /// 从源集合种移除一个值到目标集合中，如果目标集合已存在，那么只从源集合中删除，目标集合依然会存在
        /// </summary>
        /// <param name="source">源集合</param>
        /// <param name="desctination">目标集合</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> SetMoveAsync(string source, string desctination, string value, int index_db = -1);

        #endregion

        #region SetPop
        /// <summary>
        ///  移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        string SetPop(string key, int index_db = -1);
        /// <summary>
        ///  移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<string> SetPopAsync(string key, int index_db = -1);

        #endregion

        #region SetRandomMember
        /// <summary>
        ///  返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        string SetRandomMember(string key, int index_db = -1);
        /// <summary>
        ///  返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        Task<string> SetRandomMemberAsync(string key, int index_db = -1);

        #endregion

        #region SetRandomMembers
        /// <summary>
        /// 返回集合中一个或多个随机数的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="count">返回个数</param>
        /// <returns></returns>
        string[] SetRandomMembers(string key, long count, int index_db = -1);
        /// <summary>
        /// 返回集合中一个或多个随机数的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="count">返回个数</param>
        /// <returns></returns>
        Task<string[]> SetRandomMembersAsync(string key, long count, int index_db = -1);

        #endregion

        #region SetRemove
        /// <summary>
        /// 移除集合中一个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool SetRemove(string key, string value, int index_db = -1);
        /// <summary>
        /// 移除集合中一个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> SetRemoveAsync(string key, string value, int index_db = -1);
        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        long SetRemove(string key, string[] values, int index_db = -1);
        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        Task<long> SetRemoveAsync(string key, string[] values, int index_db = -1);

        #endregion

        #region SetScan
        /// <summary>
        /// 迭代集合中的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="pattern">模式</param>
        /// <param name="pageSize">每页多少条</param>
        /// <param name="cursor">位置</param>
        /// <param name="pageOffset">第几页</param>
        /// <returns></returns>
        string[] SetScan(string key, string pattern = null, int pageSize = 0, long cursor = 0, int pageOffset = 0, int index_db = -1);

        #endregion

        #endregion

        #region 有序集合

        #region SortedSetAdd
        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="score">一个或多个成员分数</param>
        /// <returns></returns>
        bool SortedSetAdd(string key, string member, double score, int index_db = -1);
        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="score">一个或多个成员分数</param>
        /// <returns></returns>
        Task<bool> SortedSetAddAsync(string key, string member, double score, int index_db = -1);
        /// <summary>
        /// 将具有指定分数的所有指定成员添加到存储在键处的排序集。如果指定的成员已经是排序集的成员，则分数更新并在正确位置重新插入元件，以确保正确排序。
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="members">成员集合</param>
        /// <returns></returns>
        long SortedSetAdd(string key, IDictionary<string, double> members, int index_db = -1);
        /// <summary>
        /// 将具有指定分数的所有指定成员添加到存储在键处的排序集。如果指定的成员已经是排序集的成员，则分数更新并在正确位置重新插入元件，以确保正确排序。
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="members">成员集合</param>
        /// <returns></returns>
        Task<long> SortedSetAddAsync(string key, IDictionary<string, double> members, int index_db = -1);

        #endregion

        #region SortedSetCombineAndStore
        /// <summary>
        /// 对两个排序集计算集合操作，并将结果存储在desctination中，可以选择执行特定聚合（默认值为SUM）。
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="desctination">目标集合</param>
        /// <param name="first">第一组集合</param>
        /// <param name="second">第二组集合</param>
        /// <param name="aggregate">特定聚合（默认值为SUM）</param>
        /// <returns></returns>
        long SortedSetCombineAndStore(RedisSetOperation operation, string desctination, string first, string second, RedisAggregate aggregate = RedisAggregate.Sum, int index_db = -1);
        /// <summary>
        /// 对两个排序集计算集合操作，并将结果存储在desctination中，可以选择执行特定聚合（默认值为SUM）。
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="desctination">目标集合</param>
        /// <param name="first">第一组集合</param>
        /// <param name="second">第二组集合</param>
        /// <param name="aggregate">特定聚合（默认值为SUM）</param>
        /// <returns></returns>
        Task<long> SortedSetCombineAndStoreAsync(RedisSetOperation operation, string desctination, string first, string second, RedisAggregate aggregate = RedisAggregate.Sum, int index_db = -1);
        /// <summary>
        /// 对多个排序集计算集合操作，并将结果存储在desctination中，可以选择执行特定聚合（默认值为SUM）。
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="desctination">目标集合</param>
        /// <param name="keys">要操作的集合</param>
        /// <param name="weights">权重</param>
        /// <param name="aggregate">特定聚合（默认值为SUM）</param>
        /// <returns></returns>
        long SortedSetCombineAndStore(RedisSetOperation operation, string desctination, string[] keys, double[] weights = null, RedisAggregate aggregate = RedisAggregate.Sum, int index_db = -1);
        /// <summary>
        /// 对多个排序集计算集合操作，并将结果存储在desctination中，可以选择执行特定聚合（默认值为SUM）。
        /// </summary>
        /// <param name="operation">操作类型</param>
        /// <param name="desctination">目标集合</param>
        /// <param name="keys">要操作的集合</param>
        /// <param name="weights">权重</param>
        /// <param name="aggregate">特定聚合（默认值为SUM）</param>
        /// <returns></returns>
        Task<long> SortedSetCombineAndStoreAsync(RedisSetOperation operation, string desctination, string[] keys, double[] weights = null, RedisAggregate aggregate = RedisAggregate.Sum, int index_db = -1);
        #endregion

        #region SortedSetDecrement
        /// <summary>
        /// 有序集合中对指定成员的分数减去减量
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="value">减量</param>
        /// <returns></returns>
        double SortedSetDecrement(string key, string member, double value, int index_db = -1);
        /// <summary>
        /// 有序集合中对指定成员的分数减去减量
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="value">减量</param>
        /// <returns></returns>
        Task<double> SortedSetDecrementAsync(string key, string member, double value, int index_db = -1);

        #endregion

        #region SortedSetIncrement
        /// <summary>
        /// 有序集合中对指定成员的分数加上增量
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="value">增量</param>
        /// <returns></returns>
        double SortedSetIncrement(string key, string member, double value, int index_db = -1);
        /// <summary>
        /// 有序集合中对指定成员的分数加上增量
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="value">增量</param>
        /// <returns></returns>
        Task<double> SortedSetIncrementAsync(string key, string member, double value, int index_db = -1);

        #endregion

        #region SortedSetLength
        /// <summary>
        /// 返回有序集中指定分数区间内的成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">分数最小（默认无穷负）</param>
        /// <param name="max">分数最大（默认无穷大）</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <returns></returns>
        long SortedSetLength(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, int index_db = -1);
        /// <summary>
        /// 返回有序集中指定分数区间内的成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">分数最小（默认无穷负）</param>
        /// <param name="max">分数最大（默认无穷大）</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <returns></returns>
        Task<long> SortedSetLengthAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, int index_db = -1);

        #endregion

        #region SortedSetLengthByValue
        /// <summary>
        /// 返回有序集中指定分数区间内的成员个数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <returns></returns>
        long SortedSetLengthByValue(string key, string min, string max, RedisExclude exclude = RedisExclude.None, int index_db = -1);
        /// <summary>
        /// 返回有序集中指定分数区间内的成员个数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <returns></returns>
        Task<long> SortedSetLengthByValueAsync(string key, string min, string max, RedisExclude exclude = RedisExclude.None, int index_db = -1);

        #endregion

        #region SortedSetRangeByRank
        /// <summary>
        ///  返回有序集中指定区间内的成员和分数，通过索引
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="order">排序正序或倒序（默认正序）</param>
        /// <returns></returns>
        string[] SortedSetRangeByRank(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending, int index_db = -1);
        /// <summary>
        ///  返回有序集中指定区间内的成员列表，通过索引
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="order">排序正序或倒序（默认正序）</param>
        /// <returns></returns>
        Task<string[]> SortedSetRangeByRankAsync(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending, int index_db = -1);

        #endregion

        #region SortedSetRangeByRankWithScores
        /// <summary>
        /// 返回有序集中指定区间内的成员和分数，通过索引
        /// </summary> 
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="order">排序正序或倒序（默认正序）</param>
        /// <returns></returns>
        KeyValuePair<string, double>[] SortedSetRangeByRankWithScores(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending, int index_db = -1);
        /// <summary>
        /// 返回有序集中指定区间内的成员和分数，通过索引
        /// </summary> 
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="order">排序正序或倒序（默认正序）</param>
        /// <returns></returns>
        Task<KeyValuePair<string, double>[]> SortedSetRangeByRankWithScoresAsync(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending, int index_db = -1);

        #endregion

        #region SortedSetRangeByScore
        /// <summary>
        /// 返回有序集中指定区间内的成员，通过索引
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <param name="order"></param>
        /// <param name="skip">第多少条开始</param>
        /// <param name="take">显示多少条</param>
        /// <returns></returns>
        string[] SortedSetRangeByScore(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1, int index_db = -1);
        /// <summary>
        /// 返回有序集中指定区间内的成员，通过索引
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <param name="order"></param>
        /// <param name="skip">第多少条开始</param>
        /// <param name="take">显示多少条</param>
        /// <returns></returns>
        Task<string[]> SortedSetRangeByScoreAsync(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1, int index_db = -1);
        #endregion

        #region SortedSetRangeByScoreWithScores
        /// <summary>
        /// 返回有序集中指定区间内的成员和分数，通过索引
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <param name="order"></param>
        /// <param name="skip">第多少条开始</param>
        /// <param name="take">显示多少条</param>
        /// <returns></returns>
        KeyValuePair<string, double>[] SortedSetRangeByScoreWithScores(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1, int index_db = -1);
        /// <summary>
        /// 返回有序集中指定区间内的成员和分数，通过索引
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <param name="order"></param>
        /// <param name="skip">第多少条开始</param>
        /// <param name="take">显示多少条</param>
        /// <returns></returns>
        Task<KeyValuePair<string, double>[]> SortedSetRangeByScoreWithScoresAsync(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1, int index_db = -1);

        #endregion

        #region SortedSetRangeByValue
        /// <summary>
        /// 返回有序集中指定分数区间内的成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <param name="skip">第多少条开始</param>
        /// <param name="take">显示多少条</param>
        /// <returns></returns>

        string[] SortedSetRangeByValue(string key, string min = null, string max = null, RedisExclude exclude = RedisExclude.None, long skip = 0, long take = -1, int index_db = -1);
        /// <summary>
        /// 返回有序集中指定分数区间内的成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <param name="skip">第多少条开始</param>
        /// <param name="take">显示多少条</param>
        /// <returns></returns>
        Task<string[]> SortedSetRangeByValueAsync(string key, string min = null, string max = null, RedisExclude exclude = RedisExclude.None, long skip = 0, long take = -1, int index_db = -1);

        #endregion

        #region SortedSetRank
        /// <summary>
        /// 返回存储在键上的已排序集中成员的排名，默认情况下，分数从低到高排序。排名（或指数）以0为基础，这意味着得分最低的成员的排名为0。
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="order">排序顺序（默认正序）</param>
        /// <returns></returns>
        long? SortedSetRank(string key, string member, RedisOrder order = RedisOrder.Ascending, int index_db = -1);
        /// <summary>
        /// 返回存储在键上的已排序集中成员的排名，默认情况下，分数从低到高排序。排名（或指数）以0为基础，这意味着得分最低的成员的排名为0。
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <param name="order">排序顺序（默认正序）</param>
        /// <returns></returns>
        Task<long?> SortedSetRankAsync(string key, string member, RedisOrder order = RedisOrder.Ascending, int index_db = -1);

        #endregion

        #region SortedSetRemove
        /// <summary>
        /// 移除有序集合中的一个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        bool SortedSetRemove(string key, string member, int index_db = -1);
        /// <summary>
        /// 移除有序集合中的一个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        long SortedSetRemove(string key, string[] members, int index_db = -1);
        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        Task<bool> SortedSetRemoveAsync(string key, string member, int index_db = -1);
        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        Task<long> SortedSetRemoveAsync(string key, string[] members, int index_db = -1);

        #endregion

        #region SortedSetRemoveRangeByRank
        /// <summary>
        /// 删除存储在键处的排序集中的所有元素，其排名介于开始和停止之间。
        /// 开始和停止都是基于0的索引，0是得分最低的元素。
        /// 这些指数可以是负数，表示从得分最高的元素开始的偏移量。
        /// 例如，-1是得分最高的元素，-2是得分第二的元素，以此类推。
        /// 返回删除的元素数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        long SortedSetRemoveRangeByRank(string key, long start, long stop, int index_db = -1);
        /// <summary>
        /// 删除存储在键处的排序集中的所有元素，其排名介于开始和停止之间。
        /// 开始和停止都是基于0的索引，0是得分最低的元素。
        /// 这些指数可以是负数，表示从得分最高的元素开始的偏移量。
        /// 例如，-1是得分最高的元素，-2是得分第二的元素，以此类推。
        /// 返回删除的元素数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        Task<long> SortedSetRemoveRangeByRankAsync(string key, long start, long stop, int index_db = -1);

        #endregion

        #region SortedSetRemoveRangeByScore
        /// <summary>
        /// 删除存储在键处的排序集中的所有元素，其排名介于开始和停止之间。
        /// 开始和停止都是基于0的索引，0是得分最低的元素。
        /// 这些指数可以是负数，表示从得分最高的元素开始的偏移量。
        /// 例如，-1是得分最高的元素，-2是得分第二的元素，以此类推。
        /// 返回删除的元素数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <returns></returns>
        long SortedSetRemoveRangeByScore(string key, double start, double stop, RedisExclude exclude = RedisExclude.None, int index_db = -1);
        /// <summary>
        /// 删除存储在键处的排序集中的所有元素，其排名介于开始和停止之间。
        /// 开始和停止都是基于0的索引，0是得分最低的元素。
        /// 这些指数可以是负数，表示从得分最高的元素开始的偏移量。
        /// 例如，-1是得分最高的元素，-2是得分第二的元素，以此类推。
        /// 返回删除的元素数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <returns></returns>

        Task<long> SortedSetRemoveRangeByScoreAsync(string key, double start, double stop, RedisExclude exclude = RedisExclude.None, int index_db = -1);

        #endregion

        #region SortedSetRemoveRangeByValue
        /// <summary>
        /// 删除此排序区间的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <returns></returns>
        long SortedSetRemoveRangeByValue(string key, string min, string max, RedisExclude exclude = RedisExclude.None, int index_db = -1);
        /// <summary>
        /// 删除此排序区间的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="exclude">是否从范围检查中排除最小值和最大值（默认为包含这两个值）</param>
        /// <returns></returns>
        Task<long> SortedSetRemoveRangeByValueAsync(string key, string min, string max, RedisExclude exclude = RedisExclude.None, int index_db = -1);

        #endregion

        #region SortedSetScan
        /// <summary>
        /// 迭代有序集合中的元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="pattern">模式</param>
        /// <param name="pageSize">每页多少条</param>
        /// <param name="cursor">位置</param>
        /// <param name="pageOffset">第几页</param>
        /// <returns></returns>
        KeyValuePair<string, double>[] SortedSetScan(string key, string pattern = null, int pageSize = 10, int cursor = 0, int pageOffset = 0, int index_db = -1);

        #endregion

        #region SortedSetScore
        /// <summary>
        /// 返回成员分数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        double? SortedSetScore(string key, string member, int index_db = -1);

        /// <summary>
        /// 返回成员分数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        Task<double?> SortedSetScoreAsync(string key, string member, int index_db = -1);


        #endregion

        #endregion

    
        #region 分布式锁


        /// <summary>
        /// 获取锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>是否已锁。</returns>
        bool Lock(string key, string lockToken, int seconds, int index_db = -1);

        /// <summary>
        /// 释放锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>是否成功。</returns>
        bool UnLock(string key, string lockToken, int index_db = -1);
        /// <summary>
        /// 查询锁
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>锁token</returns>
        string LockQuery(string key, int index_db = -1);

        /// <summary>
        /// 异步查询锁
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>锁token</returns>
        Task<string> LockQueryAsync(string key, int index_db = -1);
        /// <summary>
        /// 异步获取锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>是否成功。</returns>
        Task<bool> LockAsync(string key, string lockToken, int seconds, int index_db = -1);


        /// <summary>
        /// 异步释放锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>是否成功。</returns>
        Task<bool> UnLockAsync(string key, string lockToken, int index_db = -1);


        #endregion
        #region Lua脚本
        /// <summary>
        /// 通过keys进行模糊查询后的批量删除
        /// </summary>
        /// <param name="key"></param>
        void ScriptEvaluateDelete(string key, int index_db = -1);

        #endregion
    }
}
