using System.Linq;
using System.Text;
using NetMicro.Core.Exceptions;
using NetMicro.Core.Extensions;
using NetMicro.Data.Abstractions;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.Enums;
using NetMicro.Data.Provider.Extensions;

namespace NetMicro.Data.Provider.Entities
{
    internal class EntitySqlBuilder : IEntitySqlBuilder
    {
        private readonly IEntityDescriptor _descriptor;
        private readonly IPrimaryKeyDescriptor _primaryKey;
        private readonly ISqlAdapter _adapter;

        public EntitySqlBuilder(IEntityDescriptor descriptor)
        {
            _descriptor = descriptor;
            _primaryKey = descriptor.PrimaryKey;
            _adapter = descriptor.SqlAdapter;

        }





        #region ==插入==


        /// <summary>
        /// 获取插入语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string Insert(string tableName)
        {
            var sb = new StringBuilder();
            sb.Append("INSERT INTO {0} ");
            sb.Append("(");

            var valuesSql = new StringBuilder();

            foreach (var col in _descriptor.Columns)
            {
                //排除自增列
                if (col.IsIdentity)
                    continue;

                _descriptor.SqlAdapter.AppendQuote(sb, col.Name);
                sb.Append(",");

                _descriptor.SqlAdapter.AppendParameter(valuesSql, col.PropertyInfo.Name);
                valuesSql.Append(",");
            }

            //删除最后一个","
            sb.Remove(sb.Length - 1, 1);

            sb.Append(") VALUES");

            sb.Append("(");
            //删除最后一个","
            if (valuesSql.Length > 0)
                valuesSql.Remove(valuesSql.Length - 1, 1);

            sb.Append(valuesSql);
            sb.Append(")");

            sb.Append(";");

            var insert = sb.ToString();

            if (tableName.NotNull())
            {
                return string.Format(insert, _adapter.GetTableName(tableName));
            }
            return string.Format(insert, _adapter.GetTableName(_descriptor.TableName));
        }

        #endregion

        #region ==批量插入==


        /// <summary>
        /// 获取批量插入语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string BatchInsert(string tableName)
        {
            var sb = new StringBuilder();
            sb.Append("INSERT INTO {0} ");
            sb.Append("(");

            var valuesSql = new StringBuilder();

            foreach (var col in _descriptor.Columns)
            {
                //排除自增列
                if (col.IsIdentity)
                    continue;

                _descriptor.SqlAdapter.AppendQuote(sb, col.Name);
                sb.Append(",");

                _descriptor.SqlAdapter.AppendParameter(valuesSql, col.PropertyInfo.Name);
                valuesSql.Append(",");


            }

            //删除最后一个","
            sb.Remove(sb.Length - 1, 1);

            sb.Append(") VALUES");

            var batchInsertSql = sb.ToString();
            if (tableName.NotNull())
                return string.Format(batchInsertSql, _adapter.GetTableName(tableName));
            return string.Format(batchInsertSql, _adapter.GetTableName(_descriptor.TableName));
        }

        #endregion

        #region ==删除单条==


        /// <summary>
        /// 获取单条删除语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string DeleteSingle(string tableName)
        {
            var deleteSql = "DELETE FROM {0} ";
            if (_primaryKey.IsNo())
                throw new DataAccessException("该实体无主键不可使用此方法");
            deleteSql = $"{deleteSql} WHERE {AppendQuote(_primaryKey.Name)}={AppendParameter(_primaryKey.PropertyInfo.Name)};";

            if (tableName.NotNull())
                return string.Format(deleteSql, _adapter.GetTableName(tableName));
            return string.Format(deleteSql, _adapter.GetTableName(_descriptor.TableName));
        }

        #endregion

        #region ==软删除单条==
        /// <summary>
        /// 获取软删除单条语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string SoftDeleteSingle(string tableName)
        {
            if (!_descriptor.SoftDelete)
            {
                return string.Empty;
            }
            var sb = new StringBuilder("UPDATE {0} SET ");
            sb.AppendFormat("{0}={1},", AppendQuote(_descriptor.GetDeletedColumnName()), "1");
            sb.AppendFormat("{0}={1},", AppendQuote(_descriptor.GetDeletedTimeColumnName()), AppendParameter("DeletedTime"));
            sb.AppendFormat("{0}={1} ", AppendQuote(_descriptor.GetDeletedByColumnName()), AppendParameter("DeletedBy"));
            sb.AppendFormat(" WHERE {0}={1};", AppendQuote(_primaryKey.Name), AppendParameter(_primaryKey.PropertyInfo.Name));
            var softDeleteSingleSql = sb.ToString();

            if (tableName.NotNull())
                return string.Format(softDeleteSingleSql, _adapter.GetTableName(tableName));
            return string.Format(softDeleteSingleSql, _adapter.GetTableName(_descriptor.TableName));
        }

        #endregion


        #region ==更新单个实体==


