using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    public class FictionType
    {
        [Key]
        public int TypeId { get; set; }
        public string Name { get; set; }
    }
}
