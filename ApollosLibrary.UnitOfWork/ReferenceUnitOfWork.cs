using ApollosLibrary.Persistence.Model;
using ApollosLibrary.DataLayer;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApollosLibrary.UnitOfWork
{
    public class ReferenceUnitOfWork : IReferenceUnitOfWork
    {
        private ApollosLibraryContext _context;
        private IReferenceDataLayer _referenceDataLayer;

        public ReferenceUnitOfWork(ApollosLibraryContext context)
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
