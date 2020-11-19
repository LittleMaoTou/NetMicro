using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace NetMicro.Core.Extensions
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static partial class Extensions
    {
        private static readonly ConcurrentDictionary<string, string> DescriptionCache = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// 获取枚举类型的Description说明
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescriptionWithCache(this Enum value)
        {
            var type = value.GetType();
            var info = type.GetField(value.ToString());
            var key = type.FullName + info.Name;
            if (!DescriptionCache.TryGetValue(key, out string desc))
            {
                desc = EnumConvert(info, value);
                DescriptionCache.TryAdd(key, desc);
            }
            return desc;
        }

        /// <summary>
        /// 获取枚举类型的Description说明
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            var info = value.GetType().GetField(value.ToString());
            return EnumConvert(info, value);
        }

        static string EnumConvert(FieldInfo info, Enum value)
        {
            var attrs = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs.Length < 1)
                return string.Empty;
            else
                return attrs[0] is DescriptionAttribute
                     descriptionAttribute
                     ? descriptionAttribute.Description
                     : value.ToString();
        }

    }
}
