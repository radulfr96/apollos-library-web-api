using Microsoft.EntityFrameworkCore.Storage;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.DataLayer;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.UnitOfWork
{
    public class PublisherUnitOfWork : IPublisherUnitOfWork, IDisposable
    {
        private IPublisherDataLayer _publisherDataLayer;
        private MyLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private bool disposed = false;

        public PublisherUnitOfWork(MyLibraryContext dbContext)
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

        ~PublisherUnitOfWork()
        {
            Dispose(false);
        }
    }
}
