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
    public class SubscriptionUnitOfWork : ISubscriptionUnitOfWork
    {
        private readonly ApollosLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private ISubscriptionDataLayer _subscriptionDataLayer;
        private bool disposed = false;

        public SubscriptionUnitOfWork(ApollosLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ISubscriptionDataLayer SubscriptionDataLayer
        {
            get
            {
                if (_subscriptionDataLayer == null)
                {
                    _subscriptionDataLayer = new SubscriptionDataLayer(_dbContext);
                }
                return _subscriptionDataLayer;
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

        ~SubscriptionUnitOfWork()
        {
            Dispose(false);
        }
    }
}
