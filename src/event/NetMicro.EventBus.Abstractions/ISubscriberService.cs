using DotNetCore.CAP;

namespace NetMicro.EventBus.Abstractions
{
    /// <summary>
    /// 订阅服务需继承此标识接口
    /// </summary>
    public interface ISubscriberService : ICapSubscribe
    {
    }
}
