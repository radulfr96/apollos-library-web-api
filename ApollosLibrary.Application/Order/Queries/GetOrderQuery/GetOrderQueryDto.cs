using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Order.Queries.GetOrderQuery
{
    public class GetOrderQueryDto
    {
        public int OrderId { get; set; }
        public int BusinessId { get; set; }
        public LocalDateTime OrderDate { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
