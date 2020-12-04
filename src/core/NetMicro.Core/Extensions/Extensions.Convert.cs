using System;
using System.Collections.Generic;
using System.Linq;
using NetMicro.Core.Helper;
using Convert = NetMicro.Core.Helper.Convert;

namespace NetMicro.Core.Extensions
{
    /// <summary>
    /// 系统扩展 - 类型转换
    /// </summary>
    public static partial class Extensions
    {


        /// <summary>
        /// 转换成Byte
        /// </summary>
        /// <param name="s">输入字符串</param>
        /// <returns></returns>
        public static byte ToByte(this object s)
        {
            if (s == null || s == DBNull.Value)
                return 0;

            byte.TryParse(s.ToString(), out byte result);
            return result;
        }

        /// <summary>
        /// 转换成Char
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static char ToChar(this object s)
        {
            if (s == null || s == DBNull.Value)
                return default;

            char.TryParse(s.ToString(), out char result);
            return result;
        }





        /// <summary>
        /// 安全转换为字符串，去除两端空格，当值为null时返回""
        /// </summary>
        /// <param name="input">输入值</param>
        public static string SafeString(this object input)
        {
            return input?.ToString().Trim() ?? string.Empty;
        }

        /// <summary>
        /// 转换为bool
        /// </summary>
        /// <param name="obj">数据</param>
        public static bool ToBool(this string obj)
        {
            return Convert.ToBool(obj);
        }
        /// <summary>
        /// 转换成Boolean
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ToBool(this object s)
        {
            if (s == null) return false;
            s = s.ToString().ToLower();
            if (s.Equals(1) || s.Equals("1") || s.Equals("true") || s.Equals("是") || s.Equals("yes"))
                return true;
            if (s.Equals(0) || s.Equals("0") || s.Equals("false") || s.Equals("否") || s.Equals("no"))
                return false;

            Boolean.TryParse(s.ToString(), out bool result);
            return result;
        }
        /// <summary>
        /// 转换为可空bool
        /// </summary>
        /// <param name="obj">数据</param>
        public static bool? ToBoolOrNull(this string obj)
        {
            return Convert.ToBoolOrNull(obj);
        }

        /// <summary>
        /// 转换成short/Int16
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static short ToShort(this object s)
        {
            if (s == null || s == DBNull.Value)
                return 0;

            short.TryParse(s.ToString(), out short result);
            return result;
        }

        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int ToInt(this string obj)
        {
            return Convert.ToInt(obj);
        }



        /// <summary>
        /// 转换成Int/Int32
        /// </summary>
        /// <param name="s"></param>
        /// <param name="round">是否四舍五入，默认false</param>
        /// <returns></returns>
        public static int ToInt(this object s, bool round = false)
        {
            if (s == null || s == DBNull.Value)
                return 0;

            if (s.GetType().IsEnum)
            {
                return (int)s;
            }

            if (s is bool b)
                return b ? 1 : 0;

            if (int.TryParse(s.ToString(), out int result))
                return result;

            var f = s.ToFloat();
            return round ? System.Convert.ToInt32(f) : (int)f;
        }


        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int? ToIntOrNull(this string obj)
        {
            return Convert.ToIntOrNull(obj);
        }

        /// <summary>
        /// 转换为long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long ToLong(this string obj)
        {
            return Convert.ToLong(obj);
        }
        /// <summary>
        /// 转换成Long/Int64
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ToLong(this object s)
        {
            if (s == null || s == DBNull.Value)
                return 0L;

            long.TryParse(s.ToString(), out long result);
            return result;
        }
        /// <summary>
        /// 转换为可空long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long? ToLongOrNull(this string obj)
        {
            return Convert.ToLongOrNull(obj);
        }

        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double ToDouble(this string obj)
        {
            return Convert.ToDouble(obj);
        }
        /// <summary>
        /// 转换成Float/Single
        /// </summary>
        /// <param name="s"></param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static float ToFloat(this object s, int? decimals = null)
        {
            if (s == null || s == DBNull.Value)
                return 0f;

            float.TryParse(s.ToString(), out float result);

            if (decimals == null)
                return result;

            return (float)Math.Round(result, decimals.Value);
        }

        /// <summary>
        /// 转换成Double/Single
        /// </summary>
        /// <param name="s"></param>
        /// <param name="digits">小数位数</param>
        /// <returns></returns>
        public static double ToDouble(this object s, int? digits = null)
        {
            if (s == null || s == DBNull.Value)
                return 0d;

            double.TryParse(s.ToString(), out double result);

            if (digits == null)
                return result;

            return Math.Round(result, digits.Value);
        }



        /// <summary>
        /// 转换为可空double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double? ToDoubleOrNull(this string obj)
        {
            return Convert.ToDoubleOrNull(obj);
        }

        /// <summary>
        /// 转换为decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal ToDecimal(this string obj)
        {
            return Convert.ToDecimal(obj);
        }
        /// <summary>
        /// 转换成Decimal
        /// </summary>
        /// <param name="s"></param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object s, int? decimals = null)
        {
            if (s == null || s == DBNull.Value) return 0m;

            decimal.TryParse(s.ToString(), out decimal result);

            if (decimals == null)
                return result;

            return Math.Round(result, decimals.Value);
        }

        /// <summary>
        /// 转换为可空decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal? ToDecimalOrNull(this string obj)
        {
            return Convert.ToDecimalOrNull(obj);
        }

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime ToDate(this string obj)
        {
            return Convert.ToDate(obj);
        }
        /// <summary>
        /// 转换成DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object s)
        {
            if (s == null || s == DBNull.Value)
                return DateTime.MinValue;

            DateTime.TryParse(s.ToString(), out DateTime result);
            return result;
        }

        /// <summary>
        /// 转换成Date
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDate(this object s)
        {
            return s.ToDateTime().Date;
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime? ToDateOrNull(this string obj)
        {
            return Convert.ToDateOrNull(obj);
        }

        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid ToGuid(this string obj)
        {
            return Convert.ToGuid(obj);
        }

        /// <summary>
        /// 转换为可空Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid? ToGuidOrNull(this string obj)
        {
            return Convert.ToGuidOrNull(obj);
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="obj">数据,范例: "83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A"</param>
        public static List<Guid> ToGuidList(this string obj)
        {
            return Convert.ToGuidList(obj);
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="obj">字符串集合</param>
        public static List<Guid> ToGuidList(this IList<string> obj)
        {
            if (obj == null)
                return new List<Guid>();
            return obj.Select(t => t.ToGuid()).ToList();
        }

        /// <summary>
        /// 泛型转换，转换失败会抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T To<T>(this object s)
        {
            return (T)System.Convert.ChangeType(s, typeof(T));
        }


    }
}
