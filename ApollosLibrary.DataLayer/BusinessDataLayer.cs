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
    public class BusinessDataLayer : IBusinessDataLayer
    {
        private readonly ApollosLibraryContext _context;

        public BusinessDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddBusiness(Business Business)
        {
            await _context.Business.AddAsync(Business);
        }

        public async Task<Business> GetBusiness(int id)
        {
            return await _context.Business.FirstOrDefaultAsync(b => b.BusinessId == id);
        }

        public async Task<List<Business>> GetBusinesses()
        {
            return await _context.Business.Include(b => b.Country).Include(b => b.Type).Where(b => b.IsDeleted == null || b.IsDeleted == false).ToListAsync();
        }
    }
}
