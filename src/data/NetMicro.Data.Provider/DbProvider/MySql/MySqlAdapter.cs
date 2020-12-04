using NetMicro.Data.Abstractions.Enums;
using NetMicro.Data.Abstractions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using NetMicro.Core.Extensions;
using NetMicro.Data.Abstractions.Entities;
using MySql.Data.MySqlClient;

namespace NetMicro.Data.Provider.DbProvider.MySql
{
    internal class MySqlAdapter : SqlAdapterAbstract
    {
        public MySqlAdapter(DbOptions dbOptions) : base(dbOptions)
        {
        }


        public override SqlDialect SqlDialect => SqlDialect.MySql;

        /// <summary>
        /// 左引号
        /// </summary>
        public override char LeftQuote => '`';

        /// <summary>
        /// 右引号
        /// </summary>
        public override char RightQuote => '`';

        /// <summary>
        /// 获取最后新增ID语句
        /// </summary>
        public override string IdentitySql => "SELECT LAST_INSERT_ID() ID;";

        public override string FuncLength => "CHAR_LENGTH";

        public override string GeneratePagingSql(string select, string table, string where, string sort, int skip, int take, string groupBy = null, string having = null)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT {0} FROM {1}", select, table);
            if (where.NotNull())
                sqlBuilder.AppendFormat(" WHERE {0}", where);

            if (groupBy.NotNull())
                sqlBuilder.Append(groupBy);

            if (having.NotNull())
                sqlBuilder.Append(having);

            if (sort.NotNull())
                sqlBuilder.AppendFormat(" ORDER BY {0}", sort);

            sqlBuilder.AppendFormat(" LIMIT {0},{1}", skip, take);
            return sqlBuilder.ToString();
        }

        public override string GenerateFirstSql(string select, string table, string where, string sort, string groupBy = null, string having = null)
        {
            return GeneratePagingSql(select, table, where, sort, 0, 1, groupBy, having);
        }

        public override void CreateDatabase(List<IEntityDescriptor> entityDescriptors, out bool databaseExists)
        {
            var connStrBuilder = new MySqlConnectionStringBuilder
            {
                ConnectionString = DbOptions.ConnectionString,
                AllowUserVariables = true,
                CharacterSet = "utf8",
                SslMode = MySqlSslMode.None,
                AllowPublicKeyRetrieval = true
            };
            var database = connStrBuilder.Database;
            connStrBuilder.Database = "mysql";
            using var con = new MySqlConnection(connStrBuilder.ToString());
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;

            //判断数据库是否已存在
            cmd.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{database}' LIMIT 1;";
            databaseExists = cmd.ExecuteScalar().ToInt() > 0;
            if (!databaseExists)
            {
                //创建数据库
                cmd.CommandText = $"CREATE DATABASE {database} CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;";
                cmd.ExecuteNonQuery();
            }
            cmd.CommandText = $"USE `{database}`;";
            cmd.ExecuteNonQuery();

            //创建表
            foreach (var entityDescriptor in entityDescriptors)
            {
                if (!entityDescriptor.Ignore)
                {
                    cmd.CommandText = GetCreateTableSql(entityDescriptor);
                    cmd.ExecuteNonQuery();
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
                    defaultValue = "DEFAULT 0";
                }

                return "SMALLINT(3)";
            }

            if (propertyType.IsGuid())
                return "CHAR(36)";

            var typeCode = Type.GetTypeCode(propertyType);
            if (typeCode == TypeCode.Char || typeCode == TypeCode.String)
            {
                if (column.Max)
                    return "TEXT";

                if (column.Length < 1)
                    return "VARCHAR(50)";

                return $"VARCHAR({column.Length})";
            }

            if (typeCode == TypeCode.Boolean)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT 0";
                }
                return "BIT";
            }

            if (typeCode == TypeCode.Byte)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT 0";
                }
                return "TINYINT(1)";
            }

            if (typeCode == TypeCode.Int16 || typeCode == TypeCode.Int32)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT 0";
                }
                return "INT";
            }

            if (typeCode == TypeCode.Int64)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT 0";
                }
                return "BIGINT";
            }

            if (typeCode == TypeCode.DateTime)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT CURRENT_TIMESTAMP(0)";
                }
                return "DATETIME(0)";
            }

            if (typeCode == TypeCode.Decimal)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT 0";
                }

                var m = column.PrecisionM < 1 ? 18 : column.PrecisionM;
                var d = column.PrecisionD < 1 ? 4 : column.PrecisionD;

                return $"DECIMAL({m},{d})";
            }

            if (typeCode == TypeCode.Double)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT 0";
                }

                var m = column.PrecisionM < 1 ? 18 : column.PrecisionM;
                var d = column.PrecisionD < 1 ? 4 : column.PrecisionD;

                return $"DOUBLE({m},{d})";
            }

            if (typeCode == TypeCode.Single)
            {
                if (!isNullable)
                {
                    defaultValue = "DEFAULT 0";
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
            sql.AppendFormat("CREATE TABLE IF NOT EXISTS {0}(", AppendQuote(tableName ?? entityDescriptor.TableName));

            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];

                sql.AppendFormat("`{0}` ", column.Name);
                sql.AppendFormat("{0} ", column.TypeName);

                if (column.IsPrimaryKey)
                {
                    sql.Append("PRIMARY KEY ");

                    if (entityDescriptor.PrimaryKey.IsInt() || entityDescriptor.PrimaryKey.IsLong())
                    {
                        sql.Append("AUTO_INCREMENT ");
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

            sql.Append(") ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;");

            return sql.ToString();
        }

    }

}
