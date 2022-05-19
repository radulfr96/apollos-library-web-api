using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Order
{
    public class OrderItemDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string eISBN { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
