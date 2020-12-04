using System;
using System.Linq.Expressions;
using NetMicro.Data.Abstractions;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.SqlQueryable.GroupByQueryable;
using NetMicro.Data.Provider.SqlQueryable.Internal;

namespace NetMicro.Data.Provider.SqlQueryable.GroupByQueryable
{
    internal class GroupByQueryable2<TKey, TEntity, TEntity2> : GroupByQueryableAbstract, IGroupByQueryable2<TKey, TEntity, TEntity2> where TEntity : IEntity, new() where TEntity2 : IEntity, new()
    {
        public GroupByQueryable2(IDbSet db, QueryBody queryBody, QueryBuilder queryBuilder, Expression expression) : base(db, queryBody, queryBuilder, expression)
        {
        }
        public IGroupByQueryable2<TKey, TEntity, TEntity2> Having(Expression<Func<INetSqlGrouping2<TKey, TEntity, TEntity2>, bool>> expression)
        {
            SetHaving(expression);
            return this;
        }

        public IGroupByQueryable2<TKey, TEntity, TEntity2> OrderBy(string customOrderBy)
        {
            SetOrderBy(customOrderBy);
            return this;
        }

        public IGroupByQueryable2<TKey, TEntity, TEntity2> OrderByDescending(string customOrderBy)
        {
            SetOrderByDescending(customOrderBy);
            return this;
        }

        public IGroupByQueryable2<TKey, TEntity, TEntity2> OrderBy<TResult>(Expression<Func<INetSqlGrouping2<TKey, TEntity, TEntity2>, TResult>> expression)
        {
            SetOrderBy(expression);
            return this;
        }

        public IGroupByQueryable2<TKey, TEntity, TEntity2> OrderByDescending<TResult>(Expression<Func<INetSqlGrouping2<TKey, TEntity, TEntity2>, TResult>> expression)
        {
            SetOrderByDescending(expression);
            return this;
        }

        public IGroupByQueryable2<TKey, TEntity, TEntity2> Select<TResult>(Expression<Func<INetSqlGrouping2<TKey, TEntity, TEntity2>, TResult>> expression)
        {
            SetSelect(expression);
            return this;
        }

        public IGroupByQueryable2<TKey, TEntity, TEntity2> Limit(int skip, int take)
        {
            SetLimit(skip, take);
            return this;
        }
    }
}
