using Newtonsoft.Json;
using System.Collections.Generic;

namespace NetMicro.Validation.Provider.Pagination
{
    /// <summary>
    /// 查询结果模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingReponse<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        public IList<T> Rows { get; set; }

        /// <summary>
        /// 其他数据
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    }
}
