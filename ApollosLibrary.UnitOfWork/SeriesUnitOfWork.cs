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
    public class SeriesUnitOfWork : ISeriesUnitOfWork, IDisposable
    {
        private readonly ApollosLibraryContext _dbContext;
        private IDbContextTransaction _transaction;
        private ISeriesDataLayer _seriesDataLayer;
        private bool disposed = false;

        public SeriesUnitOfWork(ApollosLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ISeriesDataLayer SeriesDataLayer
        {
            get
            {
                if (_seriesDataLayer == null)
                {
                    _seriesDataLayer = new SeriesDataLayer(_dbContext);
                }
                return _seriesDataLayer;
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

        ~SeriesUnitOfWork()
        {
            Dispose(false);
        }
    }
}
