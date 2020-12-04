using NetMicro.Data.Abstractions.Enums;

namespace NetMicro.Data.Abstractions.Options
{
    /// <summary>
    /// 数据库配置项
    /// </summary>
    public class DbOptions
    {

        /// <summary>
        /// 数据库类型
        /// </summary>
        public SqlDialect Dialect { get; set; }


        /// <summary>
        /// 表前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 数据库版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库模式
        /// </summary>
        public string SchemaName { get; set; }


    }
}
