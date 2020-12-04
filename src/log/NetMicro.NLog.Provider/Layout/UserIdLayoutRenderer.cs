using NetMicro.Core.Contexts;
using NLog;
using NLog.LayoutRenderers;
using System.Text;

namespace NetMicro.NLog.Provider.Layout
{
    [LayoutRenderer("user_id")]
    public class UserIdLayoutRenderer : LayoutRenderer
    {
        internal object User_Id { get { return ApiContext.UserId; } }
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(User_Id);
        }
    }
}
