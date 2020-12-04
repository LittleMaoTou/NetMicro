using NetMicro.Core.Contexts;
using NLog;
using NLog.LayoutRenderers;
using System.Text;

namespace NetMicro.NLog.Provider.Layout
{
    [LayoutRenderer("request_id")]
    public class RequestIdLayoutRenderer : LayoutRenderer
    {
        internal string RequestID { get { return ApiContext.RequestId; } }
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(RequestID);
        }
    }
}
