using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetMicro.NLog.Provider.Layout;
using NLog.Web;

namespace NetMicro.NLog
{
    public static class ServiceCollectionExtensions
    {

        public static ILoggingBuilder AddFileLog(this ILoggingBuilder builder, string nlogConfigFile = "nlog.config")
        {
            LayoutExtentions.ReisterNlogLayout();
            builder.AddNLog(nlogConfigFile);
            return builder;
        }
        public static IHostBuilder UseFileLog(this IHostBuilder builder)
        {
            LayoutExtentions.ReisterNlogLayout();
            builder.UseNLog();
            return builder;
        }
    }
}
