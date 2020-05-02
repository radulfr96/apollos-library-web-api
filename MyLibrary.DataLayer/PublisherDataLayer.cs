using Microsoft.EntityFrameworkCore;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.DataLayer
{
    public class PublisherDataLayer : IPublisherDataLayer
    {
        private readonly MyLibraryContext _context;

        public PublisherDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public void AddPublisher(Publisher publisher)
        {
            _context.Publisher.Add(publisher);
        }

        public Publisher GetPublisher(int id)
        {
            return _context.Publisher.FirstOrDefault(p => p.PublisherId == id);
        }

        public List<Publisher> GetPublishers()
        {
            return _context.Publisher.Include("Country").Where(p => p.IsDeleted == null || p.IsDeleted == false).ToList();
        }
    }
}
