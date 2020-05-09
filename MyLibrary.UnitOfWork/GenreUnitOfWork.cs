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
    public class GenreUnitOfWork : IGenreUnitOfWork, IDisposable
    {
        private readonly MyLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private IGenreDataLayer _genreDataLayer;
        private bool disposed = false;

        public GenreUnitOfWork(MyLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenreDataLayer GenreDataLayer
        {
            get
            {
                if (_genreDataLayer == null)
                {
                    _genreDataLayer = new GenreDataLayer(_dbContext);
                }
                return _genreDataLayer;
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

        ~GenreUnitOfWork()
        {
            Dispose(false);
        }
    }
}