        /// <summary>
        /// 获取更新实体语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string UpdateSingle(string tableName)
        {
            if (_primaryKey.IsNo())
                throw new DataAccessException("该实体无主键，不可更新");
            var sb = new StringBuilder();
            sb.Append("UPDATE {0} SET ");
            var columns = _descriptor.Columns.Where(m => !m.IsPrimaryKey);
            foreach (var col in columns)
            {
                sb.AppendFormat("{0}={1}", AppendQuote(col.Name), AppendParameter(col.PropertyInfo.Name));
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendFormat(" WHERE {0}={1};", AppendQuote(_primaryKey.Name), AppendParameter(_primaryKey.PropertyInfo.Name));
            var updateSingleSql = sb.ToString();

            if (tableName.NotNull())
                return string.Format(updateSingleSql, _adapter.GetTableName(tableName));
            return string.Format(updateSingleSql, _adapter.GetTableName(_descriptor.TableName));
        }

        #endregion
        #region ==查询单个实体==


        /// <summary>
        /// 获取单个实体语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string Get(string tableName)
        {
            var sb = new StringBuilder("SELECT ");
            for (var i = 0; i < _descriptor.Columns.Count; i++)
            {
                var col = _descriptor.Columns[i];
                sb.AppendFormat("{0} AS {1}", AppendQuote(col.Name), AppendQuote(col.PropertyInfo.Name));

                if (i != _descriptor.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(" FROM {0} ");

            var querySql = sb.ToString();

            if (!_primaryKey.IsNo())
            {
                querySql += $" WHERE {AppendQuote(_primaryKey.Name)}={AppendParameter(_primaryKey.PropertyInfo.Name)} ";
                if (_descriptor.SoftDelete)
                {
                    var val = "0";
                    querySql += $" AND {AppendQuote(_descriptor.GetDeletedColumnName())}={val} ";
                }
            }

            if (tableName.NotNull())
                return string.Format(querySql, _adapter.GetTableName(tableName));
            return string.Format(querySql, _adapter.GetTableName(_descriptor.TableName));
        }

        #endregion

        #region ==查询单个实体带锁==



        /// <summary>
        /// 获取单个实体语句(行锁)
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetAndRowLock(string tableName)
        {
            var sb = new StringBuilder("SELECT ");
            for (var i = 0; i < _descriptor.Columns.Count; i++)
            {
                var col = _descriptor.Columns[i];
                sb.AppendFormat("{0} AS {1}", AppendQuote(col.Name), AppendQuote(col.PropertyInfo.Name));

                if (i != _descriptor.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(" FROM {0} ");

            var getAndRowLockSql = sb.ToString();

            // SqlServer行锁
            if (_descriptor.SqlAdapter.SqlDialect == SqlDialect.SqlServer)
            {
                getAndRowLockSql += " WITH (ROWLOCK, UPDLOCK) ";

            }

            if (!_primaryKey.IsNo())
            {

                getAndRowLockSql += $" WHERE {AppendQuote(_primaryKey.Name)}={AppendParameter(_primaryKey.PropertyInfo.Name)} ";


                if (_descriptor.SoftDelete)
                {
                    var val = "0";


                    getAndRowLockSql += $" AND {AppendQuote(_descriptor.GetDeletedColumnName())}={val} ";

                }

                //MySql行锁
                if (_descriptor.SqlAdapter.SqlDialect == SqlDialect.MySql)
                {
                    getAndRowLockSql += " FOR UPDATE;";
                }
            }

            if (tableName.NotNull())
                return string.Format(getAndRowLockSql, _adapter.GetTableName(tableName));
            return string.Format(getAndRowLockSql, _adapter.GetTableName(_descriptor.TableName));

        }

        #endregion

        #region ==查询单个实体无锁==



        /// <summary>
        /// 获取单个实体语句(行锁)
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetAndNoLock(string tableName)
        {
            var sb = new StringBuilder("SELECT ");
            for (var i = 0; i < _descriptor.Columns.Count; i++)
            {
                var col = _descriptor.Columns[i];
                sb.AppendFormat("{0} AS {1}", AppendQuote(col.Name), AppendQuote(col.PropertyInfo.Name));

                if (i != _descriptor.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(" FROM {0} ");

            var getAndNoLockSql = sb.ToString();

            // SqlServer行锁
            if (_descriptor.SqlAdapter.SqlDialect == SqlDialect.SqlServer)
            {
                getAndNoLockSql += "WITH (NOLOCK) ";
            }

            if (!_primaryKey.IsNo())
            {
                getAndNoLockSql += $" WHERE {AppendQuote(_primaryKey.Name)}={AppendParameter(_primaryKey.PropertyInfo.Name)} ";
                if (_descriptor.SoftDelete)
                {
                    var val = "0";
                    getAndNoLockSql += $" AND {AppendQuote(_descriptor.GetDeletedColumnName())}={val} ";
                }
            }
            if (tableName.NotNull())
                return string.Format(getAndNoLockSql, _adapter.GetTableName(tableName));
            return string.Format(getAndNoLockSql, _adapter.GetTableName(_descriptor.TableName));
        }

        #endregion


        #region ==存在语句==


        /// <summary>
        /// 存在语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string Exists(string tableName)
        {
            //没有主键，无法使用该方法
            if (_primaryKey.IsNo())
                return string.Empty;

            var sql = $"SELECT COUNT(0) FROM {{0}} WHERE {AppendQuote(_primaryKey.Name)}={AppendParameter(_primaryKey.PropertyInfo.Name)}";
            if (_descriptor.SoftDelete)
            {
                sql += $" AND {AppendQuote(_descriptor.GetDeletedColumnName())}=0 ";
            }

            if (tableName.NotNull())
                return string.Format(sql, _adapter.GetTableName(tableName));

            return string.Format(sql, _adapter.GetTableName(_descriptor.TableName));
        }

        #endregion

        #region ==清空语句==



        /// <summary>
        /// 清空语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string Clear(string tableName)
        {
            if (tableName.NotNull())
                return $"DELETE FROM {_adapter.GetTableName(tableName)}";
            return $"DELETE FROM {_adapter.GetTableName(_descriptor.TableName)}";
        }

        #endregion

        #region ==Private Methods==


        private string AppendQuote(string name)
        {
            return _descriptor.SqlAdapter.AppendQuote(name);
        }

        private string AppendParameter(string name)
        {
            return _descriptor.SqlAdapter.AppendParameter(name);
        }
        #endregion

    }
}
