using System.Linq.Expressions;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.Enums;

namespace NetMicro.Data.Provider.SqlQueryable.Internal
{
    /// <summary>
    /// 查询表连接信息
    /// </summary>
    internal class QueryJoinDescriptor
    {
        /// <summary>
        /// 连接类型
        /// </summary>
        public JoinType Type { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 实体信息
        /// </summary>
        public IEntityDescriptor EntityDescriptor { get; set; }

        /// <summary>
        /// 连接条件
        /// </summary>
        public LambdaExpression On { get; set; }

        /// <summary>
        /// 自定义表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// SqlServer的NoLock特性
        /// </summary>
        public bool NoLock { get; set; }
    }
}
