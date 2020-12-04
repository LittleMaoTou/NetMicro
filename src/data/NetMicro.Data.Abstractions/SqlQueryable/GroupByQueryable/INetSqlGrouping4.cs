﻿using System;
using System.Linq.Expressions;
using NetMicro.Data.Abstractions.Entities;

namespace NetMicro.Data.Abstractions.SqlQueryable.GroupByQueryable
{
    /// <summary>
    /// 分组查询对象
    /// </summary>
    public interface INetSqlGrouping4<out TKey, TEntity, TEntity2, TEntity3, TEntity4> : INetSqlGrouping<TKey>
        where TEntity : IEntity, new()
        where TEntity2 : IEntity, new()
        where TEntity3 : IEntity, new()
        where TEntity4 : IEntity, new()
    {
        TResult Max<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> where);

        TResult Min<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> where);

        TResult Sum<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> where);

        TResult Avg<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> where);
    }

    /// <summary>
    /// 分组查询对象Key
    /// </summary>
    public interface INetSqlGroupingKey4<out TEntity, out TEntity2, out TEntity3, out TEntity4>
    {
        TEntity T1 { get; }

        TEntity2 T2 { get; }

        TEntity3 T3 { get; }

        TEntity4 T4 { get; }
    }
}
