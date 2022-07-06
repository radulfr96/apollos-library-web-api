using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Order.Queries.GetOrdersQuery
{
    public class GetOrdersQueryDto
    {
        public List<OrderListItem> Orders { get; set; } = new List<OrderListItem>();
    }

    public class OrderListItem
    {
        public int OrderId { get; set; }
        public string Bookshop { get; set; }
        public LocalDateTime OrderDate { get; set; }
        public int NumberOfItems { get; set; }
        public decimal Total { get; set; }
    }
}
