using System;

namespace NetMicro.Cache.Abstractions
{
    /// <summary>
    /// Redis 数据类型
    /// </summary>
    public enum RedisItemType
    {
        /// <summary>
        /// 指定的密钥不存在
        /// </summary>
        None = 0,
        /// <summary>
        /// string 是 redis 最基本的类型，你可以理解成与 Memcached 一模一样的类型，一个 key 对应一个 value。
        /// string 类型是二进制安全的。意思是 redis 的 string 可以包含任何数据。比如jpg图片或者序列化的对象。
        /// string 类型是 Redis 最基本的数据类型，string 类型的值最大能存储 512MB。
        /// </summary>
        String = 1,
        /// <summary>
        /// Redis 列表是简单的字符串列表，按照插入顺序排序。你可以添加一个元素到列表的头部（左边）或者尾部（右边）。
        /// </summary>
        List = 2,
        /// <summary>
        /// Redis的Set是string类型的无序集合。
        /// 集合是通过哈希表实现的，所以添加，删除，查找的复杂度都是O(1)。
        /// </summary>
        Set = 3,
        /// <summary>
        /// Redis zset 和 set 一样也是string类型元素的集合,且不允许重复的成员。
        /// 不同的是每个元素都会关联一个double类型的分数。redis正是通过分数来为集合中的成员进行从小到大的排序。
        /// zset的成员是唯一的, 但分数(score)却可以重复。
        /// </summary>
        SortedSet = 4,
        /// <summary>
        /// Redis hash 是一个键值(key=>value)对集合。
        /// Redis hash 是一个 string 类型的 field 和 value 的映射表，hash 特别适合用于存储对象。
        /// </summary>
        Hash = 5,
        /// <summary>
        /// 无法识别该数据类型
        /// </summary>
        Unknown = 6,
    }
    /// <summary>
    /// 描述一个代数集操作，可以执行该操作来组合多个集
    /// </summary>
    public enum RedisSetOperation
    {
        /// <summary>
        /// 返回由所有给定集的并集产生的集的成员。
        /// </summary>
        Union,
        /// <summary>
        /// 返回由所有给定集的交集产生的集的成员。
        /// </summary>
        Intersect,
        /// <summary>
        /// 返回由于第一个集与所有后续集之间的差异而产生的集成员。
        /// </summary>
        Difference
    }
    /// <summary>
    /// 指定在组合排序集时应如何聚合元素
    /// </summary>
    public enum RedisAggregate
    {
        /// <summary>
        /// 添加组合元素的值
        /// </summary>
        Sum,
        /// <summary>
        /// 使用组合元素的最小值
        /// </summary>
        Min,
        /// <summary>
        /// 使用组合元素的最大值
        /// </summary>
        Max
    }
    /// <summary>
    /// 执行范围查询时，默认开始/停止限制是包含的；但是，两者也可以单独指定为独占
    /// </summary>
    [Flags]
    public enum RedisExclude
    {
        /// <summary>
        /// 开始和停止都包含在内
        /// </summary>
        None = 0,
        /// <summary>
        /// 开始是独占的，停止是包含的
        /// </summary>
        Start = 1,
        /// <summary>
        /// 开始是包含的，停止是独占的
        /// </summary>
        Stop = 2,
        /// <summary>
        /// 开始和停止都是独占的
        /// </summary>
        Both = Start | Stop
    }
    /// <summary>
    /// 排序
    /// </summary>
    public enum RedisOrder
    {
        /// <summary>
        /// 正序
        /// </summary>
        Ascending,
        /// <summary>
        /// 反序
        /// </summary>
        Descending
    }
}
