using System;
using System.Data;
using NetMicro.Data.Abstractions;

namespace NetMicro.Data.Provider
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        public IDbTransaction Transaction { get; private set; }

        public void Commit()
        {
            Transaction?.Commit();
            Close();
            Transaction = null;
        }

        public void Rollback()
        {
            Transaction?.Rollback();
            Close();
        }


        private void Close()
        {
            Transaction?.Connection?.Close();
        }
        #region dispose
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Rollback();
                }

                disposed = true;
            }
        }
        ~UnitOfWork()
        {
            Dispose(false);
        }


        #endregion


    }
}
