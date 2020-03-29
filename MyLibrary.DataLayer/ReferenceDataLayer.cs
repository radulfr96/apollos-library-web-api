using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.DataLayer
{
    class ReferenceDataLayer : IReferenceDataLayer
    {
        private MyLibraryContext _context;

        public ReferenceDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public List<Country> GetCountries()
        {
            return _context.Country.ToList();
        }
    }
}
