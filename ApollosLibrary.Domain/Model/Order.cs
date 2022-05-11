using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    # nullable disable

    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public int BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
