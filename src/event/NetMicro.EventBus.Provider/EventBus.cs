using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using NetMicro.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetMicro.EventBus.Provider
{
    /// <summary>
    /// Cap事件总线
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly ICapPublisher _publisher;
        private readonly ILogger<EventBus> _logger;
        public EventBus(ICapPublisher publisher, ILogger<EventBus> logger, IServiceProvider provider)
        {
            _publisher = publisher;
            _logger = logger;
            ServiceProvider = provider;
        }

        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <param name="callback"></param>
        public void Publish<TEvent>(string name, TEvent @event, string callback = null) where TEvent : Event
        {
            @event.Validate();
            _publisher.Publish(name, @event, callback);
        }
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <param name="headers"></param>
        public void Publish<TEvent>(string name, TEvent @event, IDictionary<string, string> headers) where TEvent : Event
        {
            @event.Validate();
            _publisher.Publish(name, @event, headers);
        }
        /// <summary>
        ///  发布事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void Publish(string name, object data, string callback = null)
        {

            _publisher.Publish(name, data, callback);
        }

        /// <summary>
        ///  发布事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        public void Publish(string name, object data, IDictionary<string, string> headers)
        {
            _publisher.Publish(name, data, headers);
        }

        /// <summary>
        ///  发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task PublishAsync<TEvent>(string name, TEvent @event, string callback = null) where TEvent : Event
        {
            @event.Validate();
            await _publisher.PublishAsync(name, @event, callback);
        }

        /// <summary>
        ///  发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task PublishAsync<TEvent>(string name, TEvent @event, IDictionary<string, string> headers) where TEvent : Event
        {
            @event.Validate();
            await _publisher.PublishAsync(name, @event, headers);
        }


        /// <summary>
        ///  发布事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task PublishAsync(string name, object data, string callback = null)
        {
            await _publisher.PublishAsync(name, data, callback);
        }

        /// <summary>
        ///  发布事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task PublishAsync(string name, object data, IDictionary<string, string> headers)
        {
            await _publisher.PublishAsync(name, data, headers);
        }






    }
}
