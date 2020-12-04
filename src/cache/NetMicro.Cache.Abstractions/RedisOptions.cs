using Microsoft.Extensions.Options;
using System;
using NetMicro.Core.Ioc;

namespace NetMicro.Cache.Provider.StackExchange
{
    public class RedisOptions
    {
        public string Connection { get; set; }
    }

    public class RedisInstance
    {
        public static RedisOptions Options
        {
            get
            {
                var options = ServiceLocator.Create<IOptions<RedisOptions>>();
                if (options == null)
                    throw new NullReferenceException("redis配置未注册");
                return options.Value;
            }
        }
    }
}
