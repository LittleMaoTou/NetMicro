using NetMicro.Data.Abstractions.Entities;

namespace NetMicro.Data.Provider.Entities
{
    /// <summary>
    /// 包含指定类型主键的实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class Entity<TKey> : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TKey Id { get; set; }
    }

    /// <summary>
    /// 主键类型为long的实体
    /// </summary>
    public class Entity : Entity<long>
    {

    }
}
