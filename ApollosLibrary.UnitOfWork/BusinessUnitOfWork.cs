using Microsoft.EntityFrameworkCore.Storage;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.DataLayer;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Threading.Tasks;
using ApollosLibrary.Domain;

namespace ApollosLibrary.UnitOfWork
{
    public class BusinessUnitOfWork : IBusinessUnitOfWork, IDisposable
    {
        private IBusinessDataLayer _BusinessDataLayer;
        private readonly ApollosLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private bool disposed = false;

        public BusinessUnitOfWork(ApollosLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IBusinessDataLayer BusinessDataLayer
        {
            get
            {
                if (_BusinessDataLayer == null)
                {
                    _BusinessDataLayer = new BusinessDataLayer(_dbContext);
                }
                return _BusinessDataLayer;
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

        ~BusinessUnitOfWork()
        {
            Dispose(false);
        }
    }
}
