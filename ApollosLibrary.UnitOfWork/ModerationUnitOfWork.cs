using ApollosLibrary.DataLayer;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using ApollosLibrary.UnitOfWork.Contracts;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.UnitOfWork
{
    public class ModerationUnitOfWork : IModerationUnitOfWork, IDisposable
    {
        private readonly ApollosLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private IModerationDataLayer _moderationDataLayer;
        private bool disposed = false;

        public ModerationUnitOfWork(ApollosLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IModerationDataLayer ModerationDataLayer
        {
            get
            {
                if (_moderationDataLayer == null)
                {
                    _moderationDataLayer = new ModerationDataLayer(_dbContext);
                }
                return _moderationDataLayer;
            }
        }

        public async Task Begin()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _transaction.CommitAsync();
        }

        public async Task Rollback()
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.RollbackTransactionAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                }
                _dbContext.Dispose();
            }

            disposed = true;
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }



        ~ModerationUnitOfWork()
        {
            Dispose(false);
        }
    }
}
