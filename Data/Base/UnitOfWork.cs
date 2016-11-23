using System.Data.Entity;
using System.Transactions;
using Data.IBase;
using Data.Models;

namespace Data.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionScope _transaction;
        private readonly ComputerServiceEntities _db;

        public UnitOfWork()
        {
            _db = new ComputerServiceEntities();

        }

        public void Dispose()
        {

        }

        public void StartTransaction()
        {
            _transaction = new TransactionScope();
        }

        public void Commit()
        {
            _db.SaveChanges();
            _transaction.Complete();
        }

        public DbContext Db
        {
            get { return _db; }
        }
    }
}
