using Microsoft.EntityFrameworkCore.Storage;
using ApollosLibrary.DataLayer;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Persistence.Model;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.UnitOfWork
{
    public class AuthorUnitOfWork : IAuthorUnitOfWork, IDisposable
    {
        private readonly ApollosLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private IAuthorDataLayer _authorDataLayer;
        private bool disposed = false;

        public AuthorUnitOfWork(ApollosLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IAuthorDataLayer AuthorDataLayer
        {
            get
            {
                if (_authorDataLayer == null)
                {
                    _authorDataLayer = new AuthorDataLayer(_dbContext);
                }
                return _authorDataLayer;
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

        ~AuthorUnitOfWork()
        {
            Dispose(false);
        }
    }
}
