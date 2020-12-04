using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NetMicro.Core;
using NetMicro.Core.Extensions;
using NetMicro.Data.Abstractions;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.Enums;
using NetMicro.Data.Abstractions.SqlQueryable;
using NetMicro.Data.Provider.Entities.Extend;
using NetMicro.Data.Provider.SqlQueryable;
using NetMicro.Core.Helper;

namespace NetMicro.Data.Provider
{
    internal class DbSet<TEntity> : IDbSet<TEntity> where TEntity : IEntity, new()
    {
        #region ==属性==

        public ISqlAdapter SqlAdapter { get; }

        public ILoggerFactory LoggerFactory { get; }
        public IDbContext DbContext { get; }

        public IEntityDescriptor EntityDescriptor { get; }

        #endregion

        #region ==字段==

        private readonly ISqlAdapter _sqlAdapter;

        private readonly IEntitySqlBuilder _sql;

        private readonly ILogger _logger;

        #endregion

        #region ==构造函数==

        public DbSet(IDbContext context, ISqlAdapter adapter, ILoggerFactory loggerFactory)
        {
            DbContext = context;
            SqlAdapter = adapter;
            LoggerFactory = loggerFactory;
            EntityDescriptor = EntityDescriptorCollection.Get<TEntity>();
            _sqlAdapter = adapter;
            _sql = EntityDescriptor.Sql;
            _logger = loggerFactory?.CreateLogger("DbSet-" + EntityDescriptor.TableName);
        }

        #endregion

        #region ==Insert==

        public bool Insert(TEntity entity, IUnitOfWork uow = null, string tableName = null)
        {
            Check.NotNull(entity, nameof(entity));

            SetCreatedBy(entity);

            var sql = _sql.Insert(tableName);
            if (EntityDescriptor.PrimaryKey.IsIdentity)
            {
                if (EntityDescriptor.PrimaryKey.IsInt())
                {
                    //自增主键
                    sql += _sqlAdapter.IdentitySql;
                    var id = ExecuteScalar<int>(sql, entity, uow);
                    if (id > 0)
                    {
                        EntityDescriptor.PrimaryKey.PropertyInfo.SetValue(entity, id);
                        _logger?.LogDebug($"Insert:({sql}),NewID({id})");

                        return true;
                    }

                    return false;
                }

                if (EntityDescriptor.PrimaryKey.IsLong())
                {
                    //自增主键
                    sql += _sqlAdapter.IdentitySql;
                    var id = ExecuteScalar<long>(sql, entity, uow);
                    if (id > 0)
                    {
                        EntityDescriptor.PrimaryKey.PropertyInfo.SetValue(entity, id);
                        _logger?.LogDebug($"Insert:({sql}),NewID({id})");

                        return true;
                    }
                    return false;
                }

            }

            _logger?.LogDebug($"Insert:({sql})");
            return Execute(sql, entity, uow) > 0;

        }

        public async Task<bool> InsertAsync(TEntity entity, IUnitOfWork uow = null, string tableName = null)
        {
            Check.NotNull(entity, nameof(entity));
            SetCreatedBy(entity);

            var sql = _sql.Insert(tableName);
            if (EntityDescriptor.PrimaryKey.IsIdentity)
            {
                if (EntityDescriptor.PrimaryKey.IsInt())
                {
                    sql += _sqlAdapter.IdentitySql;

                    var id = await ExecuteScalarAsync<int>(sql, entity, uow);
                    if (id > 0)
                    {
                        EntityDescriptor.PrimaryKey.PropertyInfo.SetValue(entity, id);
                        _logger?.LogDebug($"Insert:({sql}),NewID({id})");

                        return true;
                    }

                    return false;
                }
                if (EntityDescriptor.PrimaryKey.IsLong())
                {
                    sql += _sqlAdapter.IdentitySql;
                    var id = await ExecuteScalarAsync<long>(sql, entity, uow);
                    if (id > 0)
                    {
                        EntityDescriptor.PrimaryKey.PropertyInfo.SetValue(entity, id);
                        _logger?.LogDebug($"Insert:({sql}),NewID({id})");

                        return true;
                    }
                    return false;
                }

            }
            _logger?.LogDebug($"Insert:({sql})");
            return await ExecuteAsync(sql, entity, uow) > 0;

        }

        #endregion

        #region ==BatchInsert==

