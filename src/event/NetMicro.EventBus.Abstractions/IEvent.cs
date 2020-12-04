using System;
using NetMicro.Validation.Abstractions;

namespace NetMicro.EventBus.Abstractions
{
    /// <summary>
    /// 事件
    /// </summary>
    public interface IEvent : IValidation
    {
        /// <summary>
        /// 事件标识
        /// </summary>
        string EventId { get; set; }
        /// <summary>
        /// 事件时间
        /// </summary>
        DateTime SendTime { get; }
    }
}
