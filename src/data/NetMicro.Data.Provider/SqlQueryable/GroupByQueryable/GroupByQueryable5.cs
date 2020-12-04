using System;
using System.Linq.Expressions;
using NetMicro.Data.Abstractions;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.SqlQueryable.GroupByQueryable;
using NetMicro.Data.Provider.SqlQueryable.Internal;

namespace NetMicro.Data.Provider.SqlQueryable.GroupByQueryable
{
    internal class GroupByQueryable5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5> : GroupByQueryableAbstract, IGroupByQueryable5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5>
        where TEntity : IEntity, new()
        where TEntity2 : IEntity, new()
        where TEntity3 : IEntity, new()
        where TEntity4 : IEntity, new()
        where TEntity5 : IEntity, new()
    {
        public GroupByQueryable5(IDbSet db, QueryBody queryBody, QueryBuilder queryBuilder, Expression expression) : base(db, queryBody, queryBuilder, expression)
        {
        }
        public IGroupByQueryable5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5> Having(Expression<Func<INetSqlGrouping5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5>, bool>> expression)
        {
            SetHaving(expression);
            return this;
        }

        public IGroupByQueryable5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5> OrderBy(string customOrderBy)
        {
            SetOrderBy(customOrderBy);
            return this;
        }

        public IGroupByQueryable5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5> OrderByDescending(string customOrderBy)
        {
            SetOrderByDescending(customOrderBy);
            return this;
        }

        public IGroupByQueryable5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5> OrderBy<TResult>(Expression<Func<INetSqlGrouping5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5>, TResult>> expression)
        {
            SetOrderBy(expression);
            return this;
        }

        public IGroupByQueryable5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5> OrderByDescending<TResult>(Expression<Func<INetSqlGrouping5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5>, TResult>> expression)
        {
            SetOrderByDescending(expression);
            return this;
        }

        public IGroupByQueryable5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5> Select<TResult>(Expression<Func<INetSqlGrouping5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5>, TResult>> expression)
        {
            SetSelect(expression);
            return this;
        }

        public IGroupByQueryable5<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5> Limit(int skip, int take)
        {
            SetLimit(skip, take);
            return this;
        }
    }
}