        public bool BatchInsert(List<TEntity> entityList, int flushSize = 10000, IUnitOfWork uow = null, string tableName = null)
        {
            if (entityList == null || !entityList.Any())
                return false;

            //判断有没有事务
            var hasTran = true;
            if (uow == null)
            {
                uow = DbContext.UnitOfWork();
                hasTran = false;
            }

            try
            {

                if (_sqlAdapter.SqlDialect == SqlDialect.SQLite)
                {
                    #region ==SQLite使用Dapper的官方方法==

                    entityList.ForEach(entity =>
                    {
                        SetCreatedBy(entity);

                    });
                    Execute(_sql.Insert(tableName), entityList);

                    #endregion
                }
                else
                {
                    #region ==自定义==
                    var batchInsertColumnList = EntityDescriptor.Columns.Where(m => m.IsIdentity == false).ToList();
                    var sqlBuilder = new StringBuilder();

                    for (var t = 0; t < entityList.Count; t++)
                    {
                        var mod = (t + 1) % flushSize;
                        if (mod == 1)
                        {
                            sqlBuilder.Clear();
                            sqlBuilder.Append(_sql.BatchInsert(tableName));
                        }
                        var entity = entityList[t];
                        SetCreatedBy(entity);

                        sqlBuilder.Append("(");

                        for (var i = 0; i < batchInsertColumnList.Count; i++)
                        {
                            var col = batchInsertColumnList[i];
                            var value = col.PropertyInfo.GetValue(entity);
                            var type = col.PropertyInfo.PropertyType;
                            AppendValue(sqlBuilder, type, value);
                            if (i < batchInsertColumnList.Count - 1)
                                sqlBuilder.Append(",");
                        }
                        sqlBuilder.Append(")");
                        if (mod > 0 && t < entityList.Count - 1)
                            sqlBuilder.Append(",");
                        else if (mod == 0 || t == entityList.Count - 1)
                        {
                            sqlBuilder.Append(";");
                            Execute(sqlBuilder.ToString(), uow: uow);
                        }
                    }

                    #endregion
                }

                if (!hasTran)
                    uow.Commit();

                return true;
            }
            catch
            {
                if (!hasTran)
                    uow.Rollback();

                throw;
            }
        }

        public async Task<bool> BatchInsertAsync(List<TEntity> entityList, int flushSize = 10000, IUnitOfWork uow = null, string tableName = null)
        {
            if (entityList == null || !entityList.Any())
                return false;

            //判断有没有事务
            var hasTran = true;
            if (uow == null)
            {
                uow = DbContext.UnitOfWork();
                hasTran = false;
            }

            try
            {
                if (_sqlAdapter.SqlDialect == SqlDialect.SQLite)
                {
                    #region ==SQLite使用Dapper的官方方法==

                    entityList.ForEach(entity =>
                    {
                        SetCreatedBy(entity);

                    });

                    await ExecuteAsync(_sql.Insert(tableName), entityList);

                    #endregion
                }
                else
                {
                    #region ==自定义方法==
                    var batchInsertColumnList = EntityDescriptor.Columns.Where(m => m.IsIdentity == false).ToList();
                    var sqlBuilder = new StringBuilder();

                    for (var t = 0; t < entityList.Count; t++)
                    {
                        var mod = (t + 1) % flushSize;
                        if (mod == 1)
                        {
                            sqlBuilder.Clear();
                            sqlBuilder.Append(_sql.BatchInsert(tableName));
                        }

                        var entity = entityList[t];

                        SetCreatedBy(entity);

                        sqlBuilder.Append("(");
                        for (var i = 0; i < batchInsertColumnList.Count; i++)
                        {
                            var col = batchInsertColumnList[i];
                            var value = col.PropertyInfo.GetValue(entity);
                            var type = col.PropertyInfo.PropertyType;

                            AppendValue(sqlBuilder, type, value);

                            if (i < batchInsertColumnList.Count - 1)
                                sqlBuilder.Append(",");
                        }

                        sqlBuilder.Append(")");

                        if (mod > 0 && t < entityList.Count - 1)
                            sqlBuilder.Append(",");
                        else if (mod == 0 || t == entityList.Count - 1)
                        {
                            sqlBuilder.Append(";");
                            await ExecuteAsync(sqlBuilder.ToString(), uow: uow);
                        }
                    }

                    #endregion
                }

                if (!hasTran)
                    uow.Commit();

                return true;
            }
            catch
            {
                if (!hasTran)
                    uow.Rollback();

                throw;
            }
        }

