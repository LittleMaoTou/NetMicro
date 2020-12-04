﻿using System;

namespace NetMicro.Data.Abstractions.Entities
{
    /// <summary>
    /// 实体信息
    /// </summary>
    public interface IEntityDescriptor
    {
        /// <summary>
        /// 上下文
        /// </summary>
        IDbContext DbContext { get; }



        /// <summary>
        /// 表名称
        /// </summary>
        string TableName { get; }

      

        /// <summary>
        /// 不创建表
        /// </summary>
        bool Ignore { get; }

        /// <summary>
        /// 实体类型
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// 列集合
        /// </summary>
        IColumnDescriptorCollection Columns { get; }

        /// <summary>
        /// 主键列
        /// </summary>
        IPrimaryKeyDescriptor PrimaryKey { get; }

        /// <summary>
        /// 是否包含软删除
        /// </summary>
        bool SoftDelete { get; }

        /// <summary>
        /// SQL语句
        /// </summary>
        IEntitySqlBuilder Sql { get; }

        /// <summary>
        /// 数据库适配器
        /// </summary>
        ISqlAdapter SqlAdapter { get; }

        /// <summary>
        /// 是否是EntityBase类型实体
        /// </summary>
        bool IsEntityBase { get; }
    }
}
