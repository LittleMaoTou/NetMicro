using NLog.LayoutRenderers;

namespace NetMicro.NLog.Provider.Layout
{
    public class LayoutExtentions
    {
        public static void ReisterNlogLayout()
        {
            LayoutRenderer.Register<RequestIdLayoutRenderer>("request_id");
            LayoutRenderer.Register<UserIdLayoutRenderer>("user_id");
            LayoutRenderer.Register<UserNameLayoutRenderer>("user");
        }
    }
}