        #endregion

        #region ==Delete==

        private DynamicParameters GetDeleteParameters(dynamic id)
        {
            PrimaryKeyValidate(id);

            var dynParams = new DynamicParameters();
            dynParams.Add(_sqlAdapter.AppendParameter(EntityDescriptor.PrimaryKey.Name), id);
            return dynParams;
        }

        public bool Delete(dynamic id, IUnitOfWork uow = null, string tableName = null)
        {
            var dynParams = GetDeleteParameters(id);
            var sql = _sql.DeleteSingle(tableName);
            _logger?.LogDebug($"Delete:{sql}");
            return Execute(sql, dynParams, uow) > 0;
        }

        public async Task<bool> DeleteAsync(dynamic id, IUnitOfWork uow = null, string tableName = null)
        {
            var dynParams = GetDeleteParameters(id);
            var sql = _sql.DeleteSingle(tableName);
            _logger?.LogDebug($"DeleteAsync:{sql}");
            return await ExecuteAsync(sql, dynParams, uow) > 0;
        }

        #endregion

        #region ==SoftDelete==

        private DynamicParameters GetSoftDeleteParameters(dynamic id)
        {
            PrimaryKeyValidate(id);
            var dynParams = new DynamicParameters();
            dynParams.Add(_sqlAdapter.AppendParameter(EntityDescriptor.PrimaryKey.Name), id);
            dynParams.Add(_sqlAdapter.AppendParameter("DeletedTime"), DateTime.Now);
            var deleteBy = EntityDescriptor.PrimaryKey.Type.GetUserId();
            dynParams.Add(_sqlAdapter.AppendParameter("DeletedBy"), deleteBy);

            return dynParams;
        }

        public bool SoftDelete(dynamic id, IUnitOfWork uow = null, string tableName = null)
        {
            if (!EntityDescriptor.SoftDelete)
                throw new Exception("该实体未继承软删除实体，无法使用软删除功能~");

            var dynParams = GetSoftDeleteParameters(id);
            var sql = _sql.SoftDeleteSingle(tableName);
            _logger?.LogDebug($"SoftDelete:{sql}");
            return Execute(sql, dynParams, uow) > 0;
        }

        public async Task<bool> SoftDeleteAsync(dynamic id, IUnitOfWork uow = null, string tableName = null)
        {
            if (!EntityDescriptor.SoftDelete)
                throw new Exception("该实体未继承软删除实体，无法使用软删除功能~");

            var dynParams = GetSoftDeleteParameters(id);
            var sql = _sql.SoftDeleteSingle(tableName);
            _logger?.LogDebug($"SoftDeleteAsync:{sql}");
            return await ExecuteAsync(sql, dynParams, uow) > 0;
        }

        #endregion

        #region ==Update==

        private void UpdateCheck(TEntity entity)
        {
            Check.NotNull(entity, nameof(entity));

            if (EntityDescriptor.PrimaryKey.IsNo())
                throw new ArgumentException("没有主键的实体对象无法使用该方法", nameof(entity));

            SetModifiedBy(entity);
        }

        public bool Update(TEntity entity, IUnitOfWork uow = null, string tableName = null)
        {
            UpdateCheck(entity);
            var sql = _sql.UpdateSingle(tableName);
            _logger?.LogDebug($"Update:{sql}");
            return Execute(sql, entity, uow) > 0;
        }



        public async Task<bool> UpdateAsync(TEntity entity, IUnitOfWork uow = null, string tableName = null)
        {
            UpdateCheck(entity);
            var sql = _sql.UpdateSingle(tableName);
            _logger?.LogDebug($"UpdateAsync:{sql}");
            return await ExecuteAsync(sql, entity, uow) > 0;
        }

        #endregion

        #region ==Get==

        private DynamicParameters GetParameters(dynamic id)
        {
            PrimaryKeyValidate(id);

            var dynParams = new DynamicParameters();
            dynParams.Add(EntityDescriptor.PrimaryKey.Name, id);
            return dynParams;
        }

        public TEntity Get(dynamic id, IUnitOfWork uow = null, string tableName = null, bool rowLock = false, bool noLock = false)
        {
            var dynParams = GetParameters(id);
            string sql;
            if (rowLock)
                sql = _sql.GetAndRowLock(tableName);
            else if (_sqlAdapter.SqlDialect == SqlDialect.SqlServer && noLock)
                sql = _sql.GetAndNoLock(tableName);
            else
                sql = _sql.Get(tableName);
            _logger?.LogDebug($"Get:{sql}");
            return QuerySingleOrDefault<TEntity>(sql, dynParams, uow);
        }

