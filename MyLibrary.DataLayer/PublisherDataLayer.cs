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
    public class PublisherDataLayer : IPublisherDataLayer
    {
        private readonly MyLibraryContext _context;

        public PublisherDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task AddPublisher(Publisher publisher)
        {
            await _context.Publisher.AddAsync(publisher);
        }

        public async Task<Publisher> GetPublisher(int id)
        {
            return await _context.Publisher.FirstOrDefaultAsync(p => p.PublisherId == id);
        }

        public async Task<List<Publisher>> GetPublishers()
        {
            return await _context.Publisher.Include("Country").Where(p => p.IsDeleted == null || p.IsDeleted == false).ToListAsync();
        }
    }
}
