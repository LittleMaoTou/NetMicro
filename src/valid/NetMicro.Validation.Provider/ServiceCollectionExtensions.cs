using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetMicro.Web.Provider.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMicro.Validation.Provider
{
    /// <summary>
    /// 请求参数验证
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 请求参数自动验证自定义输出
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRequestValid(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    //获取验证失败的模型字段 
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();
                    var str = string.Join("|", errors);
                    return new BadRequestObjectResult(ApiResult.Error(400, str));
                };
            });
            return services;
        }

        /// <summary>
        /// 请求参数自动验证自定义输出
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRequestValid(this IServiceCollection services, Action<ApiBehaviorOptions> configureOptions)
        {
            services.Configure(configureOptions);
            return services;
        }
    }
}
