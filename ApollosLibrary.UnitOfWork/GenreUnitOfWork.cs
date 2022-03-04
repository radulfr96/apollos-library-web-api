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
    public class GenreUnitOfWork : IGenreUnitOfWork, IDisposable
    {
        private readonly ApollosLibraryContextOld _dbContext;
        private IDbContextTransaction _transaction;
        private IGenreDataLayer _genreDataLayer;
        private bool disposed = false;

        public GenreUnitOfWork(ApollosLibraryContextOld dbContext)
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

        ~GenreUnitOfWork()
        {
            Dispose(false);
        }
    }
}
