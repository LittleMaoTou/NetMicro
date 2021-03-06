﻿using System;
using NetMicro.Data.Abstractions.Attributes;

namespace NetMicro.Data.Provider.Entities.Extend
{
    public class EntityBaseWithSoftDelete<TKey> : EntityWithSoftDelete<TKey>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        public TKey CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改人
        /// </summary>
        public TKey ModifiedBy { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Ignore]
        public string Creator { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Ignore]
        public string Modifier { get; set; }
    }

    /// <summary>
    /// 包含软删除功能的实体基类
    /// </summary>
    public class EntityBaseWithSoftDelete : EntityBaseWithSoftDelete<long>
    {

    }
}
