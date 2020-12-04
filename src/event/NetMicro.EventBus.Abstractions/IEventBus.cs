using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetMicro.EventBus.Abstractions
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public interface IEventBus
    {
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <param name="callback"></param>
        void Publish<TEvent>(string name, TEvent @event, string callback = null) where TEvent : Event;

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <param name="headers"></param>
        void Publish<TEvent>(string name, TEvent @event, IDictionary<string, string> headers) where TEvent : Event;

        /// <summary>
        ///  发布事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        void Publish(string name, object data, string callback = null);


        /// <summary>
        ///  发布事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        void Publish(string name, object data, IDictionary<string, string> headers);


        /// <summary>
        ///  发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task PublishAsync<TEvent>(string name, TEvent @event, string callback = null) where TEvent : Event;


        /// <summary>
        ///  发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="name"></param>
        /// <param name="event"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task PublishAsync<TEvent>(string name, TEvent @event, IDictionary<string, string> headers) where TEvent : Event;



        /// <summary>
        ///  发布事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task PublishAsync(string name, object data, string callback = null);


        /// <summary>
        ///  发布事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task PublishAsync(string name, object data, IDictionary<string, string> headers);



    }
}
