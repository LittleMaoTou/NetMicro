﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NetMicro.Data.Abstractions;
using NetMicro.Data.Abstractions.Entities;

namespace NetMicro.Data.Provider
{
    /// <summary>
    /// 仓储抽象类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class RepositoryAbstract<TEntity> : IRepository<TEntity> where TEntity : IEntity, new()
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        protected readonly IDbContext DbContext;

        /// <summary>
        /// 数据集
        /// </summary>
        protected readonly IDbSet<TEntity> Db;

        protected RepositoryAbstract(IDbContext context)
        {
            DbContext = context;
            Db = context.Set<TEntity>();
        }

        #region ==Exists==

        public virtual bool Exists(Expression<Func<TEntity, bool>> where)
        {
            return Db.Find(where).Exists();
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> where, IUnitOfWork uow)
        {
            return Db.Find(where).UseUow(uow).Exists();
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where)
        {
            return Db.Find(where).ExistsAsync();
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where, IUnitOfWork uow)
        {
            return Db.Find(where).UseUow(uow).ExistsAsync();
        }

        public virtual bool Exists(dynamic id)
        {
            return Db.Exists(id);
        }

        public virtual bool Exists(dynamic id, IUnitOfWork uow)
        {
            return Db.Exists(id, uow);
        }

        public virtual Task<bool> ExistsAsync(dynamic id)
        {
            return Db.ExistsAsync(id);
        }

        public virtual Task<bool> ExistsAsync(dynamic id, IUnitOfWork uow)
        {
            return Db.ExistsAsync(id, uow);
        }

        #endregion

        #region ==Add==

        public virtual bool Add(TEntity entity)
        {
            if (entity == null)
                return false;

            return Db.Insert(entity);
        }

        public virtual bool Add(TEntity entity, IUnitOfWork uow)
        {
            if (entity == null)
                return false;

            return Db.Insert(entity, uow);
        }

        public virtual Task<bool> AddAsync(TEntity entity)
        {
            if (entity == null)
                return Task.FromResult(false);

            return Db.InsertAsync(entity);
        }

        public virtual Task<bool> AddAsync(TEntity entity, IUnitOfWork uow)
        {
            if (entity == null)
                return Task.FromResult(false);

            return Db.InsertAsync(entity, uow);
        }

        public virtual bool Add(List<TEntity> list)
        {
            return Db.BatchInsert(list);
        }

        public virtual bool Add(List<TEntity> list, IUnitOfWork uow)
        {
            return Db.BatchInsert(list, uow: uow);
        }
        public virtual Task<bool> AddAsync(List<TEntity> list)
        {
            return Db.BatchInsertAsync(list);
        }

        public virtual Task<bool> AddAsync(List<TEntity> list, IUnitOfWork uow)
        {
            return Db.BatchInsertAsync(list, uow: uow);
        }

        #endregion

        #region ==Delete==

        public virtual bool Delete(dynamic id)
        {
            return Db.Delete(id);
        }

        public virtual bool Delete(dynamic id, IUnitOfWork uow)
        {
            return Db.Delete(id, uow);
        }

        public virtual Task<bool> DeleteAsync(dynamic id)
        {
            return Db.DeleteAsync(id);
        }

        public virtual Task<bool> DeleteAsync(dynamic id, IUnitOfWork uow)
        {
            return Db.DeleteAsync(id, uow);
        }

        #endregion

        #region ==Update==

        public virtual bool Update(TEntity entity)
        {
            return Db.Update(entity);
        }

        public virtual bool Update(TEntity entity, IUnitOfWork uow)
        {
            return Db.Update(entity, uow);
        }

        public virtual Task<bool> UpdateAsync(TEntity entity)
        {
            return Db.UpdateAsync(entity);
        }

        public virtual Task<bool> UpdateAsync(TEntity entity, IUnitOfWork uow)
        {
            return Db.UpdateAsync(entity, uow);
        }

        #endregion

        #region ==Get==

        public virtual TEntity Get(dynamic id)
        {
            return Db.Get(id);
        }

        public virtual TEntity Get(dynamic id, IUnitOfWork uow, bool rowLock = false, bool noLock = true)
        {
            return Db.Get(id, uow, null, rowLock, noLock);
        }

        public virtual Task<TEntity> GetAsync(dynamic id)
        {
            return Db.GetAsync(id);
        }

        public virtual Task<TEntity> GetAsync(dynamic id, IUnitOfWork uow, bool rowLock = false, bool noLock = true)
        {
            return Db.GetAsync(id, uow, null, rowLock, noLock);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> @where)
        {
            return Db.Find(where).First();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> @where, IUnitOfWork uow)
        {
            return Db.Find(where).UseUow(uow).First();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> @where)
        {
            return Db.Find(where).FirstAsync<TEntity>();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> @where, IUnitOfWork uow)
        {
            return Db.Find(where).UseUow(uow).FirstAsync<TEntity>();
        }

        #endregion

        #region ==GetAll==

        public virtual IList<TEntity> GetAll()
        {
            return Db.Find().ToList<TEntity>();
        }

        public virtual IList<TEntity> GetAll(IUnitOfWork uow)
        {
            return Db.Find().UseUow(uow).ToList<TEntity>();
        }

        public virtual Task<IList<TEntity>> GetAllAsync()
        {
            return Db.Find().ToListAsync<TEntity>();
        }

        public virtual Task<IList<TEntity>> GetAllAsync(IUnitOfWork uow)
        {
            return Db.Find().UseUow(uow).ToListAsync<TEntity>();
        }

        #endregion

        #region ==SoftDelete==

        public bool SoftDelete(dynamic id)
        {
            return Db.SoftDelete(id);
        }

        public bool SoftDelete(dynamic id, IUnitOfWork uow)
        {
            return Db.SoftDelete(id, uow);
        }

        public Task<bool> SoftDeleteAsync(dynamic id)
        {
            return Db.SoftDeleteAsync(id);
        }

        public Task<bool> SoftDeleteAsync(dynamic id, IUnitOfWork uow)
        {
            return Db.SoftDeleteAsync(id, uow);
        }

        public bool Clear(IUnitOfWork uow = null)
        {
            return Db.Clear(uow);
        }

        public Task<bool> ClearAsync(IUnitOfWork uow = null)
        {
            return Db.ClearAsync(uow);
        }

        #endregion

        #region ==表操作==

        public Task CreateTable(string tableName = null)
        {
            return Db.ExecuteAsync(DbContext.SqlAdapter.GetCreateTableSql(Db.EntityDescriptor, tableName));
        }

        #endregion
    }
}
