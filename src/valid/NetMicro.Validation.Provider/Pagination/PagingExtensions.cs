using NetMicro.Validation.Provider.Pagination;
using System.Linq;

namespace NetMicro.Core.Pagination
{
    public static class PagingExtensions
    {
        /// <summary>
        /// 获取Paging分页类
        /// </summary>
        public static Paging Paging(this PagingDto model)
        {
            var paging = new Paging(model.Index, model.Size);
            if (model.Sort != null && model.Sort.Any())
            {
                foreach (var sort in model.Sort)
                {
                    paging.OrderBy.Add(new Sort(sort.OrderBy, sort.Type));
                }
            }
            return paging;
        }

        /// <summary>
        /// 获取Paging分页类
        /// </summary>
        public static Paging Paging<T>(this PagingDto<T> model)
        {
            var paging = new Paging(model.Index, model.Size);
            if (model.Sort != null && model.Sort.Any())
            {
                foreach (var sort in model.Sort)
                {
                    paging.OrderBy.Add(new Sort(sort.OrderBy, sort.Type));
                }
            }
            return paging;
        }
    }
}
