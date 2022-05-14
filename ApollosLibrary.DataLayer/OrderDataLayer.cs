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

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders
                .Include(s => s.OrderItems)
                .FirstOrDefaultAsync(a => a.OrderId == id);
        }

        public async Task<List<Order>> GetOrders(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteOrderItems(int orderId)
        {
            var order = (await _context.Orders.Include(b => b.OrderItems).FirstOrDefaultAsync(b => b.OrderId == orderId));

            if (order != null)
                order.OrderItems = new ();
        }
    }
}
