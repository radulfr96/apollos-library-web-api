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
    public class BookUnitOfWork : IBookUnitOfWork
    {
        private IBookDataLayer _bookDataLayer;
        private readonly MyLibraryContext _context;
        private IDbContextTransaction _transaction;
        private bool disposed = false;

        public BookUnitOfWork(MyLibraryContext context)
        {
            _context = context;
        }

        public IBookDataLayer BookDataLayer
        {
            get
            {
                if (_bookDataLayer == null)
                {
                    _bookDataLayer = new BookDataLayer(_context);
                }
                return _bookDataLayer;
            }
        }

        public void Begin()
        {
            _transaction = _context.Database.BeginTransaction();
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
                _context.Dispose();
            }

            disposed = true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        ~BookUnitOfWork()
        {
            Dispose(false);
        }
    }
}
