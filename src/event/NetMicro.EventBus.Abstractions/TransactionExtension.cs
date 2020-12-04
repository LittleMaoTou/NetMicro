using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using NetMicro.Core.Extensions;
using NetMicro.Data.Abstractions;

namespace NetMicro.EventBus.Abstractions
{
    public static class TransactionExtension
    {

        public static ICapTransaction BeginTransaction(this IUnitOfWork unitOfWork, IEventBus eventBus, bool autoCommit = false)
        {
            unitOfWork.CheckNull("事务扩展，工作单元不可传递为空");
            var capTransaction = eventBus.ServiceProvider.GetService<ICapTransaction>();
            var publisher = eventBus.ServiceProvider.GetService<ICapPublisher>();
            capTransaction.DbTransaction = unitOfWork.Transaction;
            capTransaction.AutoCommit = false;
            publisher.Transaction.Value = capTransaction;
            return capTransaction;
        }

        public static void Flush(this ICapTransaction capTransaction)
        {
            capTransaction.CheckNull("事务扩展，cap事务不可传递为空");
            capTransaction?.GetType().GetMethod("Flush", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.Invoke(capTransaction, null);
        }

        public static void Commit(this IUnitOfWork unitOfWork, ICapTransaction capTransaction)
        {
            unitOfWork.CheckNull("事务扩展，工作单元不可传递为空");
            unitOfWork.Commit();
            capTransaction.Flush();
        }

    }
}
