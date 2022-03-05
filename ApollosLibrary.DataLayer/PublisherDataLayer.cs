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
    public class PublisherDataLayer : IPublisherDataLayer
    {
        private readonly ApollosLibraryContext _context;

        public PublisherDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddPublisher(Publisher publisher)
        {
            await _context.Publishers.AddAsync(publisher);
        }

        public async Task<Publisher> GetPublisher(int id)
        {
            return await _context.Publishers.FirstOrDefaultAsync(p => p.PublisherId == id);
        }

        public async Task<List<Publisher>> GetPublishers()
        {
            return await _context.Publishers.Include("Country").Where(p => p.IsDeleted == null || p.IsDeleted == false).ToListAsync();
        }
    }
}
