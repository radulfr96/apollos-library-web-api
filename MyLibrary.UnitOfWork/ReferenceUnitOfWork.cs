using MyLibrary.Persistence.Model;
using MyLibrary.DataLayer;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.UnitOfWork
{
    public class ReferenceUnitOfWork : IReferenceUnitOfWork
    {
        private MyLibraryContext _context;
        private IReferenceDataLayer _referenceDataLayer;

        public ReferenceUnitOfWork(MyLibraryContext context)
        {
            _context = context;
        }

        public IReferenceDataLayer ReferenceDataLayer
        {
            get
            {
                if (_referenceDataLayer == null)
                {
                    _referenceDataLayer = new ReferenceDataLayer(_context);
                }
                return _referenceDataLayer;
            }
        }
    }
}