        public Task<TEntity> GetAsync(dynamic id, IUnitOfWork uow = null, string tableName = null, bool rowLock = false, bool noLock = false)
        {
            var dynParams = GetParameters(id);
            string sql;
            if (rowLock)
                sql = _sql.GetAndRowLock(tableName);
            else if (_sqlAdapter.SqlDialect == SqlDialect.SqlServer && noLock)
                sql = _sql.GetAndNoLock(tableName);
            else
                sql = _sql.Get(tableName);

            _logger?.LogDebug($"GetAsync:{sql}");
            return QuerySingleOrDefaultAsync<TEntity>(sql, dynParams, uow);
        }

        #endregion

        #region ==Exists==

        public bool Exists(dynamic id, IUnitOfWork uow = null, string tableName = null, bool noLock = false)
        {
            //没有主键的表无法使用Exists方法
            if (EntityDescriptor.PrimaryKey.IsNo())
                throw new ArgumentException("该实体没有主键，无法使用Exists方法~");

            var dynParams = GetParameters(id);
            var sql = _sql.Exists(tableName);

            _logger?.LogDebug($"Exists:{sql}");

            return QuerySingleOrDefault<int>(sql, dynParams, uow) > 0;
        }

        public async Task<bool> ExistsAsync(dynamic id, IUnitOfWork uow = null, string tableName = null, bool noLock = false)
        {
            //没有主键的表无法使用Exists方法
            if (EntityDescriptor.PrimaryKey.IsNo())
                throw new ArgumentException("该实体没有主键，无法使用Exists方法~");

            var dynParams = GetParameters(id);
            var sql = _sql.Exists(tableName);

            _logger?.LogDebug($"ExistsAsync:{sql}");
            return (await QuerySingleOrDefaultAsync<int>(sql, dynParams, uow)) > 0;
        }

        #endregion

        #region ==Execute==

        public int Execute(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).Execute(sql, param, tran, commandType: commandType);
        }

