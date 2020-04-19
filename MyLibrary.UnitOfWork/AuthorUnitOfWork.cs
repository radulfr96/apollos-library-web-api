using Microsoft.EntityFrameworkCore.Storage;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.UnitOfWork
{
    public class AuthorUnitOfWork : IAuthorUnitOfWork, IDisposable
    {
        private readonly MyLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private IAuthorDataLayer _authorDataLayer;
        private bool disposed = false;

        public AuthorUnitOfWork(MyLibraryContext dbContext)
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

        public void Begin()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
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

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        ~AuthorUnitOfWork()
        {
            Dispose(false);
        }
    }
}
