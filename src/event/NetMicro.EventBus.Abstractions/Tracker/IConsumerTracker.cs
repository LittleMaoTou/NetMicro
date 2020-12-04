using System.Threading.Tasks;

namespace NetMicro.EventBus.Abstractions.Tracker
{
    /// <summary>
    /// 消费者跟踪，保证幂等
    /// </summary>
    public interface IConsumerTracker
    {
        /// <summary>
        /// 是否已消费
        /// </summary>
        /// <returns></returns>
        Task<bool> HasProcessedAsync(string eventId);

        /// <summary>
        /// 已消费存储
        /// </summary>
        /// <returns></returns>
        Task<bool> MarkAsProcessedAsync(string eventId);




        /// <summary>
        /// 是否已消费
        /// </summary>
        /// <returns></returns>
        bool HasProcessed(string eventId);

        /// <summary>
        /// 已消费存储
        /// </summary>
        /// <returns></returns>
        bool MarkAsProcessed(string eventId);



    }
}
