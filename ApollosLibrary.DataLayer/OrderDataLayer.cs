using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using ApollosLibrary.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer
{
    public class OrderDataLayer : IOrderDataLayer
    {
        private readonly ApollosLibraryContext _context;

        public OrderDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddOrder(Order series)
        {
            await _context.Orders.AddAsync(series);
        }

        public async Task DeleteOrder(int id)
        {
            _context.Orders.Remove(await _context.Orders.FirstOrDefaultAsync(g => g.OrderId == id));
        }

        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders
                .Include(s => s.OrderItems)
                .FirstOrDefaultAsync(a => a.OrderId == id);
        }

        public async Task<List<Order>> GetOrders(Guid userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }
    }
}
