using Microsoft.EntityFrameworkCore.Storage;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.DataLayer;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Threading.Tasks;
using ApollosLibrary.Domain;

namespace ApollosLibrary.UnitOfWork
{
    public class PublisherUnitOfWork : IPublisherUnitOfWork, IDisposable
    {
        private IPublisherDataLayer _publisherDataLayer;
        private ApollosLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private bool disposed = false;

        public PublisherUnitOfWork(ApollosLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IPublisherDataLayer PublisherDataLayer
        {
            get
            {
                if (_publisherDataLayer == null)
                {
                    _publisherDataLayer = new PublisherDataLayer(_dbContext);
                }
                return _publisherDataLayer;
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

        ~PublisherUnitOfWork()
        {
            Dispose(false);
        }
    }
}
