﻿using System;

namespace NetMicro.Core.Attributes
{
    /// <summary>
    /// 单例注入(使用该特性的服务系统会自动注入)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class ScopedAttribute : Attribute
    {
        /// <summary>
        /// 是否使用自身的类型进行注入
        /// </summary>
        public bool Itself { get; set; }

        /// <summary>
        /// 是否使用自身的类型进行注入
        /// </summary>
        /// <param name="itself"></param>
        public ScopedAttribute(bool itself = false)
        {
            Itself = itself;
        }
    }
}
