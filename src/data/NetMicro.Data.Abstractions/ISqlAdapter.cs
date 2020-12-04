﻿using System.Collections.Generic;
using System.Text;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.Enums;
using NetMicro.Data.Abstractions.Options;

namespace NetMicro.Data.Abstractions
{
    public interface ISqlAdapter
    {
        #region ==属性==

        /// <summary>
        /// 数据库配置项
        /// </summary>
        DbOptions DbOptions { get; }


        /// <summary>
        /// 获取表名
        /// </summary>
        string GetTableName(string tableName);

        /// <summary>
        /// 数据库类型
        /// </summary>
        SqlDialect SqlDialect { get; }

        /// <summary>
        /// 左引号
        /// </summary>
        char LeftQuote { get; }

        /// <summary>
        /// 右引号
        /// </summary>
        char RightQuote { get; }

        /// <summary>
        /// 参数前缀符号
        /// </summary>
        char ParameterPrefix { get; }

        /// <summary>
        /// 获取新增ID的SQL语句
        /// </summary>
        string IdentitySql { get; }

        /// <summary>
        /// 字符串截取函数
        /// </summary>
        string FuncSubstring { get; }

        /// <summary>
        /// 字符串长度函数
        /// </summary>
        string FuncLength { get; }

        /// <summary>
        /// 转小写函数
        /// </summary>
        string FuncLower { get; }

        /// <summary>
        /// 转大写函数
        /// </summary>
        string FuncUpper { get; }

        /// <summary>
        /// 命名小写
        /// </summary>
        bool ToLower { get; }

        #endregion

        #region ==方法==

        /// <summary>
        /// 给定的值附加引号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string AppendQuote(string value);

        /// <summary>
        /// 给定的值附加引号
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void AppendQuote(StringBuilder sb, string value);

        /// <summary>
        /// 附加参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns></returns>
        string AppendParameter(string parameterName);

        /// <summary>
        /// 附加参数
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="parameterName">参数名</param>
        /// <returns></returns>
        void AppendParameter(StringBuilder sb, string parameterName);

        /// <summary>
        /// 附加含有值的参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns></returns>
        string AppendParameterWithValue(string parameterName);

        /// <summary>
        /// 附加含有值的参数
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="parameterName">参数名</param>
        /// <returns></returns>
        void AppendParameterWithValue(StringBuilder sb, string parameterName);

        /// <summary>
        /// 附加查询条件
        /// </summary>
        /// <param name="queryWhere"></param>
        /// <returns></returns>
        string AppendQueryWhere(string queryWhere);

        /// <summary>
        /// 附加查询条件
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="queryWhere">查询条件</param>
        void AppendQueryWhere(StringBuilder sb, string queryWhere);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="select"></param>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="groupBy"></param>
        /// <param name="having"></param>
        /// <returns></returns>
        string GeneratePagingSql(string select, string table, string where, string sort, int skip, int take, string groupBy = null, string having = null);

        /// <summary>
        /// 生成获取第一条数据的Sql
        /// </summary>
        /// <param name="select"></param>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <param name="groupBy"></param>
        /// <param name="having"></param>
        /// <returns></returns>
        string GenerateFirstSql(string select, string table, string where, string sort, string groupBy = null, string having = null);

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="entityDescriptors"></param>
        /// <param name="databaseExists"></param>

        void CreateDatabase(List<IEntityDescriptor> entityDescriptors, out bool databaseExists);

        /// <summary>
        /// 解析列类型名称并返回默认值
        /// </summary>
        /// <param name="column"></param>
        /// <param name="defaultValue"></param>
        string GetColumnTypeName(IColumnDescriptor column, out string defaultValue);

        /// <summary>
        /// 获取创建表Sql语句
        /// </summary>
        /// <param name="entityDescriptor"></param>
        /// <param name="tableName">指定表名称</param>
        /// <returns></returns>
        string GetCreateTableSql(IEntityDescriptor entityDescriptor, string tableName = null);

        #endregion


    }
}
