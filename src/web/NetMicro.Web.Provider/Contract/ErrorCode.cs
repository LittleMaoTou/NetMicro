using System.ComponentModel;

namespace NetMicro.Web.Provider.Contract
{
    /// <summary>
    /// 基础错误代码
    /// </summary>
    public enum ErrorCode
    {
        [Description("成功")]
        Success = 200,
        [Description("失败")]
        Error = 2000,
        [Description("未找到对应结果")]
        NoFind = 2001,
        [Description("本地错误")]
        LogicalError = 2002,
        [Description("拒绝访问")]
        DenyAccess = 2003,
        [Description("网络错误")]
        NetworkError = 2004,
        [Description("远端错误")]
        RemoteError = 2005,
        [Description("客户端应重新请求")]
        ReTry = 2006,
        [Description("服务不可用")]
        Unavailable = 2007,
        [Description("未知的RefreshToken")]
        RefreshToken_Unknow = 2008,
        [Description("未知的ClientKey")]
        ClientKey = 2009,
        [Description("未知的AccessToken")]
        AccessToken_Unknow = 2010,
        [Description("AccessToken过期")]
        AccessToken_Unavailable = 2011,
        [Description("账户不存在")]
        NotFindAcount = 2012,
        [Description("账户未激活")]
        AcountNotActivty = 2013,
        [Description("账户已禁用，请联系管理员")]
        AcountDisabled = 2014,
        [Description("账户锁定，不允许修改")]
        AcountLocked = 2015,
    }
}
