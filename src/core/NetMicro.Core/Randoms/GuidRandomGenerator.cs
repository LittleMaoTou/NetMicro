﻿using NetMicro.Core.Attributes;
using NetMicro.Core.Ids;

namespace NetMicro.Core.Randoms
{
    /// <summary>
    /// Guid随机数生成器，每次创建一个新的Guid字符串，去掉了Guid的分隔符
    /// </summary>
    [Singleton]
    public class GuidRandomGenerator : IRandomGenerator
    {
        /// <summary>
        /// 生成随机数
        /// </summary>
        public string Generate()
        {
            return IdBuilder.Guid;
        }

        /// <summary>
        /// Guid随机数生成器实例
        /// </summary>
        public static readonly IRandomGenerator Instance = new GuidRandomGenerator();
    }
}
