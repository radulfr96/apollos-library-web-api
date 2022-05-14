using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Order.Queries.GetOrdersQuery
{
    public class GetOrdersQueryDto
    {
        public List<OrderDTO> Orders = new List<OrderDTO>();
    }

    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string Bookshop { get; set; }
        public int NumberOfItems { get; set; }
        public decimal Total { get; set; }
    }
}
