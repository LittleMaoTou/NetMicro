using NetMicro.Core.Exceptions;
using NetMicro.Core.Ids;
using NetMicro.Core.Validations;
using NetMicro.Validation.Abstractions;
using System;

namespace NetMicro.EventBus.Abstractions
{
    /// <summary>
    /// 事件
    /// </summary>
    public abstract class Event : IEvent
    {
        /// <summary>
        /// 初始化事件
        /// </summary>
        public Event(string eventId)
        {
            EventId = eventId;
            SendTime = DateTime.Now;
        }
        /// <summary>
        /// 初始化事件
        /// </summary>
        public Event()
        {
            EventId = IdBuilder.Guid;
            SendTime = DateTime.Now;
        }

        /// <summary>
        /// 事件标识
        /// </summary>
        public string EventId { get; set; }
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime SendTime { get; }

        /// <summary>
        /// 验证
        /// </summary>
        public virtual ValidationResultCollection Validate()
        {
            var result = DataAnnotationValidation.Validate(this);
            if (result.IsValid)
                return ValidationResultCollection.Success;
            throw new ValidationException(result.ToString(), 400);
        }

    }
}
