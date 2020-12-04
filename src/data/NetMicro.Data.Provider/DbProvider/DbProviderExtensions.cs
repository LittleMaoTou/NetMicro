using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using System.Data;
using NetMicro.Core.Exceptions;
using NetMicro.Data.Abstractions;
using NetMicro.Data.Abstractions.Enums;
using NetMicro.Data.Abstractions.Options;
using NetMicro.Data.Provider.DbProvider.MySql;
using NetMicro.Data.Provider.DbProvider.SQLite;
using NetMicro.Data.Provider.DbProvider.SqlServer;

namespace NetMicro.Data.Provider.DbProvider
{
    public static class DbProviderExtensions
    {
        public static ISqlAdapter SetDbSqlAdapter(this DbOptions options)
        {
            if (options == null)
                throw new DataAccessException("数据库配置不可为空");
            switch (options.Dialect)
            {
                case SqlDialect.SqlServer:
                    return new SqlServerAdapter(options);
                case SqlDialect.MySql:
                    return new MySqlAdapter(options);
                case SqlDialect.SQLite:
                    return new SQLiteAdapter(options);
                default:
                    throw new DataAccessException("不支持数据库类型");
            }
        }

        public static IDbConnection SetConnection(this DbOptions options)
        {
            if (options == null)
                throw new DataAccessException("数据库配置不可为空");
            switch (options.Dialect)
            {
                case SqlDialect.MySql:
                    return new MySqlConnection(options.ConnectionString);
                case SqlDialect.SqlServer:
                    return new SqlConnection(options.ConnectionString);
                case SqlDialect.SQLite:
                    return new SqliteConnection(options.ConnectionString);
                default:
                    throw new DataAccessException("不支持数据库类型");
            }
        }
    }
}
