using NetMicro.Core.Contexts;
using NLog;
using NLog.LayoutRenderers;
using System.Text;

namespace NetMicro.NLog.Provider.Layout
{
    [LayoutRenderer("user")]
    public class UserNameLayoutRenderer : LayoutRenderer
    {
        internal string User { get { return ApiContext.UserName; } }
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(User);
        }
    }
}
