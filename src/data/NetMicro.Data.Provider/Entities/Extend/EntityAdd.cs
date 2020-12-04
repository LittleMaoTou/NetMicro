using System;
using NetMicro.Data.Abstractions.Attributes;

namespace NetMicro.Data.Provider.Entities.Extend
{
    public class EntityAdd<TKey> : Entity<TKey>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        public TKey CreatedBy { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Ignore]
        public string Creator { get; set; }

    }

    public class EntityAdd : EntityAdd<long>
    {

    }
}
