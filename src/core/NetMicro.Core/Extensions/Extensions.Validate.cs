using System;
using System.Collections.Generic;
using System.Linq;

namespace NetMicro.Core.Extensions
{
    /// <summary>
    /// 系统扩展 - 验证
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 检测对象是否为null,为null则抛出<see cref="ArgumentNullException"/>异常
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="parameterName">参数名</param>
        public static void CheckNull(this object obj, string parameterName)
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 判断Guid是否为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmpty(this Guid s)
        {
            return s == Guid.Empty;
        }

        /// <summary>
        /// 判断Guid是否不为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool NotEmpty(this Guid s)
        {
            return s != Guid.Empty;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty<T>(this IEnumerable<T> value)
        {
            if (value == null)
                return true;
            return !value.Any();
        }
    }
}
