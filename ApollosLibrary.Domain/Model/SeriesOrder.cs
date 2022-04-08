using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

# nullable disable

namespace ApollosLibrary.Domain
{
    public class SeriesOrder
    {
        [Key]
        public int OrderId { get; set; }
        public int SeriesId { get; set; }
        public Series Series { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Number { get; set; }
    }
}
