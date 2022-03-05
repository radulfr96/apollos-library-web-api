using Microsoft.EntityFrameworkCore;
using ApollosLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApollosLibrary.Domain;

namespace ApollosLibrary.DataLayer
{
    public class ReferenceDataLayer : IReferenceDataLayer
    {
        private ApollosLibraryContext _context;

        public ReferenceDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task<List<Country>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<FictionType> GetFictionType(int fictionTypeId)
        {
            return await _context.FictionTypes.FirstOrDefaultAsync(f => f.TypeId == fictionTypeId);
        }

        public async Task<FormType> GetFormType(int formTypeId)
        {
            return await _context.FormTypes.FirstOrDefaultAsync(f => f.TypeId == formTypeId);
        }

        public async Task<PublicationFormat> GetPublicationFormat(int publicationFormatId)
        {
            return await _context.PublicationFormats.FirstOrDefaultAsync(f => f.TypeId == publicationFormatId);
        }
    }
}
