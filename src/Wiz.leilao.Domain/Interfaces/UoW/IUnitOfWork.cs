using System;

namespace Wiz.leilao.Domain.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        void BeginTransaction();
        void BeginCommit();
        void BeginRollback();
    }
}
