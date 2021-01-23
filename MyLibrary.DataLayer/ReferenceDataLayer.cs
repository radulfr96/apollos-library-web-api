using Microsoft.EntityFrameworkCore;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.DataLayer
{
    public class ReferenceDataLayer : IReferenceDataLayer
    {
        private MyLibraryContext _context;

        public ReferenceDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task<List<Country>> GetCountries()
        {
            return await _context.Country.ToListAsync();
        }
    }
}
