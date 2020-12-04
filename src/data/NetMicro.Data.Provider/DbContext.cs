using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using NetMicro.Core.Exceptions;
using NetMicro.Core.Extensions;
using NetMicro.Data.Abstractions;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.Options;
using NetMicro.Data.Provider.DbProvider;
using NetMicro.Data.Provider.Entities;
using IsolationLevel = System.Data.IsolationLevel;

namespace NetMicro.Data.Provider
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public abstract class DbContext : IDbContext
    {
        #region ==属性==

        public ISqlAdapter SqlAdapter { get; private set; }

        public ILoggerFactory LoggerFactory { get; private set; }

        public DbOptions DbOptions { get; }

        #endregion

        #region ==构造函数==

        public DbContext(DbOptions options, ILoggerFactory loggerFactory)
        {
            if (options == null)
                throw new DataAccessException("数据库配置不可为空");
            DbOptions = options;
            LoggerFactory = loggerFactory;
            if (SqlAdapter == null)
                SqlAdapter = DbOptions.SetDbSqlAdapter();
            InitializeSets();
        }
        #endregion

        #region ==方法==

        /// <summary>
        /// 创建新的连接
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public IDbConnection Connection(IDbTransaction transaction = null)
        {
            if (transaction != null)
                return transaction.Connection;
            return DbOptions.SetConnection();
        }

        public IUnitOfWork UnitOfWork()
        {
            //SQLite数据库开启事务时会报 database is locked 错误
            if (SqlAdapter.SqlDialect == Abstractions.Enums.SqlDialect.SQLite)
                return new UnitOfWork(null);

            var con = Connection();
            con.Open();
            return new UnitOfWork(con.BeginTransaction());
        }

        public IUnitOfWork UnitOfWork(IsolationLevel isolationLevel)
        {
            //SQLite数据库开启事务时会报 database is locked 错误
            if (SqlAdapter.SqlDialect == Abstractions.Enums.SqlDialect.SQLite)
                return new UnitOfWork(null);

            var con = Connection();
            con.Open();
            return new UnitOfWork(con.BeginTransaction(isolationLevel));
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : IEntity, new()
        {
            var properties = Properties();
            if (properties == null || !properties.Any())
                throw new NullReferenceException("未找到指定的实体数据集");
            var propertyInfo = properties.Where(m => m.PropertyType.GenericTypeArguments.Single() == typeof(TEntity)).FirstOrDefault();
            if (propertyInfo == null)
                throw new NullReferenceException("未找到指定的实体数据集");

            return (IDbSet<TEntity>)propertyInfo.GetValue(this);

        }

        public void CreateDatabase(List<IEntityDescriptor> entityDescriptors, out bool databaseExists)
        {
            SqlAdapter.CreateDatabase(entityDescriptors, out databaseExists);
        }

        #endregion

        #region ==私有方法==

        /// <summary>
        /// 初始化IDbSet
        /// </summary>
        private void InitializeSets()
        {
            var properties = Properties();
            if (properties == null || !properties.Any())
                return;
            foreach (var propertyInfo in properties)
            {
                var entityType = propertyInfo.PropertyType.GenericTypeArguments.Single();
                if (!EntityDescriptorCollection.Exisit(entityType))
                    EntityDescriptorCollection.Add(new EntityDescriptor(this, entityType));
                var dbSetType = typeof(DbSet<>).MakeGenericType(entityType);
                var dbSet = Activator.CreateInstance(dbSetType, this, SqlAdapter, LoggerFactory);
                propertyInfo.SetValue(this, dbSet);
            }
        }


        private IEnumerable<PropertyInfo> Properties()
        {
            return GetType().GetRuntimeProperties()
               .Where(p => !p.IsStatic()
                           && !p.GetIndexParameters().Any()
                           && p.PropertyType.GetTypeInfo().IsGenericType
                           && p.PropertyType.GetGenericTypeDefinition() == typeof(IDbSet<>));
        }


        #endregion
    }
}
