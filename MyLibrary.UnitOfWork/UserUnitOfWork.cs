using Microsoft.EntityFrameworkCore.Storage;
using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.DataLayer;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
using System;
using System.Threading.Tasks;

namespace MyLibrary.UnitOfWork
{
    public class UserUnitOfWork : IUserUnitOfWork, IDisposable
    {
        private MyLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private IUserDataLayer _userDataLayer;
        private IRoleDataLayer _roleDataLayer;
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

        public IRoleDataLayer RoleDataLayer
        {
            get
            {
                if (_roleDataLayer == null)
                {
                    _roleDataLayer = new RoleDataLayer(_dbContext);
                }
                return _roleDataLayer;
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

        ~UserUnitOfWork()
        {
            Dispose(false);
        }
    }
}
