using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetMicro.Core.Ioc
{
    public static class ServiceLocator
    {
        internal static IServiceProvider Current { get; set; }
        /// <summary>
        /// 创建类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>()
        {
            if (Current == null)
                throw new ArgumentNullException(nameof(Current), "调用此方法时必须先调用BuildServiceProvider！");
            return Current.GetService<T>();
        }
        /// <summary>
        /// 创建类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> CreateList<T>()
        {
            if (Current == null)
                throw new ArgumentNullException(nameof(Current), "调用此方法时必须先调用BuildServiceProvider！");
            return Current.GetServices<T>().ToList();
        }

        /// <summary>
        /// 创建作用域
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IServiceScope CreateScope()
        {
            if (Current == null)
                throw new ArgumentNullException(nameof(Current), "调用此方法时必须先调用BuildServiceProvider！");
            return Create<IServiceScopeFactory>().CreateScope();
        }


    }
}
