using NetMicro.Core.Pagination;
using NetMicro.Validation.Provider.Dtos;
using System.Collections.Generic;

namespace NetMicro.Validation.Provider.Pagination
{
    /// <summary>
    /// 查询分页模型
    /// </summary>
    public class PagingDto : DtoBase
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
    /// 查询分页模型
    /// </summary>
    public class PagingDto<T> : PagingDto
    {
        /// <summary>
        /// 过滤
        /// </summary>
        public T Filter { get; set; }
    }
}