        public Task<int> ExecuteAsync(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).ExecuteAsync(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==ExecuteScalar==

        public T ExecuteScalar<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).ExecuteScalar<T>(sql, param, tran, commandType: commandType);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).ExecuteScalarAsync<T>(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==ExecuteReader==

        public IDataReader ExecuteReader(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).ExecuteReader(sql, param, tran, commandType: commandType);
        }

        public Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).ExecuteReaderAsync(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==QueryFirstOrDefault==

        public dynamic QueryFirstOrDefault(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QueryFirstOrDefault(sql, param, tran, commandType: commandType);
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QueryFirstOrDefault<T>(sql, param, tran, commandType: commandType);
        }

        public Task<dynamic> QueryFirstOrDefaultAsync(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QueryFirstOrDefaultAsync(sql, param, tran, commandType: commandType);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QueryFirstOrDefaultAsync<T>(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==QuerySingleOrDefault==

        public dynamic QuerySingleOrDefault(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QuerySingleOrDefault(sql, param, tran, commandType: commandType);
        }

        public T QuerySingleOrDefault<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QuerySingleOrDefault<T>(sql, param, tran, commandType: commandType);
        }

        public Task<dynamic> QuerySingleOrDefaultAsync(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QuerySingleOrDefaultAsync(sql, param, tran, commandType: commandType);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QuerySingleOrDefaultAsync<T>(sql, param, tran, commandType: commandType);
        }


        #endregion

        #region ==QueryMultipleAsync==

        public SqlMapper.GridReader QueryMultiple(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QueryMultiple(sql, param, tran, commandType: commandType);
        }

        public Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QueryMultipleAsync(sql, param, tran, commandType: commandType);
        }

        #endregion

        #region ==Query==

        public IEnumerable<dynamic> Query(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).Query(sql, param, tran, commandType: commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).Query<T>(sql, param, tran, commandType: commandType);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QueryAsync(sql, param, tran, commandType: commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null)
        {
            var tran = GetTransaction(uow);
            return DbContext.Connection(tran).QueryAsync<T>(sql, param, tran, commandType: commandType);
        }

        public INetSqlQueryable<TEntity> Find(bool noLock = true)
        {
            return new NetSqlQueryable<TEntity>(this, null, null, noLock);
        }

        public INetSqlQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, bool noLock = true)
        {
            return new NetSqlQueryable<TEntity>(this, expression, null, noLock);
        }

        public INetSqlQueryable<TEntity> Find(string tableName, bool noLock = true)
        {
            return new NetSqlQueryable<TEntity>(this, null, tableName, noLock);
        }

        public INetSqlQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, string tableName, bool noLock = true)
        {
            return new NetSqlQueryable<TEntity>(this, expression, tableName, noLock);
        }

        #endregion



        #region ==Clear==

        public bool Clear(IUnitOfWork uow = null, string tableName = null)
        {
            var sql = _sql.Clear(tableName);
            _logger?.LogDebug($"Clear:{sql}");
            Execute(sql, uow: uow);
            return true;
        }

        public async Task<bool> ClearAsync(IUnitOfWork uow = null, string tableName = null)
        {
            var sql = _sql.Clear(tableName);
            _logger?.LogDebug($"ClearAsync:{sql}");
            await ExecuteAsync(sql, uow: uow);
            return true;
        }

        #endregion

        #region ==私有方法==

        /// <summary>
        /// 主键验证
        /// </summary>
        /// <param name="id"></param>
        private void PrimaryKeyValidate(dynamic id)
        {
            //没有主键的表无法删除单条记录
            if (EntityDescriptor.PrimaryKey.IsNo())
                throw new ArgumentException("该实体没有主键，无法使用该方法~");

            //验证id有效性
            if (EntityDescriptor.PrimaryKey.IsInt() || EntityDescriptor.PrimaryKey.IsLong())
            {
                if (id < 1)
                    throw new ArgumentException("主键不能小于1~");
            }
            else
            {
                if (id == null)
                    throw new ArgumentException("主键不能为空~");
            }
        }

        /// <summary>
        /// 附加值
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        private void AppendValue(StringBuilder sqlBuilder, Type type, object value)
        {
            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (value == null)
            {
                sqlBuilder.AppendFormat("NULL");
            }
            else if (type.IsEnum)
            {
                sqlBuilder.AppendFormat("{0}", value.ToInt());
            }
            else if (type.IsBool())
            {
                sqlBuilder.AppendFormat("{0}", value.ToInt());
            }
            else if (type.IsDateTime())
            {
                sqlBuilder.AppendFormat("'{0:yyyy-MM-dd HH:mm:ss}'", value);
            }
            else
            {
                sqlBuilder.AppendFormat("'{0}'", value);
            }
        }

        /// <summary>
        /// 设置添加人以及修改人
        /// </summary>
        /// <param name="entity"></param>
        private void SetCreatedBy(TEntity entity)
        {

            if (EntityDescriptor.IsEntityBase)
            {
                int i = 0;
                foreach (var column in EntityDescriptor.Columns)
                {
                    var name = column.PropertyInfo.Name;
                    if (name.Equals("CreatedBy") || name.Equals("ModifiedBy"))
                    {

                        var createdBy = EntityDescriptor.PrimaryKey.Type.GetUserId();
                        column.PropertyInfo.SetValue(entity, createdBy);
                        i++;

                    }

                    if (i > 1)
                        break;
                }
            }
        }

        /// <summary>
        /// 设置修改人
        /// </summary>
        /// <param name="entity"></param>
        private void SetModifiedBy(TEntity entity)
        {
            if (EntityDescriptor.IsEntityBase)
            {
                int i = 0;
                foreach (var column in EntityDescriptor.Columns)
                {
                    var name = column.PropertyInfo.Name;
                    if (name.Equals("ModifiedBy"))
                    {
                        var modifiedBy = column.PropertyInfo.GetValue(entity);
                        var accountId = EntityDescriptor.PrimaryKey.Type.GetUserId();
                        if (modifiedBy == default || modifiedBy != accountId)
                        {
                            column.PropertyInfo.SetValue(entity, accountId);
                            i++;
                        }
                    }

                    if (name.Equals("ModifiedTime"))
                    {
                        column.PropertyInfo.SetValue(entity, DateTime.Now);
                        i++;
                    }

                    if (i > 1)
                        break;
                }
            }
        }

        /// <summary>
        /// 获取事务
        /// </summary>
        /// <param name="uow"></param>
        /// <returns></returns>
        private IDbTransaction GetTransaction(IUnitOfWork uow)
        {
            return uow?.Transaction;
        }

        #endregion
    }
}
