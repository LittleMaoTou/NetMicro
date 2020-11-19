using Newtonsoft.Json;

namespace NetMicro.WebFunction.Contract
{
    public interface IApiResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonIgnore]
        bool Successful { get; }

        /// <summary>
        /// 成功或失败标记
        /// </summary>

        int Code { get; set; }


        /// <summary>
        /// 请求id
        /// </summary>

        string RequestId { get; set; }

        /// <summary>
        /// msg信息
        /// </summary>

        string Msg { get; set; }

    }

    public interface IApiResult<TData> : IApiResult
    {
        /// <summary>
        /// 返回值
        /// </summary>
        TData ResultData { get; set; }


    }
}
