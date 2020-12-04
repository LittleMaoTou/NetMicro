using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.Options;

namespace NetMicro.Data.Abstractions
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        DbOptions DbOptions { get; }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="entityDescriptors"></param>
        /// <param name="databaseExists"></param>
        void CreateDatabase(List<IEntityDescriptor> entityDescriptors, out bool databaseExists);
        /// <summary>
        /// sql适配器
        /// </summary>
        ISqlAdapter SqlAdapter { get; }

        /// <summary>
        /// 日志
        /// </summary>
        ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// 创建新的连接
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        IDbConnection Connection(IDbTransaction transaction = null);

        /// <summary>
        /// 创建新的工作单元
        /// </summary>
        /// <returns></returns>
        IUnitOfWork UnitOfWork();

        /// <summary>
        /// 创建新的工作单元
        /// </summary>
        /// <returns></returns>
        IUnitOfWork UnitOfWork(IsolationLevel isolationLevel);

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IDbSet<TEntity> Set<TEntity>() where TEntity : IEntity, new();

    }


}
