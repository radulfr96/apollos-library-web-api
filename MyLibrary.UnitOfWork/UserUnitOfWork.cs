using Microsoft.EntityFrameworkCore.Storage;
using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer;
using MyLibrary.DataLayer.Contracts;
using System;

namespace MyLibrary.UnitOfWork
{
    public class UserUnitOfWork : IUserUnitOfWork, IDisposable
    {
        private MyLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private IUserDataLayer _userDataLayer;
        private bool disposed = false;

        public UserUnitOfWork(MyLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserDataLayer UserDataLayer
        {
            get
            {
                if (_userDataLayer == null)
                {
                    _userDataLayer = new UserDataLayer(_dbContext);
                }
                return _userDataLayer;
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

        ~UserUnitOfWork()
        {
            Dispose(false);
        }
    }
}
