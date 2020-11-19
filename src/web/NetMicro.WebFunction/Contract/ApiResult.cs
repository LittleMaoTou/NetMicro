using NetMicro.Core.Extensions;
using Newtonsoft.Json;

namespace NetMicro.WebFunction.Contract
{
    /// <summary>
    /// API返回基类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiResult<TData> : IApiResult<TData>
    {

        /// <summary>
        /// 处理是否成功
        /// </summary>
        [JsonIgnore]
        public bool Successful { get; private set; }
        /// <summary>
        /// 成功或失败标记
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; } = ErrorCode.Success.GetHashCode();


        /// <summary>
        /// 请求id
        /// </summary>
        [JsonProperty("rid")]
        public string RequestId { get; set; } //= ApiContext.RequestId;



        /// <summary>
        /// msg信息
        /// </summary>
        [JsonProperty("msg")]
        public string Msg { get; set; }


        /// <summary>
        /// 返回值
        /// </summary>
        [JsonProperty("data")]
        public TData ResultData { get; set; }

        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public ApiResult<TData> Error(ErrorCode errCode)
        {
            Successful = false;
            Code = errCode.GetHashCode();
            Msg = errCode.ToDescription();
            return this;
        }

        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ApiResult<TData> Error(int errCode, string message)
        {
            Successful = false;
            Code = errCode;
            Msg = message;
            return this;
        }

        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public ApiResult<TData> Error(string message = null)
        {
            Successful = false;
            Code = ErrorCode.Error.GetHashCode();
            Msg = message ?? "失败";
            return this;
        }

        /// <summary>
        /// 生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public ApiResult<TData> Succees(TData data = default)
        {
            Successful = true;
            Code = ErrorCode.Success.GetHashCode();
            Msg = ErrorCode.Success.ToDescription();
            ResultData = data;
            return this;
        }


    }


    /// <summary>
    /// API返回数据泛型类
    /// </summary>

    public static class ApiResult
    {

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static IApiResult Success<T>(T data = default(T))
        {
            return new ApiResult<T>().Succees(data);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static IApiResult Success()
        {
            return Success<string>();
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static IApiResult Error<T>(int errCode, string message)
        {
            return new ApiResult<T>().Error(errCode, message);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <returns></returns>
        public static IApiResult Error(int errCode, string message)
        {
            return Error<string>(errCode, message);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IApiResult Error<T>(ErrorCode errCode)
        {
            return Error<T>(errCode.GetHashCode(), errCode.ToDescription());
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IApiResult Error(ErrorCode errCode)
        {
            return Error<string>(errCode);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IApiResult Error<T>(string message = null)
        {
            return Error<T>(ErrorCode.Error.GetHashCode(), message ?? "失败");
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IApiResult Error(string message = null)
        {
            return Error<string>(message);
        }


        /// <summary>
        /// 根据布尔值返回结果
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IApiResult Result<T>(bool success)
        {
            return success ? Success<T>() : Error<T>();
        }

        /// <summary>
        /// 根据布尔值返回结果
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IApiResult Result(bool success)
        {
            return success ? Success() : Error();
        }

        /// <summary>
        /// 数据已存在
        /// </summary>
        /// <returns></returns>
        public static IApiResult HasExists => Error("数据已存在");

        /// <summary>
        /// 数据不存在
        /// </summary>
        public static IApiResult NotExists => Error("数据不存在");
    }
}
