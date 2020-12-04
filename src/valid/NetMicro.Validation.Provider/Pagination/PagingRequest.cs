using System.Collections.Generic;
using NetMicro.Core.Applications.Dtos;
using NetMicro.Core.Pagination;

namespace NetMicro.Validation.Provider.Pagination
{
    /// <summary>
    /// 查询分页
    /// </summary>
    public class PagingRequest : RequestBase
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int Index { get; set; } = 1;

        /// <summary>
        /// 页大小
        /// </summary>
        public int Size { get; set; } = 15;

        /// <summary>
        /// 排序字段
        /// </summary>
        public List<Sort> Sort { get; set; }
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    public class PagingRequest<T> : PagingRequest
    {
        /// <summary>
        /// 过滤
        /// </summary>
        public T Filter { get; set; }
    }
}
