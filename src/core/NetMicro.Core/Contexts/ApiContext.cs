using NetMicro.Core.Extensions;
using NetMicro.Core.Helper;
using NetMicro.Core.Ids;
using NetMicro.Core.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convert = NetMicro.Core.Helper.Convert;

namespace NetMicro.Core.Contexts
{
    /// <summary>
    /// 请求上下文
    /// </summary>
    public class ApiContext
    {
        /// <summary>
        /// 取请求ID的方法
        /// </summary>
        public static Func<string> GetRequestIdFunc;
        /// <summary>
        /// 取请求ID
        /// </summary>
        public static string RequestId => GetRequestIdFunc?.Invoke() ?? IdBuilder.Guid;
        /// <summary>
        /// 取得当前用户的方法
        /// </summary>
        public static Func<string> GetUserNameFunc;
        /// <summary>
        /// 取得当前用户
        /// </summary>
        public static string UserName => GetUserNameFunc?.Invoke() ?? Web.HttpContext?.User?.FindFirst(ClaimsName.UserName)?.Value;


        /// <summary>
        /// 取得当前用户Id的方法
        /// </summary>
        public static Func<long> GetUserIdFunc;

        private static long userId => GetUserIdFunc?.Invoke() ?? 0;
        /// <summary>
        /// 取得当前用户Id
        /// </summary>
        public static long UserId
        {
            get
            {

                if (userId > 0)
                    return userId;
                var accountId = Web.HttpContext?.User?.FindFirst(ClaimsName.UserId);
                if (accountId != null && accountId.Value.NotNull())
                {
                    long.TryParse(accountId.Value, out long Id);
                    return Id;
                }

                return 0;
            }
        }



        /// <summary>
        /// 取得当前用户Id
        /// </summary>
        public static TKey GetUserId<TKey>()
        {
            var accountId = Web.HttpContext?.User?.FindFirst(ClaimsName.UserId);
            if (accountId != null && accountId.Value.NotNull())
            {
                long.TryParse(accountId.Value, out long Id);
                return Convert.To<TKey>(accountId.Value);

            }
            return default;
        }




        /// <summary>
        /// User-Agent
        /// </summary>
        public static string UserAgent => Web.Browser;


        public AccountType AccountType
        {
            get
            {
                var ty = Web.HttpContext?.User?.FindFirst(ClaimsName.UserType);

                if (ty != null && ty.Value.NotNull())
                {
                    return (AccountType)ty.Value.ToInt();
                }

                return AccountType.UnKnown;
            }
        }

        /// <summary>
        /// 获取当前用户IPv4
        /// </summary>
        public string IPv4
        {
            get
            {
                if (Web.HttpContext?.Connection == null)
                    return "";

                return Web.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }

        /// <summary>
        /// 获取当前用户IPv6
        /// </summary>
        public string IPv6
        {
            get
            {
                if (Web.HttpContext?.Connection == null)
                    return "";

                return Web.HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString();
            }
        }
        /// <summary>
        /// 取得当前机器
        /// </summary>
        public static string MachineName => Web.Host;
        /// <summary>
        /// 取得当前Ip
        /// </summary>
        public static string IpName => Web.Ip;
        /// <summary>
        /// 取得当前Url
        /// </summary>
        public static string UrlName => Web.Url;
    }
}
