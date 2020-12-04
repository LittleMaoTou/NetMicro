﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using NetMicro.Core.Extensions;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.Options;

namespace NetMicro.Data.Provider.DbProvider.SqlServer
{
    internal class SqlServerAdapter : SqlAdapterAbstract
    {
        public SqlServerAdapter(DbOptions dbOptions) : base(dbOptions)
        {
        }



        /// <summary>
        /// 左引号
        /// </summary>
        public override char LeftQuote => '[';

        /// <summary>
        /// 右引号
        /// </summary>
        public override char RightQuote => ']';

        /// <summary>
        /// 获取最后新增ID语句
        /// </summary>
        public override string IdentitySql => "SELECT SCOPE_IDENTITY() ID;";

        public override string FuncSubstring => "SUBSTRING";

        public override string FuncLength => "LEN";

        public override string GeneratePagingSql(string select, string table, string where, string sort, int skip, int take, string groupBy = null, string having = null)
        {
            var sqlBuilder = new StringBuilder();

            if (DbOptions.Version.IsNull() || DbOptions.Version.ToInt() >= 2012)
            {
                #region ==2012+版本==

                sqlBuilder.AppendFormat("SELECT {0} FROM {1}", @select, table);
                if (!string.IsNullOrWhiteSpace(where))
                    sqlBuilder.AppendFormat(" WHERE {0}", @where);

                if (groupBy.NotNull())
                    sqlBuilder.Append(groupBy);

                if (having.NotNull())
                    sqlBuilder.Append(having);

                sqlBuilder.AppendFormat(" ORDER BY {0} OFFSET {1} ROW FETCH NEXT {2} ROW ONLY", sort, skip, take);

                #endregion
            }
            else
            {
                #region ==2018及以下版本==

                sqlBuilder.AppendFormat("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {0}) AS RowNum,{1} FROM {2}", sort, @select, table);
                if (!string.IsNullOrWhiteSpace(where))
                    sqlBuilder.AppendFormat(" WHERE {0}", @where);

                sqlBuilder.AppendFormat(") AS T WHERE T.RowNum BETWEEN {0} AND {1}", skip, skip + take);

                #endregion
            }

            return sqlBuilder.ToString();
        }

        public override string GenerateFirstSql(string select, string table, string where, string sort, string groupBy = null, string having = null)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT TOP 1 {0} FROM {1}", select, table);
            if (!string.IsNullOrWhiteSpace(where))
                sqlBuilder.AppendFormat(" WHERE {0}", where);

            if (groupBy.NotNull())
                sqlBuilder.Append(groupBy);

            if (having.NotNull())
                sqlBuilder.Append(having);

            if (!string.IsNullOrWhiteSpace(sort))
            {
                sqlBuilder.AppendFormat(" ORDER BY {0} ", sort);
            }

            return sqlBuilder.ToString();
        }

        public override void CreateDatabase(List<IEntityDescriptor> entityDescriptors, out bool databaseExists)
        {
            var connStrBuilder = new SqlConnectionStringBuilder
            {
                ConnectionString = DbOptions.ConnectionString,
                MultipleActiveResultSets = true
            };
            var database = connStrBuilder.InitialCatalog;
            connStrBuilder.InitialCatalog = "master";
            using var con = new SqlConnection(connStrBuilder.ToString());
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;

            //判断数据库是否已存在
            cmd.CommandText = $"SELECT TOP 1 1 FROM sysdatabases WHERE name = '{database}'";
            databaseExists = cmd.ExecuteScalar().ToInt() > 0;
            if (!databaseExists)
            {
                //创建数据库
                cmd.CommandText = $"CREATE DATABASE [{database}]";
                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = $"USE [{database}];";
            cmd.ExecuteNonQuery();

            //创建表
            foreach (var entityDescriptor in entityDescriptors)
            {
                if (!entityDescriptor.Ignore)
                {
                    cmd.CommandText = $"SELECT TOP 1 1 FROM sysobjects WHERE id = OBJECT_ID(N'{entityDescriptor.TableName}') AND xtype = 'U';";
                    var obj = cmd.ExecuteScalar();
                    if (obj.ToInt() < 1)
                    {
                        cmd.CommandText = GetCreateTableSql(entityDescriptor);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

        }

        public override string GetColumnTypeName(IColumnDescriptor column, out string defaultValue)
        {
            defaultValue = "";
            var propertyType = column.PropertyInfo.PropertyType;
            var isNullable = propertyType.IsNullable();
            if (isNullable)
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
                if (propertyType == null)
                    throw new Exception("Property2Column error");
            }

            if (propertyType.IsEnum)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT(0)";
                }

                return "SMALLINT";
            }

            if (propertyType.IsGuid())
                return "UNIQUEIDENTIFIER";

            var typeCode = Type.GetTypeCode(propertyType);
            if (typeCode == TypeCode.Char || typeCode == TypeCode.String)
            {
                if (column.Max)
                    return "NVARCHAR(MAX)";

                if (column.Length < 1)
                    return "NVARCHAR(50)";

                return $"NVARCHAR({column.Length})";
            }

            if (typeCode == TypeCode.Boolean)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT(0)";
                }
                return "BIT";
            }

            if (typeCode == TypeCode.Byte)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT(0)";
                }
                return "TINYINT(1)";
            }

            if (typeCode == TypeCode.Int16 || typeCode == TypeCode.Int32)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT(0)";
                }
                return "INT";
            }

            if (typeCode == TypeCode.Int64)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT(0)";
                }
                return "BIGINT";
            }

            if (typeCode == TypeCode.DateTime)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT(GETDATE())";
                }
                return "DATETIME";
            }

            if (typeCode == TypeCode.Decimal || typeCode == TypeCode.Double)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT(0)";
                }

                var m = column.PrecisionM < 1 ? 18 : column.PrecisionM;
                var d = column.PrecisionD < 1 ? 4 : column.PrecisionD;

                return $"DECIMAL({m},{d})";
            }

            if (typeCode == TypeCode.Single)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT(0)";
                }

                var m = column.PrecisionM < 1 ? 18 : column.PrecisionM;
                var d = column.PrecisionD < 1 ? 4 : column.PrecisionD;

                return $"FLOAT({m},{d})";
            }

            return string.Empty;
        }

        public override string GetCreateTableSql(IEntityDescriptor entityDescriptor, string tableName = null)
        {
            var columns = entityDescriptor.Columns;
            var sql = new StringBuilder();
            sql.AppendFormat("CREATE TABLE [{0}](", tableName ?? entityDescriptor.TableName);

            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                sql.AppendFormat("[{0}] ", column.Name);
                sql.AppendFormat("{0} ", column.TypeName);

                if (column.IsPrimaryKey)
                {
                    sql.Append("PRIMARY KEY ");

                    if (entityDescriptor.PrimaryKey.IsInt() || entityDescriptor.PrimaryKey.IsLong())
                    {
                        sql.Append("IDENTITY(1,1) ");
                    }
                }

                if (!column.Nullable)
                {
                    sql.Append("NOT NULL ");
                }

                if (!column.IsPrimaryKey && column.DefaultValue.NotNull())
                {
                    sql.Append(column.DefaultValue);
                }

                if (i < columns.Count - 1)
                {
                    sql.Append(",");
                }
            }

            sql.Append(");");

            return sql.ToString();
        }
    }

}
