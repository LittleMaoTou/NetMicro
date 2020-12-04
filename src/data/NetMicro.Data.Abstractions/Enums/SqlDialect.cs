using System.ComponentModel;

namespace NetMicro.Data.Abstractions.Enums
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum SqlDialect
    {
        /// <summary>
        /// SqlServer
        /// </summary>
        [Description("SqlServer")]
        SqlServer,
        /// <summary>
        /// MySql
        /// </summary>
        [Description("MySql")]
        MySql,
        /// <summary>
        /// SQLite
        /// </summary>
        [Description("SQLite")]
        SQLite


    }
}
